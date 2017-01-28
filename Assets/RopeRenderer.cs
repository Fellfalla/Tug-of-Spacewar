using UnityEngine;

namespace Assets
{
    [RequireComponent(typeof(LineRenderer), typeof(Joint2D))]
    public class RopeRenderer : MonoBehaviour {

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        void Update ()
        {
            var lineRenderer = GetComponent<LineRenderer>();

            lineRenderer.widthMultiplier = 0.1f;
            lineRenderer.numPositions = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, GetComponent<Joint2D>().connectedBody.GetComponentInParent<Transform>().position);
        }
    }
}
