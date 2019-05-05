using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WhackAMoleGame
{
    public class HolePlayer : MonoBehaviour
    {
        public Animator Animator;
        public float MinAnimatedSpeed = 1;
        public float AnimationSpeed = 2;
        public PlayerType PlayerType;
        public GameObject StarsEffect;
        public Action<HolePlayer> OnHide;
        public Action<HolePlayer> OnSetFree;
        public Action<HolePlayer, float> OnCatched;
        private bool isSurviveMode;

        void Start()
        {
            StarsEffect.SetActive(false);
        }

        public void Initialize(bool isSurvive)
        {
            isSurviveMode = isSurvive;
        }

        private bool isCatch = false;
        private float showtTime;

        public void Show()
        {
            StarsEffect.SetActive(false);
            showtTime = 0;
            isCatch = false;
            Animator.Play("Show");

           if(!isSurviveMode)
                Animator.speed = Random.Range(MinAnimatedSpeed, AnimationSpeed);
            else
                Animator.speed = AnimationSpeed;
        }

        public void StartHide()
        {
            isCatch = true;
            StarsEffect.SetActive(false);
        }

        public void Hide()
        {
            if (OnHide != null)
                OnHide(this);
        }

        public void SetFree()
        {
            if (OnSetFree != null)
                OnSetFree(this);
        }

        void Update()
        {
            showtTime += Time.deltaTime;
        }

        public void Catch()
        {
            if(isCatch) return;
            isCatch = true;

            StarsEffect.SetActive(true);
            Animator.Play("Catch");

            if (OnCatched != null)
                OnCatched(this, showtTime);

        }
    }
}