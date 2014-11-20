using UnityEngine;
using System.Collections;

public class Twinkle : MonoBehaviour {

    public float twinkleDuration = 3.0f;
    public float twinkleSpeed = 30.0f;
    float twinkleTime = 0.0f;

    float time = 0.0f;


	// Use this for initialization
	void Start () 
    {
        twinkleTime = 0.0f;
        time = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
    {

        if (Mathf.Sin(twinkleTime) > 0)
        {
            renderer.enabled = true;
        }
        else
        {
            renderer.enabled = false;
        }

        twinkleTime += Time.deltaTime * twinkleSpeed;

        time += Time.deltaTime;

        if (time >= twinkleDuration)
        {
            renderer.enabled = true;
            enabled = false;
        }
	
	}
}
