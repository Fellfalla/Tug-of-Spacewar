using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRope : MonoBehaviour {

    public GameObject HookPrefab;
    public GameObject RopePrefab;

    public float ThrowForce = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        float distance = transform.position.z - Camera.main.transform.position.z;
        var targetPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        targetPos = Camera.main.ScreenToWorldPoint(targetPos);
        var direction = targetPos - transform.position;
        Debug.DrawLine(transform.position, targetPos, Color.red);

	    if (Input.GetMouseButtonDown(0))
	    {

	        var hook = Instantiate(HookPrefab, transform.position, Quaternion.identity);

            var offset = direction;
            offset.Normalize();
            offset.Scale(hook.GetComponent<Collider2D>().bounds.size);
            hook.transform.Translate(offset);
            hook.SetActive(true);
	        var throwForce = direction*ThrowForce;

	        hook.GetComponent<Rigidbody2D>().AddForce(throwForce);

            var rope = Instantiate(RopePrefab, transform.position, Quaternion.identity);
            rope.GetComponent<RopeScript>().StartAnchor = gameObject;
            rope.GetComponent<RopeScript>().EndAnchor = hook;
            rope.SetActive(true);
	    }
	}

}
