using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Rope : MonoBehaviour
{

    public Vector2 Destiny;
    public float Speed = 1;

    public float Distance = 2;

    public int MaxLength = 10;

    public GameObject NodePrefab;

    [HideInInspector()]
    public GameObject Thrower;

    [HideInInspector()]
    public GameObject LastNode;

    private bool _done = false;
    private bool _shot = false;
    private int _curLength = 0;
	// Use this for initialization
	void Start () {
		//Thrower = GameObject.FindGameObjectWithTag("Player");
	    LastNode = this.gameObject;

	}
	
	// Update is called once per frame
	void Update ()
	{

	    if (_curLength < MaxLength) //(Vector2) transform.position != Destiny)
	    {
	        var distanceLastNodeToThrower = Vector2.Distance(Thrower.transform.position, LastNode.transform.position);
	        if ( distanceLastNodeToThrower > Distance)
	        {
	            // Instanciate new Rope Node
                CreateNode();
	        }
	    }
	    else if (_done == false)
	    {
	        _done = true;
            if (LastNode.GetComponent<HingeJoint2D>() == null)
            {
                AddJoind2D(LastNode);
            }
            LastNode.GetComponent<HingeJoint2D>().connectedBody = Thrower.GetComponent<Rigidbody2D>();
            //Thrower.GetComponent<Joint2D>().connectedBody = LastNode.GetComponent<Rigidbody2D>();
            //LastNode.transform.SetParent(Thrower.transform);
	    }

	}


    void CreateNode()
    {
        Vector2 pos2Create = Thrower.transform.position - LastNode.transform.position;

        pos2Create.Normalize();
        pos2Create *= Distance;
        pos2Create += (Vector2) LastNode.transform.position;

        GameObject newNode = Instantiate(NodePrefab, pos2Create, Quaternion.identity);

        //LastNode.transform.SetParent(newNode.transform);

        // Connect last Hinge
        if (LastNode.GetComponent<HingeJoint2D>() == null)
        {
            AddJoind2D(LastNode);
        }

        LastNode.GetComponent<HingeJoint2D>().connectedBody = newNode.GetComponent<Rigidbody2D>();
        //LastNode.GetComponent<DistanceJoint2D>().connectedBody = newNode.GetComponent<Rigidbody2D>();

        LastNode = newNode;
        _curLength++;
    }

    private void AddJoind2D(GameObject targetGameObject)
    {
        targetGameObject.AddComponent<HingeJoint2D>();
        
        //Configure
        LastNode.GetComponent<HingeJoint2D>().autoConfigureConnectedAnchor = false;
        //LastNode.GetComponent<HingeJoint2D>().maxDistanceOnly = true;
        //LastNode.GetComponent<HingeJoint2D>().distance = Distance;
        //LastNode.GetComponent<HingeJoint2D>().autoConfigureDistance = false;

    }
}
