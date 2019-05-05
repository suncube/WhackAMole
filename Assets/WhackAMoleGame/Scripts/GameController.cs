using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace WhackAMoleGame
{
    [System.Serializable]
    public class AnimSpeedRange
    {
        public float Percent;
        public float AnimSpeed;
    }

    public class GameController : MonoBehaviour
    {
        [Header("Game Settings")]
        public bool isSurviveMode;
        public Image GameProgressImage;
        public Text Progress;
        public int ShowCount;
        public float MaxAnimSpeed;
        public AnimSpeedRange[] AnimSpeedRange;
        private float CurentShow;

        [Header("UI")]
        public GameObject StartButton;
        public GameObject ContinueButton;
        public Text Lives;
        public Image[] LivesProgress;
        public Text ScoreText;
        public Text BestScore;
        public Animator SceneAnimator;
        public Toggle SettingsButton;
        public GameObject SettingsPanel;
        
        [Header("ResultView")]
        public GameObject ResultWindow;
        public Text ResultScore;

        [Header("Game")]
        public int HitPointValue = 100;
        public Hole[] Holes;
        public HolePlayer HeroPlayer;
        public float MaxHeroWaitViewTime = 2f;

        public HolePlayer[] BulglarPlayer;
        public float MaxBulglarWaitViewTime = 2f;

        public int MaxLives = 5;
        private int curLives = 5;
        private int score;
        private bool isActiveGame = false;
        bool isInitialized = false;
        bool isPaused = false;
        // Use this for initialization
        void Start()
        {
            Initialize();
        }

        void Initialize()
        { 
            TouchManager.Runtime.OnHitObject += OnHitObject;
            ScoreText.text = "0";
            ResultWindow.SetActive(false);
            StartButton.SetActive(false);
            OpenCloseSettings();

            GameProgressImage.transform.parent.gameObject.SetActive(true);

            HeroPlayer.OnCatched += CatchPlayer;
            HeroPlayer.OnSetFree += ActivatePlayer;
            HeroPlayer.Hide();//!
            
            for (int i = 0; i < BulglarPlayer.Length; i++)
            {
                BulglarPlayer[i].Initialize(isSurviveMode);
                BulglarPlayer[i].OnCatched += CatchPlayer;
                BulglarPlayer[i].OnSetFree += ActivatePlayer;
            }
            
            if (Holes == null || Holes.Length == 0)
            {
                var components = gameObject.GetComponentsInChildren<Hole>();
                Holes = new Hole[components.Length];
                for (int index = 0; index < components.Length; index++)
                    Holes[index] = components[index];
            }

           
            ContinueButton.SetActive(false);

            HeroPlayer.gameObject.SetActive(false);
            for (int i = 0; i < BulglarPlayer.Length; i++)
                BulglarPlayer[i].gameObject.SetActive(false);

            for (int i = 0; i < Holes.Length; i++)
            {
                Holes[i].ResetHole();
            }
            SceneAnimator.Play("Show");
         
        }

        public void OpenCloseSettings()
        {
            SettingsPanel.SetActive(SettingsButton.isOn);
        }

        public void PauseGame()
        {
            Debug.Log("Pause");

            Stop();
            //
            if (isActiveGame)
                ContinueButton.SetActive(true);
        }

        void Stop()
        {
            isPaused = true;
            StopAllCoroutines();

            HeroPlayer.gameObject.SetActive(false);

            for (int i = 0; i < BulglarPlayer.Length; i++)
                BulglarPlayer[i].gameObject.SetActive(false);

            for (int i = 0; i < Holes.Length; i++)
            {
                Holes[i].ResetHole();
            }
            //
        }

        public void ShowViewAnimEnded()
        {
            StartButton.SetActive(true);
            isInitialized = true;
        }

        void Update()
        {
            GameProgressImage.fillAmount = CurentShow/ShowCount;
            Progress.text = ((int) ((CurentShow/ShowCount)*100)).ToString() +"%";
        }

        public void OnOpenHoles()
        {
            isActiveGame = true;
            Invoke("RunPlayers", 1f);
        }

        public void Continue()
        {
            isPaused = false;
            ContinueButton.SetActive(false);
            RunPlayers();
        }

        public void RunGame()
        {
            ResultWindow.SetActive(false);
            StartButton.SetActive(false);

            isPaused = false;
            CurentShow = 0;

            curLives = MaxLives;
            ScoreText.text = "0";
            score = 0;
            Lives.text = curLives.ToString();
            for (int i = 0; i < LivesProgress.Length; i++)
            {
                if(LivesProgress[i])
                    LivesProgress[i].gameObject.SetActive(true);
            }

            StopAllCoroutines();
            HeroPlayer.gameObject.SetActive(false);
            for (int i = 0; i < BulglarPlayer.Length; i++)
                BulglarPlayer[i].gameObject.SetActive(false);

            for (int i = 0; i < Holes.Length; i++)
            {
                Holes[i].ResetHole();
            }

            SceneAnimator.Play("Open");
        }

        public void RunPlayers()
        {
            isPaused = false;
            ContinueButton.SetActive(false);

            var range = GetFreeHoleId();
            Holes[range].ActivatePalyer(HeroPlayer, MaxHeroWaitViewTime);

            for (int i = 0; i < BulglarPlayer.Length; i++)
            {
                range = GetFreeHoleId();
                Holes[range].ActivatePalyer(BulglarPlayer[i], MaxBulglarWaitViewTime);
            }
        }
        public void EndGame()
        {
            Debug.Log("End GAME");
            isActiveGame = false;
            Stop();

            // todo - catch Colider off
            HeroPlayer.gameObject.SetActive(false);

            for (int i = 0; i < BulglarPlayer.Length; i++)
                BulglarPlayer[i].gameObject.SetActive(false);
        
            ResultWindow.SetActive(true);
            ResultScore.text = score.ToString();

            var bestScore = PlayerPrefs.GetInt("BestScore",0);
            if (bestScore < score)
            {
                bestScore = score;
                PlayerPrefs.SetInt("BestScore", bestScore);
                PlayerPrefs.Save();
                //new record
            }

            BestScore.text = bestScore.ToString();
            //
        }

        private void AddCurrentShow()
        {
            CurentShow++;

            if(!isSurviveMode) return;

            var percent = CurentShow / ShowCount;
            var speed = 1+(MaxAnimSpeed - 1)*percent;

            HeroPlayer.AnimationSpeed = speed;

            for (int i = 0; i < BulglarPlayer.Length; i++)
                BulglarPlayer[i].AnimationSpeed = speed;

        }
        private void ActivatePlayer(HolePlayer player)
        {
            if (!isActiveGame || isPaused) return;
           
            if (/*!isSurviveMode &&*/ player.PlayerType == PlayerType.Bulglar)
            {
                AddCurrentShow();
                if (CurentShow >= ShowCount)
                {
                    EndGame();
                    return;
                }
            }
            var range = GetFreeHoleId();
            Holes[range].ActivatePalyer(player, MaxHeroWaitViewTime);
        }
        private void OnHitObject(GameObject obj)
        {
            var holePlayer = obj.GetComponent<HolePlayer>();
            if(!holePlayer) return;

            holePlayer.Catch();
        }
        private void CatchPlayer(HolePlayer player, float time)
        {
            SoundManager.runtime.PlayCatch();
            if (player.PlayerType == PlayerType.Hero)
            {
                curLives --;
                ShowMinusLive(player.transform, -1);
            }
            if (player.PlayerType == PlayerType.Bulglar)
            {
                var addScore = (int) (HitPointValue* player.AnimationSpeed /(1 + time));
                ShowAddPointsEffect(player.transform,addScore);
                score += addScore;
            }
            if (curLives <= 0)
                EndGame();

            if(curLives<MaxLives && LivesProgress[curLives])
                LivesProgress[curLives].gameObject.SetActive(false);

            ScoreText.text = score.ToString();
            Lives.text = curLives.ToString();

        }
        int GetFreeHoleId()
        {
            var range = Random.Range(0, Holes.Length);
            while (Holes[range].IsLock)
                range = Random.Range(0, Holes.Length);
            return range;
        }

        public Transform AddPointEffect;
        public Transform MinusLiveEffect;

        public void ShowAddPointsEffect(Transform obj, int score)
        {
            var instantiate = Instantiate(AddPointEffect);
            instantiate.parent = obj.parent;
            instantiate.position = new Vector3();
            instantiate.rotation = new Quaternion();
            Destroy(instantiate.gameObject, 1.2f);
        }

        public void ShowMinusLive(Transform obj, int live)
        {
            var instantiate = Instantiate(MinusLiveEffect);
            instantiate.parent = obj.parent;
            instantiate.position = new Vector3();
            instantiate.rotation = new Quaternion();

            Destroy(instantiate.gameObject, 1.2f);
        }

        void OnDestroy()
        {
            PlayerPrefs.Save();
        }

        void OnEnable()
        {
            //
            if (!isInitialized)
            {
                SceneAnimator.Play("Show");
                return;
            }

            if(isActiveGame)
                ContinueButton.SetActive(true);
            
        }

        void OnDisable()
        {
            PauseGame();
        }

        public void ToMainMenu()
        {
            LoadingScene.LoadScene = "StartScene"; 
            Application.LoadLevel("Loading");
        }

    }

}