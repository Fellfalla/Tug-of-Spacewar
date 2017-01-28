using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{

    /// <summary>
    /// In Rotations per minute
    /// </summary>
    public float RotationSpeedX = 0;
    public float RotationSpeedY = 0;
    public float RotationSpeedZ = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
        
	    var rotationX = convertRpmToDegAngle(RotationSpeedX, Time.deltaTime);
	    var rotationY = convertRpmToDegAngle(RotationSpeedY, Time.deltaTime);;
	    var rotationZ = convertRpmToDegAngle(RotationSpeedZ, Time.deltaTime);;

		transform.Rotate(new Vector3(1,0,0), rotationX);

		transform.Rotate(new Vector3(0,1,0), rotationY);

		transform.Rotate(new Vector3(0,0,1), rotationZ);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rotSpeed"></param>
    /// <param name="delta"></param>
    /// <returns>Angle in rad</returns>
    private float convertRpmToRadAngle(float rotSpeed, float delta)
    {
        float partOfMinute = delta / 60;

        return 2*Mathf.PI*partOfMinute*rotSpeed;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rotSpeed"></param>
    /// <param name="delta"></param>
    /// <returns>Angle in rad</returns>
    private float convertRpmToDegAngle(float rotSpeed, float delta)
    {
        float partOfMinute = delta / 60;

        return 360*partOfMinute*rotSpeed;
    }
}
