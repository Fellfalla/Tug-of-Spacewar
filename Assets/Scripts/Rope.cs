using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{

    public Vector2 Destiny;
    public float Speed = 1;

    public float Distance = 2;

    public int MaxLength = 10;

    public GameObject NodePrefab;

    public GameObject Thrower;

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

	    //transform.position = Vector2.MoveTowards(transform.position, Destiny, Speed);

	    //if (!_shot)
	    //{
	    //    _shot = true;
     //       GetComponent<Rigidbody2D>().AddForce(Vector2.MoveTowards(transform.position, Destiny, Speed));
	    //}



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

            LastNode.GetComponent<HingeJoint2D>().connectedBody = Thrower.GetComponent<Rigidbody2D>();
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
        LastNode.GetComponent<HingeJoint2D>().connectedBody = newNode.GetComponent<Rigidbody2D>();

        LastNode = newNode;
        _curLength++;
    }
}
