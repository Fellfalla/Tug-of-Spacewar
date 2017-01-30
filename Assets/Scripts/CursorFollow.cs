using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Follows without timescale
/// </summary>
public class CursorFollow : MonoBehaviour
{

    public bool IgnoreTimescale = true;
    
    public float speed = 8.0f;
    public float distanceFromCamera = 5.0f;
    private double _lastTime;



    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {

	    if (IgnoreTimescale)
	    {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = distanceFromCamera;

            Vector3 mouseScreenToWorld = Camera.main.ScreenToWorldPoint(mousePosition);

            var delta = Time.realtimeSinceStartup - _lastTime;
            Vector3 position = Vector3.Lerp(transform.position, mouseScreenToWorld, 1.0f - Mathf.Exp(-speed * (float)delta));

            transform.position = position;
	        _lastTime = Time.realtimeSinceStartup;
	    }
	    else
	    {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = distanceFromCamera;

            Vector3 mouseScreenToWorld = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector3 position = Vector3.Lerp(transform.position, mouseScreenToWorld, 1.0f - Mathf.Exp(-speed * Time.deltaTime));

            transform.position = position;
        }



    }
}
