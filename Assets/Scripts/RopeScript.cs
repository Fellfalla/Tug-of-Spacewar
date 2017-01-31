using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeScript : MonoBehaviour
{

    public GameObject StartAnchor;

    public GameObject EndAnchor;

    public float Length = 1;

    public float Mass = 1;


    [Range(0.0f, 1)]
    public float Damping = 0.01f;

    [Range(0.001f, 5)]
    public float Elasticity = 0.001f;

    [Range(2, 50)]
    public int SegmentCount;

    private GameObject[] _segments = new GameObject[0];

    /// <summary>
    /// mass of a node
    /// </summary>
    /// <returns></returns>
    float GetMassDelta()
    {
        return Mass/SegmentCount;
    }

    /// <summary>
    /// distance between two nodes
    /// </summary>
    /// <returns></returns>
    float GetLengthDelta()
    {
        return Length/SegmentCount;
    }

	// Use this for initialization
	void Start ()
	{
	    RefreshSegments();
		UpdateLineRendererPositions();

	}

    void RefreshSegments()
    {
        // create local variables
        var newSegmentArray = new GameObject[SegmentCount];
        int previousSegmentCount = _segments.Length;
        int reusableCount = Math.Min(SegmentCount, previousSegmentCount); // get length of reusable segments
        for (int i = 0; i < reusableCount; i++)
        {
            newSegmentArray[i] = _segments[i];
            _segments[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            _segments[i].transform.parent = null;
        }

        // assign reusable segments and new array size
        _segments = newSegmentArray;
        
        // Create missing Nodes
        for (int i = reusableCount; i < SegmentCount; i++)
	    {

            var startEndPosition = 1;
            _segments[i] = InstanciateSegment(startEndPosition);

        }

        //_segments[0].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        //_segments[0].transform.parent = StartAnchor.transform;
        //_segments[_segments.Length-1].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        //_segments[_segments.Length-1].transform.parent = EndAnchor.transform;

        // Create node rendering
	    GetComponent<LineRenderer>().numPositions = SegmentCount; 
    }

    GameObject InstanciateSegment(float startEndPosition)
    {
        var segment = new GameObject("procedural_rope_segment");
        //segment.tr
        segment.transform.position = Vector3.Lerp(StartAnchor.transform.position, EndAnchor.transform.position, startEndPosition);
	    segment.AddComponent<Rigidbody2D>();
	    segment.GetComponent<Rigidbody2D>().mass = GetMassDelta();
	    segment.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
	    segment.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        return segment;
    }

	// Update is called once per frame
	void Update () {
	    if (_segments.Length != SegmentCount)
	    {
	        RefreshSegments();
	    }
        UpdateLineRendererPositions();
		
	}

    void FixedUpdate()
    {

        


        // Simulate Forces
        for (int i = 1; i < _segments.Length - 2; i++)
        {
            var seg1 = _segments[i];
            var seg2 = _segments[i + 1];
            SimulateForces(seg1, seg2);
            Damp(seg1, seg2);
        }

        if (StartAnchor != null && EndAnchor != null && _segments.Length == 2)
        {
            SimulateForces(StartAnchor, EndAnchor);
        }

        // distiguinsh if anchor point exists or not
        if (StartAnchor != null)
        {
            SimulateForces(StartAnchor, _segments[1]);
            _segments[0].transform.position = StartAnchor.transform.position;
        }

        else
        {
             SimulateForces(_segments[0], _segments[1]);
        }

        if (EndAnchor != null)
        {
            SimulateForces(EndAnchor, _segments[_segments.Length-2]);
	        _segments[_segments.Length - 1].transform.position = EndAnchor.transform.position;
        }
        else
        {
             SimulateForces(_segments[_segments.Length-2], _segments[_segments.Length - 1]);
        }
    }

    /// <summary>
    /// moves <param name="obj2"/> in direction of <param name="obj1"/>
    /// until <see cref="GetLengthDelta()"/> is not exceeded
    /// </summary>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    void SimulateForces(GameObject obj1, GameObject obj2)
    {
        var distanceVector = obj2.transform.position - obj1.transform.position;
	    //if (distanceVector.sqrMagnitude > _lengthDeltaSq)
	    //{

	    var exceeding = distanceVector;
        if (exceeding.magnitude < GetLengthDelta())
        {
            return;
        }

        exceeding.Normalize();
	    exceeding = exceeding*GetLengthDelta();
	    exceeding = - obj2.transform.position + exceeding + obj1.transform.position;

        var force2 = exceeding/Elasticity;
        var force1 = -force2;

        obj2.GetComponent<Rigidbody2D>().AddForce(force2);
        obj1.GetComponent<Rigidbody2D>().AddForce(force1);

        Debug.DrawRay(obj2.transform.position, force2/100, Color.blue);
        Debug.DrawRay(obj1.transform.position, force1/100, Color.blue);
	    
        //obj2.transform.Translate(Vector3.Lerp(Vector3.zero, exceeding, Elasticity));
	    //}
    }

    void Damp(GameObject obj1, GameObject obj2)
    {
        // Get relative velocity
        var vel1 = obj2.GetComponent<Rigidbody2D>().velocity;
        var vel2 = obj1.GetComponent<Rigidbody2D>().velocity;
        var relativeVelocity = vel2 - vel1;

        //var dampingAmout = relativeVelocity.sqrMagnitude*Damping;

        var dampingForce1 = - relativeVelocity * Damping;
        var dampingForce2 = -dampingForce1;

        Debug.DrawRay(obj1.transform.position, dampingForce1, Color.blue);
        Debug.DrawRay(obj2.transform.position, dampingForce2, Color.blue);

        obj1.GetComponent<Rigidbody2D>().AddForce(dampingForce1);
        obj2.GetComponent<Rigidbody2D>().AddForce(dampingForce2);
    }

    void UpdateLineRendererPositions()
    {
        int positions = _segments.Length;
        int curPos = 0;
        GetComponent<LineRenderer>().numPositions = positions;

        //if (StartAnchor != null)
        //{
        //    positions++;
        //    GetComponent<LineRenderer>().numPositions = positions;
        //    GetComponent<LineRenderer>().SetPosition(0, StartAnchor.transform.position);
        //}

        //if (EndAnchor != null)
        //{
        //    positions++;
        //    GetComponent<LineRenderer>().numPositions = positions;
        //    GetComponent<LineRenderer>().SetPosition(positions - 1, EndAnchor.transform.position);
        //}

        for (int i = 0; i < _segments.Length; i++)
		{
		    GetComponent<LineRenderer>().SetPosition(curPos, _segments[i].transform.position);
		    curPos++;
		}


    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < _segments.Length; i++)
		{
            Gizmos.color = Color.red;
            Handles.Label(_segments[i].transform.position, i.ToString());
            Gizmos.DrawSphere(_segments[i].transform.position, 0.1f);
		}
    }
}
