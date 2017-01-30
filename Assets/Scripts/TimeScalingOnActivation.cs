using UnityEngine;

public class TimeScalingOnActivation : MonoBehaviour
{

    public float Timescale;

    private float _previousTimescale;

	// Use this for initialization
	void Start () {
		
	}

    void OnEnable()
    {
        _previousTimescale = Time.timeScale;
        Time.timeScale = Timescale;
        Cursor.visible = true;
    }

    void OnDisable()
    {
        Time.timeScale = _previousTimescale;
        Cursor.visible = false;
    }



    // Update is called once per frame
    void Update () {
    }
}
