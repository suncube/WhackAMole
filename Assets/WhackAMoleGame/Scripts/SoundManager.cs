using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Toggle SoundOffOn;
    public static SoundManager runtime;
    public AudioListener AudioSource;
    public AudioSource MainMusic;

    public Transform[] Sounds;
    private bool _isActivate;
	// Use this for initialization
	void Awake ()
	{
	    runtime = this;
        var i = PlayerPrefs.GetInt("SoundActivate",0);
	    ActivateSounds(i > 0 ? false : true);
	    SoundOffOn.isOn = !_isActivate;
	}

    public void ChangeSound()
    {
        ActivateSounds(!SoundOffOn.isOn);
    }

    public void ActivateSounds(bool isActivate)
    {
        _isActivate = isActivate;
        MainMusic.volume = _isActivate ? 0.2f : 0;
        PlayerPrefs.SetInt("SoundActivate",_isActivate ? 0: 1);
        PlayerPrefs.Save();
    }
  
    public void PlayCatch()
    {
        if(!_isActivate) return;
        var range = Random.Range(0, Sounds.Length - 1);
        var instantiate = Instantiate(Sounds[range]);
        Destroy(instantiate.gameObject,2f);
    }

    void OnDestroy()
    {
        runtime = null;
        PlayerPrefs.Save();
    }
}
