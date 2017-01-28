using UnityEngine;

namespace Assets.Scripts
{
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


            float distance = transform.position.z - Camera.main.transform.position.z;
            var targetPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            targetPos = Camera.main.ScreenToWorldPoint(targetPos);
            var direction = targetPos - transform.position;
            Debug.DrawLine(transform.position, targetPos, Color.red);

            if (Input.GetMouseButtonDown(0))
            {
                Vector2 destiny = targetPos;

                _curHook = Instantiate(HookPrefab, transform.position, Quaternion.identity);

                _curHook.GetComponent<Rope>().Thrower = gameObject;
                _curHook.GetComponent<Rope>().Destiny = destiny;



                var throwForce =  direction *ThrowForce;
       
                _curHook.GetComponent<Rigidbody2D>().AddForce(throwForce);
            }

        }
    }
}
