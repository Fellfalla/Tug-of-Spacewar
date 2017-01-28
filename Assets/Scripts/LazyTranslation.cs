using UnityEngine;

namespace Assets.Scripts
{
    public class LazyTranslation : MonoBehaviour {

        public float Factor = 0.1f;
        public bool InvertX = false;
        public bool InvertY = false;


        private Vector3 _lastPosition;

        // Use this for initialization
        void Start ()
        {
            _lastPosition = transform.parent.position;
        }

	
        // Update is called once per frame
        void Update ()
        {
            var parentPos = transform.parent.position;
            var movement = parentPos - _lastPosition;

            // 0.1f, damit bei Faktor 1 der Hintergrund genau so schnell ist wie die Kamera
            var offset = Vector3.zero;
            offset.x -= movement.x*Factor;
            offset.y -= movement.y*Factor;

            if (InvertX)
            {
                offset.x *= -1;
            }

            if (InvertY)
            {
                offset.y *= -1;
            }

            offset = transform.InverseTransformDirection(offset);
            transform.Translate(offset);


            // apply position
            _lastPosition = parentPos;
        }
    }
}
