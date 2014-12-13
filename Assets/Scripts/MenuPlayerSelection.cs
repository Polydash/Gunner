using UnityEngine;
using System.Collections;

public class MenuPlayerSelection : MonoBehaviour {


    private PlayerManager m_playerManager;
    private int m_maxPlayers = -1;

    private int pointCount = 11;

    public int pointCountMin = 10;
    public int pointCountMax = 1000;

	// Use this for initialization
	void Start ()
    {
        m_playerManager = GameObject.Find("PlayersManager").GetComponent<PlayerManager>();
        if (!m_playerManager)
        {
            Debug.Log("Can't get PlayerManager in MenuPlayerSelection");
        }

        m_maxPlayers = m_playerManager.GetMaxPlayer();// = 2 for the moment

	}
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i < m_maxPlayers; ++i)
        {
            if (Input.GetButtonUp("P" + (i + 1).ToString() + " A"))
            {
                m_playerManager.GetPlayerTab()[i] = !m_playerManager.GetPlayerTab()[i];
            }

            if (Input.GetButton("P" + (i + 1).ToString() + " Start") && m_playerManager.GetPlayerTab()[i])
            {
                m_playerManager.m_PointCount = pointCount;
                m_playerManager.SetInGame(true);
                Application.LoadLevel("Game");
            }
        }

        //Change the round count
        for (int i = 0; i < m_maxPlayers; ++i)
        {
            if (Input.GetAxis("P" + (i + 1).ToString() + " LHorizontal") > 0 && m_playerManager.GetPlayerTab()[i])
            {
                ++pointCount;
                if (pointCount > pointCountMax)
                {
                    pointCount = pointCountMin;
                }
                break;
            }

            if (Input.GetAxis("P" + (i + 1).ToString() + " LHorizontal") < 0 && m_playerManager.GetPlayerTab()[i])
            {
                --pointCount;
                if (pointCount < pointCountMin)
                {
                    pointCount = pointCountMax;
                }
                break;
            }
        }
       
	}


    void OnGUI()
    {
        for (int i = 0; i < m_maxPlayers; ++i)
        {
            GUI.Box(new Rect(10, 10 + i * 30, 200, 30), "Player " + (i + 1).ToString() + " " + (m_playerManager.GetPlayerTab()[i]?"Ready !":" not Ready."));
        }

        GUI.Box(new Rect(Screen.width - 200, 50, 200, 30), "Points to win : " + pointCount.ToString());

        GUI.Box(new Rect(10, Screen.height - 50, 200, 30), "Press start to Begin");
    }




}
