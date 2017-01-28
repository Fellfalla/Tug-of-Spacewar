using System;
using Assets.PropertyAttributes;
using UnityEngine;

namespace Assets.Scripts
{
    public class Thruster : MonoBehaviour
    {

        [RangeDouble(0, Math.PI/2)]
        public double MaxSteering = 0;

        /// <summary>
        /// Power of thrust in percent
        /// </summary>
        [HideInInspector]
        [Range(0,1)]
        public int Power;

        public bool InvertXY = false;
    
        public uint Thrust;

        [HideInInspector]
        public bool IsActive = false;
        [HideInInspector]
        public bool LeftSteering = false;
        [HideInInspector]
        public bool RightSteering = false;

        public ParticleSystem[] ParticleSystems;

        private double _direction;

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        void Update ()
        {
            IsActive = Input.GetKey(KeyCode.W);
            RightSteering = Input.GetKey(KeyCode.D);
            LeftSteering = Input.GetKey(KeyCode.A);

            //var oldEmission = ParticleSystem.emission;
            //oldEmission.enabled = true;
            if (IsActive)
            {
                Power = 1;
                //if (ParticleSystem != null)
                //{
                //    ParticleSystem.Emit();
                //}

                foreach (var pSystem in ParticleSystems)
                {
                    pSystem.Play(); // = oldEmission;
                }

            }
            else
            {
                Power = 0;
            
                foreach (var pSystem in ParticleSystems)
                {
                    pSystem.Stop(); // = oldEmission;
                }
                //if (ParticleSystem != null)
                //{
                //    ParticleSystem.Emit(0);
                //}
            }



            if (RightSteering)
            {
                _direction = -MaxSteering;
            }
            if (LeftSteering)
            {
                _direction = MaxSteering;
            }

            var xValue = Math.Cos(_direction)*Power*Thrust;
            var yValue = Math.Sin(_direction)*Power*Thrust;

            if (InvertXY)
            {
                var temp = xValue;
                xValue = yValue;
                yValue = temp;
            }

            var forceVektor = new Vector2((float)xValue , (float) yValue);
            this.gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(forceVektor);
        }
    }
}
