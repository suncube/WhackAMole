using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public static string LoadScene = "StartScene";
    public static float LoadSpeed = 25f;


    public Text LoadingProgress;
    public Image LoadingImage;
    public string StartLoadScene;
    public Texture texture_loading;
    public bool isAutoLoad;

    private AsyncOperation AsOp;
    private float loading_progress = 0;
    private int Round_load;
    
    void Start ()
    {
        var level_Name = LoadScene;
        if (!string.IsNullOrEmpty(StartLoadScene))
            level_Name = StartLoadScene;

       AsOp = Application.LoadLevelAsync(level_Name); 
       AsOp.allowSceneActivation = false; 

    }

    void OnGUI()
    {

        if (loading_progress <= 100)
        {
            loading_progress += Time.deltaTime * LoadSpeed;
            Round_load = Mathf.RoundToInt(loading_progress); 
        }

        Debug.Log(AsOp.progress+ " " + Round_load);
        if (Math.Abs(AsOp.progress - 0.9) < 0.05 && Round_load == 100)
        {
            if (isAutoLoad)
            {
                AsOp.allowSceneActivation = true;
            }
            else
            {
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 200, 300, 300), " Press any button");

                if (Input.anyKey)
                {  
                    AsOp.allowSceneActivation = true; 
                }
            }
           
        }


        if (AsOp != null)
        { 
            LoadingProgress.text = Round_load.ToString() + "%";
            LoadingImage.fillAmount = loading_progress/100;
        }



    }
}
