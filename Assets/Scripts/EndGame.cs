using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour
{

    private PlayerManager PM;

    float timeSlow = 3.0f;
    float m_Time = 0.0f;

    // Use this for initialization
    void Start()
    {
        PM = GameObject.Find("PlayersManager").GetComponent<PlayerManager>();
        m_Time = 0.0f;
    }

    // Update is called once per frame
    void Update() 
    {

        if (PM.m_playerVictory)
        {
            //Time.timeScale = 0.2f;
            ////Time.timeScale = 1.0f - Mathf.Lerp(0.0f, 1.0f, m_Time / timeSlow);
            //print(Time.timeScale);
            ////m_Time += Time.deltaTime;
        }
	}
}
