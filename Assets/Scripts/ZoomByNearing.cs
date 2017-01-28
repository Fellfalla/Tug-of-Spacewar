using System;
using Assets.Scripts;
using UnityEngine;

namespace Assets
{
    public class ZoomByNearing : MonoBehaviour
    {
        private Vector3 _translation = Vector3.zero;

        public Transform Target;
        public float rate = 0.15f;
        public float MinDistance = 0.5f;
        private float _sqrMinDistance;
        private float _sqrMaxDistance;
        public float MaxDistance = 1000;

        [Range(0,1)]
        public float ZoomDamping = 1;
        // Use this for initialization
        void Start ()
        {
            _sqrMinDistance = MinDistance*MinDistance;
            _sqrMaxDistance = MaxDistance*MaxDistance;

            var followingComponent = GetComponent<FollowingCamera>();
            if (Target == null && followingComponent != null)
            {
                this.Target = followingComponent.Target;
            }
        }
	
        // Update is called once per frame
        void LateUpdate () {
            if (Target != null)
            {
                // distance from this to target
                var distance = Target.transform.position - transform.position;

                var input = Input.GetAxis("Mouse ScrollWheel");

                var intertiaTranslation = _translation - _translation * ZoomDamping;
                
                var newTranslation = distance * rate * input;

                _translation = intertiaTranslation + newTranslation;

                var expectedDistance = distance.sqrMagnitude - Math.Sign(input) * _translation.sqrMagnitude;

                if (expectedDistance < _sqrMinDistance && (input > 0 || intertiaTranslation.sqrMagnitude > 0))
                {
                    // too near
                    _translation = Vector3.zero;
                }
                else if (expectedDistance > _sqrMaxDistance && (input < 0 || intertiaTranslation.sqrMagnitude > 0))
                {
                    // too far
                    _translation = Vector3.zero;
                
                }

                transform.Translate(_translation);
                //}


            }
        }
    }
}
