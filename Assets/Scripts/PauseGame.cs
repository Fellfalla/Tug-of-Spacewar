using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{

    public KeyCode Key = KeyCode.Escape;
    public GameObject[] EnabledDisabledGameObjects;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(Key))
	    {
	        foreach (var gameObj in EnabledDisabledGameObjects)
	        {
                gameObj.SetActive(!gameObj.activeInHierarchy);
            }
            //toggle
        }

	}
}
