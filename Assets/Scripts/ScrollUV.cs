using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA;

[RequireComponent(typeof(MeshRenderer))]
public class ScrollUV : MonoBehaviour
{
    public float Factor = 0.1f;

    private Vector3 _lastPosition;

	// Use this for initialization
	void Start ()
	{
	    _lastPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    MeshRenderer mr = GetComponent<MeshRenderer>();
	    var mat = mr.material; // access to first material of mr.materials

	    var movement = transform.position - _lastPosition;

	    var offset = mat.mainTextureOffset;

        // 0.1f, damit bei Faktor 1 der Hintergrund genau so schnell ist wie die Kamera
	    offset.x -= 0.1f * movement.x * Factor / transform.lossyScale.x;
	    offset.y -= 0.1f * movement.y * Factor / transform.lossyScale.z;
        
	    mat.mainTextureOffset = offset;

        // apply position
	    _lastPosition = transform.position;
	}

}
