using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeUnscaler : MonoBehaviour
{
    private double _lastTime;
    public GameObject[] ParticleSystems;
	
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Time.timeScale < 0.01f)
        {
            float deltaTime = Time.realtimeSinceStartup - (float)_lastTime;

            foreach (var gameobject in ParticleSystems)
            {
                if (gameobject != null)
                {
                    gameobject.GetComponent<ParticleSystem>().Simulate(deltaTime, true, false);
                }
            }
            _lastTime = Time.realtimeSinceStartup;

        }
        else
        {
            float deltaTime = Time.realtimeSinceStartup - (float)_lastTime;

            foreach (var gameobject in ParticleSystems)
            {
                if (gameobject != null)
                {
                    gameobject.GetComponent<ParticleSystem>().Simulate(deltaTime, true, false);
                }
            }
            _lastTime = Time.realtimeSinceStartup;

        }
    }
}
