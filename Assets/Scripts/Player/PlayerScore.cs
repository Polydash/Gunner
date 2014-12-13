using UnityEngine;
using System.Collections;

public class PlayerScore : MonoBehaviour {


    public int m_playerScore { get; set; }

    public int TouchScore = 2;
    public int TouchGuardScore = 1;
    public int KillScore = 5;

    public bool m_AddTouchScore { get; set; }
    public bool m_AddTouchGuardScore { get; set; }

    public float DeathTimer = 5.0f;

    public float m_Time { get; set; }

    public GameObject LastPlayerTouched { get; set; }

	// Use this for initialization
	void Start ()
    {
        m_AddTouchScore = false;
        m_AddTouchGuardScore = false;
        m_playerScore = 0;

        m_Time = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (LastPlayerTouched && LastPlayerTouched.GetComponent<PlayerDeath>().IsDead() && m_Time < DeathTimer)
        {
            m_playerScore += KillScore;
        }

        if (m_AddTouchScore)
        {
            m_playerScore += TouchScore;
            m_AddTouchScore = false;
        }

        if (m_AddTouchGuardScore)
        {
            m_playerScore += TouchGuardScore;
            m_AddTouchGuardScore = false;
        }

        m_Time += Time.deltaTime;

        print("My Score = " + m_playerScore);
	}
}
