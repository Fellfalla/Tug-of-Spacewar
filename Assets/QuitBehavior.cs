using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void Quit()
    {
        // Save game data

        // Close game
        Application.Quit();
    }
}
