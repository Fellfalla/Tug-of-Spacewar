using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoCursor : MonoBehaviour {



	// Use this for initialization
	void Start () {
		//Cursor.SetCursor( null, Vector2.zero, CursorMode.Auto);
	    UnityEngine.Cursor.visible = false;
	}

    void Reset()
    {
	    UnityEngine.Cursor.visible = true;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
