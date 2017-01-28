using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FixedJoint2D))]
public class GrippingScript : MonoBehaviour
{
    private FixedJoint2D _grippingJoint;
    private GameObject _grippedGameObject;

	// Use this for initialization
	void Start ()
	{

        _grippingJoint = GetComponent<FixedJoint2D>();
	    _grippingJoint.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButton(1))
	    {
	        Ungrip(_grippedGameObject);
	    }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {

        if (coll.gameObject.GetComponent<Grippable>() != null)
        {
            Grip(coll.gameObject);
            
            // rotate Hook towards target
            Vector3 targetDir = coll.gameObject.transform.position - transform.position;
            //float step = speed * Time.deltaTime;
            Vector3 newDir = targetDir;
            Debug.DrawRay(transform.position, newDir, Color.red);
            //transform.rotation = Quaternion.LookRotation(targetDir);

            //var gripperToCamera = Camera.main.transform.position - transform.position;
            //var up = Vector3.Cross(gripperToCamera, targetDir);
            //transform.LookAt(coll.gameObject.transform, gripperToCamera);
        }
    }

    void Ungrip(GameObject toUngrip)
    {
        if (toUngrip != null)
        {
            _grippingJoint.connectedBody = null;
            _grippedGameObject = null;
            _grippingJoint.enabled = false;
        }

    }

    void Grip(GameObject toGrip)
    {

        _grippingJoint.connectedBody = toGrip.GetComponent<Rigidbody2D>();
        _grippedGameObject = toGrip;
        _grippingJoint.enabled = true;
    }
}
