using UnityEngine;
using System.Collections;

public class Twinkle : MonoBehaviour {

    public float twinkleDuration = 3.0f;
    public float twinkleSpeed = 30.0f;
    float twinkleTime = 0.0f;

    float time = 0.0f;

    public bool _debug = false;

    GameObject PlayerName;

	// Use this for initialization
    void OnEnable() 
    {
        twinkleTime = 0.0f;
        time = 0.0f;
        if(!_debug)
            this.GetComponent<PlayerControl>().m_hasControl = false;
        else
            this.GetComponent<PlayerControl>().m_hasControl = true;

        foreach (Transform t in transform)
         {
             if(t.name == "PlayerName")
             {
                 PlayerName = t.gameObject;
             }
         }

        PlayerName.renderer.enabled = true;
        PlayerName.GetComponent<TextMesh>().text = "P "+ GetComponent<PlayerID>().GetPlayerID().ToString();
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
            this.GetComponent<PlayerControl>().m_hasControl = true;
            PlayerName.renderer.enabled = false;
            renderer.enabled = true;
            enabled = false;
        }
	
	}

    
}
