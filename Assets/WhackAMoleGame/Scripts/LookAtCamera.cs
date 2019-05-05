using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour
{
    public Transform Target;

	void Start ()
	{
	    Target = Camera.main.transform;
	}

	void Update () {
       if(Target==null) return;
       transform.LookAt(Target);
	}
}
