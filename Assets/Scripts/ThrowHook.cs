using UnityEngine;

public class ThrowHook : MonoBehaviour
{

    public GameObject HookPrefab;
    public float ThrowForce = 1;

    GameObject _curHook;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var targetPos = Input.mousePosition;
	    targetPos.z = this.transform.position.z;
        targetPos = Camera.main.ScreenToWorldPoint(targetPos);

        Vector3 pz = Camera.main.ViewportToWorldPoint(Input.mousePosition);

        Debug.DrawLine(transform.position, pz, Color.red);

	    if (Input.GetMouseButtonDown(0))
	    {
	        var screenPoint = Input.mousePosition;
	        screenPoint.z = transform.position.z;
	        //Vector2 destiny = Camera.main.ViewportToWorldPoint(screenPoint);
	        Vector2 destiny = transform.position + new Vector3(0, 20, 0);

            _curHook = Instantiate(HookPrefab, transform.position, Quaternion.identity);
            //_curHook = Instantiate(HookPrefab, transform.position + new Vector3(0, 20, 0), Quaternion.identity);


            _curHook.GetComponent<Rope>().Thrower = gameObject;
            _curHook.GetComponent<Rope>().Destiny = destiny;



	        var throwForce = Vector2.MoveTowards(transform.position, destiny, ThrowForce);
       
            _curHook.GetComponent<Rigidbody2D>().velocity = throwForce;
	    }

	}
}
