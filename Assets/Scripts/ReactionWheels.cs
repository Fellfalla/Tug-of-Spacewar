using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionWheels : MonoBehaviour
{

    public int Force;

    [HideInInspector]
    public bool IsActive = false;
    [HideInInspector]
    public bool LeftSteering = false;
    [HideInInspector]
    public bool RightSteering = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    IsActive = Input.GetKey(KeyCode.W);
	    RightSteering = Input.GetKey(KeyCode.D);
	    LeftSteering = Input.GetKey(KeyCode.A);

	    var torque = 0;

	    if (LeftSteering)
	    {
	        torque = Force;
	    }
	    if (RightSteering)
	    {
	        torque = -Force;
	    }

        this.gameObject.GetComponent<Rigidbody2D>().AddTorque(torque);
	}
}
