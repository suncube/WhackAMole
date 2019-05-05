using UnityEngine;
using System.Collections;
using WhackAMoleGame;

public class EventResever : MonoBehaviour
{
    public HolePlayer HolePlayer;

    public void StartHide()
    {
        HolePlayer.StartHide();
    }

    public void Hide()
    {
        HolePlayer.Hide();
    }
}