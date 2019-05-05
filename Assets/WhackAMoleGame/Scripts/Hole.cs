using UnityEngine;

namespace WhackAMoleGame
{
    public class Hole : MonoBehaviour
    {
        public bool IsLock { get; private set; }
        private HolePlayer currentPlayer;

        public void ActivatePalyer(HolePlayer player, float time = 1f)
        {
            IsLock = true;

            currentPlayer = player;
            currentPlayer.transform.parent = transform;
            currentPlayer.transform.localPosition = new Vector3();

            currentPlayer.OnHide += OnHidePlayer;

            Invoke("ActivateHole", Random.Range(0.2f,time));
        }

        public void ActivateHole()
        {
            if (!currentPlayer) return;
            currentPlayer.gameObject.SetActive(true);
            currentPlayer.Show();
        }

        public void ResetHole()
        {
           
            if (!currentPlayer) return;

            currentPlayer.OnHide -= OnHidePlayer;
            currentPlayer.gameObject.SetActive(false);
            currentPlayer = null;
            IsLock = false;
        }

        private void OnHidePlayer(HolePlayer player)
        {

            currentPlayer.OnHide -= OnHidePlayer;
            currentPlayer.gameObject.SetActive(false);
            currentPlayer.SetFree();
            currentPlayer = null;
            IsLock = false;
        }

    }
}