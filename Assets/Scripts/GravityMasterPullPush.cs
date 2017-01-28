using System;
using UnityEngine;
using System.Collections;

public class GravityMasterPullPush : MonoBehaviour {

    public float ForceStrength = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        foreach (var gravityObject in GameObject.FindObjectsOfType<GravityObject>())
        {
            Rigidbody2D rigid2D;
            Rigidbody rigid;
            if ((rigid = gravityObject.gameObject.GetComponent<Rigidbody>()) != null)
            {
                Update3D(rigid);
            }
            else if((rigid2D = gravityObject.gameObject.GetComponent<Rigidbody2D>()) != null)
            {
                Update2D(rigid2D);
            }

        }
	}

    void Update2D(Rigidbody2D rigid)
    {
        var distance = rigid.transform.position - transform.position; // abstoßen

        var force = distance;
        force.Normalize();

        var direction = force;

	    var scaling = (1/distance.magnitude) * ForceStrength;

        if (distance.magnitude < this.transform.localScale.magnitude*2)
        {
	        force.Scale(new Vector3(scaling,scaling,scaling));
            direction.Scale(new Vector3(scaling, scaling, scaling));
            rigid.AddForce(force + direction);
        }
        else
        {
            scaling *= -1;
	        force.Scale(new Vector3(scaling,scaling,scaling));
            rigid.AddForce(force);
        }
    }

    
    void Update3D(Rigidbody rigid)
    {
        var distance = rigid.transform.position - transform.position; // abstoßen

        var force = distance;
        force.Normalize();

        var direction = force;

	    var scaling = (1/distance.magnitude) * ForceStrength;

        if (distance.magnitude < this.transform.localScale.magnitude*2)
        {
	        force.Scale(new Vector3(scaling,scaling,scaling));
            direction.Scale(new Vector3(scaling, scaling, scaling));
            rigid.AddForce(force + direction);
        }
        else
        {
            scaling *= -1;
	        force.Scale(new Vector3(scaling,scaling,scaling));
            rigid.AddForce(force);
        }
    }
}
