using UnityEngine;
using System.Collections;

public class InGameInterface : MonoBehaviour {

    private GameObject[] m_players;

    private int rectWidth = 100;
    private int rectHeight = 20;

    PlayerManager PM;

    public bool _debug = false;

	// Use this for initialization
	void OnEnable ()
    {
        if (!_debug)
        {
            PM = GameObject.Find("PlayersManager").GetComponent<PlayerManager>();
            m_players = GameObject.Find("SpawnerManager").GetComponent<SpawnerManager>().m_players;
            print(m_players.Length);
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

        if (!PM.m_playerVictory)
        {
            if (m_players.Length >= 1)
            {
                GUI.Box(new Rect(0, 0, rectWidth, rectHeight), "Player 1 : " + m_players[0].GetComponent<PlayerScore>().m_playerScore.ToString());
            }

            if (m_players.Length >= 2)
            {
                GUI.Box(new Rect(Screen.width - rectWidth, 0, rectWidth, rectHeight), "Player 2 : " + m_players[1].GetComponent<PlayerScore>().m_playerScore.ToString());
            }

            if (m_players.Length >= 3)
            {
                GUI.Box(new Rect(0, Screen.height - rectHeight, rectWidth, rectHeight), "Player 3 : " + m_players[2].GetComponent<PlayerScore>().m_playerScore.ToString());
            }

            if (m_players.Length >= 4)
            {
                GUI.Box(new Rect(Screen.width - rectWidth, Screen.height - rectHeight, rectWidth, rectHeight), "Player 4 : " + m_players[3].GetComponent<PlayerScore>().m_playerScore.ToString());
            }
        }
        else
        {
            GUI.Box(new Rect(Screen.width/2 - rectWidth/2, Screen.height/2 - rectHeight/2, rectWidth, rectHeight), "Player " + GameObject.Find("PlayersManager").GetComponent<PlayerManager>().m_playerWinner.GetComponent<PlayerID>().GetPlayerID().ToString() + " Win !!");
            GUI.Box(new Rect(Screen.width / 2 - rectWidth / 2, Screen.height / 2 - rectHeight / 2 + 50, rectWidth, rectHeight+20), "Start to Restart\nB to Menu");
        
        }
        
    }
}
