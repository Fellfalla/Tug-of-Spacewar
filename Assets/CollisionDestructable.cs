using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDestructable : MonoBehaviour
{

    public float MaxImpactVelocity;
    public GameObject Explosion;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        float maxSqForce = MaxImpactVelocity*MaxImpactVelocity;
        if (maxSqForce < coll.relativeVelocity.sqrMagnitude)
        {
            var instance = Object.Instantiate(Explosion, gameObject.transform.position, gameObject.transform.rotation);
            Object.Destroy(gameObject);
        }
    }
}
