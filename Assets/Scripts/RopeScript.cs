using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeScript : MonoBehaviour
{

    public GameObject StartAnchor;

    public GameObject EndAnchor;

    public float Length = 1;

    public float Mass = 1;


    [Range(0.0001f, 1)]
    public float Damping = 0.01f;

    [Range(0.001f, 100)]
    public float Elasticity = 0.001f;

    [Range(1, 100)]
    public int SegmentCount;

    private GameObject[] _segments;

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
	    InitSegments();
		UpdateLineRendererPositions();

	}

    void InitSegments()
    {
        // Create Nodes
        _segments = new GameObject[SegmentCount];
	    for (int i = 0; i < SegmentCount; i++)
	    {
	        _segments[i] = InstanciateSegment(i);

            // interpolate positions
	        //_segments[i].transform.position = (EndAnchor.position - StartAnchor.position)*((float)i/SegmentCount);
	    }
        // Create node rendering
	    GetComponent<LineRenderer>().numPositions = SegmentCount + 2; // +2 cause of start and end point
    }

    GameObject InstanciateSegment(int number)
    {
        var segment = new GameObject("procedural_rope_segment");
        //segment.tr
        segment.transform.position = Vector3.Lerp(StartAnchor.transform.position, EndAnchor.transform.position, ((float) number) / SegmentCount);
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
	        InitSegments();
	    }
        UpdateLineRendererPositions();
		
	}

    void FixedUpdate()
    {
	    AdjustDistance(StartAnchor, _segments[0]);
        
        // Simulate Forces
	    for (int i = 0; i < _segments.Length - 1; i++)
	    {
	        var seg1 = _segments[i];
	        var seg2 = _segments[i+1];
	        AdjustDistance(seg1, seg2);
	        Damp(seg1, seg2);
	    }

	    AdjustDistance(_segments[_segments.Length-1], EndAnchor);
    }

    /// <summary>
    /// moves <param name="obj2"/> in direction of <param name="obj1"/>
    /// until <see cref="_lengthDelta"/> is not exceeded
    /// </summary>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    void AdjustDistance(GameObject obj1, GameObject obj2)
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
	    exceeding = exceeding - obj2.transform.position;
        Debug.DrawLine(obj2.transform.position, obj1.transform.position + exceeding, Color.blue);
        obj2.GetComponent<Rigidbody2D>().AddForce(exceeding/Elasticity);
        obj1.GetComponent<Rigidbody2D>().AddForce(exceeding/Elasticity);
	    //obj2.transform.Translate(Vector3.Lerp(Vector3.zero, exceeding, Elasticity));
	    //}
    }

    void Damp(GameObject obj1, GameObject obj2)
    {
        // Get relative velocity
        var vel1 = obj2.GetComponent<Rigidbody2D>().velocity;
        var vel2 = obj1.GetComponent<Rigidbody2D>().velocity;
        var relativeVelocity = vel2 - vel1;

        var dampingAmout = relativeVelocity.sqrMagnitude*Damping;
        obj2.GetComponent<Rigidbody2D>().AddForce(- vel2 * dampingAmout);
        obj1.GetComponent<Rigidbody2D>().AddForce(- vel1 * dampingAmout);
    }

    void UpdateLineRendererPositions()
    {
        int numPositions = GetComponent<LineRenderer>().numPositions;
		GetComponent<LineRenderer>().SetPosition(0, StartAnchor.transform.position);
        for (int i = 0; i < SegmentCount; i++)
		{
		    GetComponent<LineRenderer>().SetPosition(i + 1, _segments[i].transform.position);
		}
		GetComponent<LineRenderer>().SetPosition(numPositions - 1, EndAnchor.transform.position);
    }
}
