using UnityEngine;
using System.Collections;

public class InGameInterface : MonoBehaviour {

    private GameObject[] m_players;

    private int rectWidth = 150;
    private int rectHeight = 30;

    PlayerManager PM;

    public bool _debug = false;

    public GUISkin mySkin;

    private float ratioX = 1366.0f / Screen.width;
    private float ratioY = 768.0f / Screen.height;

	// Use this for initialization
	void OnEnable ()
    {
        if (!_debug)
        {
            PM = GameObject.Find("PlayersManager").GetComponent<PlayerManager>();
            m_players = GameObject.Find("SpawnerManager").GetComponent<SpawnerManager>().m_players;
        }
       
	}

    void Start()
    {
        if (_debug)
        {
            PM = GameObject.Find("DEBUGPlayerManager").GetComponent<PlayerManager>();
            m_players = GameObject.Find("SpawnerManager").GetComponent<SpawnerManager>().m_players;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {

        ratioX = Screen.width / 1366.0f;
        ratioY = Screen.width / 768.0f;

        if (PM.m_playerVictory)
        {
            for (int i = 0; i < PM.GetMaxPlayer() ; ++i)
            {
                if (Input.GetButtonDown("P" + (i + 1).ToString() + " Start") && PM.GetPlayerTab()[i])
                {
                    PM.Reset();
                }

                if (Input.GetButtonDown("P" + (i + 1).ToString() + " B") && PM.GetPlayerTab()[i])
                {
                    PM.ResetToMenu();
                    GameObject.Destroy(GameObject.Find("PlayersManager"));
                    Application.LoadLevel("Menu");
                }
            }
        }
	}

    void OnGUI()
    {
        GUI.skin = mySkin;
        Color oldColor = GUI.color;
        if (!PM.m_playerVictory)
        {
            if (m_players.Length >= 1)
            {
                GUI.color = Color.cyan;
                GUI.Box(new Rect(0, 0, rectWidth * ratioX, rectHeight * ratioY), "Player 1 \n " + m_players[0].GetComponent<PlayerScore>().m_playerScore.ToString());
            }

            if (m_players.Length >= 2)
            {
                GUI.color = Color.red;
                GUI.Box(new Rect(Screen.width - rectWidth * ratioX, 0, rectWidth * ratioX, rectHeight * ratioY), "Player 2 \n " + m_players[1].GetComponent<PlayerScore>().m_playerScore.ToString());
            }

            if (m_players.Length >= 3)
            {
                GUI.color = Color.yellow;
                GUI.Box(new Rect(0, Screen.height - rectHeight * ratioY, rectWidth * ratioX, rectHeight * ratioY), "Player 3 \n " + m_players[2].GetComponent<PlayerScore>().m_playerScore.ToString());
            }

            if (m_players.Length >= 4)
            {
                GUI.color = Color.green;
                GUI.Box(new Rect(Screen.width - rectWidth * ratioX, Screen.height - rectHeight * ratioY, rectWidth * ratioX, rectHeight * ratioY), "Player 4 \n " + m_players[3].GetComponent<PlayerScore>().m_playerScore.ToString());
            }
        }
        else
        {
            GUI.Box(new Rect(Screen.width/2 - rectWidth/2, Screen.height/2 - rectHeight/2, rectWidth, rectHeight), "Player " + GameObject.Find("PlayersManager").GetComponent<PlayerManager>().m_playerWinner.GetComponent<PlayerID>().GetPlayerID().ToString() + " Win !!");
            GUI.Box(new Rect(Screen.width / 2 - rectWidth / 2, Screen.height / 2 - rectHeight / 2 + 50, rectWidth, rectHeight+20), "Start to Restart\nB to Menu");
        
        }
        
    }
}
