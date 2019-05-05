using UnityEngine;
using System.Collections;

public class StartSceneController : MonoBehaviour {

    public void LoadVRGame()
    {
        LoadingScene.LoadScene = "VRGame";
        Application.LoadLevel("Loading");
        LoadingScene.LoadSpeed = 7;
    }

    public void LoadGame()
    {
        LoadingScene.LoadScene = "Game";
        LoadingScene.LoadSpeed = 10;
        Application.LoadLevel("Loading");
    }
}
