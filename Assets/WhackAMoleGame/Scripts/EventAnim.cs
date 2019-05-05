using UnityEngine;
using WhackAMoleGame;

public class EventAnim : MonoBehaviour
{
    public GameController Controller;
    public void AnimEnded()
    {
        Controller.ShowViewAnimEnded();
    }
    public void OpenEnded()
    {
        Controller.OnOpenHoles();
    }
}