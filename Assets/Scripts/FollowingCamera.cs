using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent (typeof (Camera))]
    public class FollowingCamera : MonoBehaviour
    {
        private Camera _camera;
        private Vector3 _oldTargetPosition;
        private Vector3 _oldPosition;
        private Vector3 _desiredTransform;
        public Transform Target;

        public float Damping = 2f;
        public float CameraIntertia = 1f;

        // Use this for initialization
        void Start ()
        {
            _camera = this.GetComponent<Camera>();
            _oldTargetPosition = Target.position;
            _desiredTransform = transform.position;
            _oldPosition = transform.position;
        }
	
        // Update is called once per frame
        void Update ()
        {


        }

        void LateUpdate()
        {
            // Early out if we don't have a target
            if (Target == null)
                return;
            else
            {
                var target = Target;
                var targetMovement = target.position - _oldTargetPosition;
                var speed = (transform.position - _oldPosition);
                var speedTranslation = speed*CameraIntertia;
                _oldPosition = transform.position;
                //var oldPosition = this.transform.position;
                //var newPosition = new Vector3(Target.transform.position.x, Target.transform.position.y,
                //    oldPosition.z);


                _desiredTransform += targetMovement;
            

                var xNext = Mathf.Lerp(transform.position.x, _desiredTransform.x, Damping * Time.deltaTime);
                var yNext = Mathf.Lerp(transform.position.y, _desiredTransform.y, Damping * Time.deltaTime);
                var zNext = Mathf.Lerp(transform.position.z, _desiredTransform.z, Damping * Time.deltaTime);

                var nextPoint = new Vector3(xNext, yNext, zNext);
                var translation = nextPoint - transform.position;

                //this.transform.Translate(translation);
                //this.transform.Translate(speedTranslation);

                this.transform.Translate(targetMovement);

                // Always look at the target
                //transform.LookAt(target);

                // Init varaibles for next update
                _oldTargetPosition = target.position;
            }
        }
    }
}
