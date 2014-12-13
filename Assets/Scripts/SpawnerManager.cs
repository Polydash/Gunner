using UnityEngine;
using System.Collections;

public class SpawnerManager : MonoBehaviour {


    private PlayerManager m_playerManager;
    private int m_maxPlayers = -1;

    private int m_currentPlayerNumber;

    //Spawner Tab
    private GameObject[] m_SpawnerTab;

    //Players
    private GameObject[] m_players;

    int m_deathCounter = 0;

    public bool Debug = false;

    SoundManager SM;

   

	// Use this for initialization
	void Start () 
    {
        if (!Debug)
        {
            m_playerManager = GameObject.Find("PlayersManager").GetComponent<PlayerManager>();
            SM = GameObject.Find("Camera").GetComponent<SoundManager>();
        }
        else
        {
            m_playerManager = GameObject.Find("DEBUGPlayerManager").GetComponent<PlayerManager>();
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              

        if (!m_playerManager)
        {
            //Debug.Log("Can't get PlayerManager in SpawnerManager");
        }

        
        m_players = GameObject.FindGameObjectsWithTag("Player");
        m_SpawnerTab = GameObject.FindGameObjectsWithTag("Spawner");
        m_currentPlayerNumber = m_players.Length;

        SM.m_playSoundStartGame = true;

       // print(m_SpawnerTab.Length);
	}

    public void NotifyDeath()
    {
        ++m_deathCounter;
    }
	
	// Update is called once per frame
	void Update () 
    {
        
        //for (int i = 0; i < m_players.Length; ++i)
        //{
        //    if(m_players[i].GetComponent<PlayerDeath>().IsDead())
        //    {
        //        ++m_deathCounter;
        //    }
        //}

       // print("DC : " + m_deathCounter + ", (m_currentPlayerNumber - 1) : " + (m_currentPlayerNumber - 1));

        if (m_currentPlayerNumber == 1 && m_deathCounter == 1)//Case where there is only one player
        {
            m_deathCounter = 0;
            //Respawn everybody
            for (int i = 0; i < m_players.Length; ++i)
            {
                m_players[i].transform.position = m_SpawnerTab[i].transform.position;
                m_players[i].GetComponent<Twinkle>().enabled = true;
                m_players[i].GetComponent<PlayerDeath>().Reset();
                m_players[i].rigidbody2D.velocity = new Vector2(0, 0);
            }
        }
        else if(m_currentPlayerNumber != 1 && m_deathCounter >= (m_currentPlayerNumber - 1))
        {
            m_deathCounter = 0;
            //Respawn everybody
            for (int i = 0; i < m_players.Length; ++i)
            {
                m_players[i].transform.position = m_SpawnerTab[i].transform.position;
                m_players[i].GetComponent<Twinkle>().enabled = true;
                m_players[i].GetComponent<PlayerDeath>().Reset();
                m_players[i].rigidbody2D.velocity = new Vector2(0, 0);
            }
            SM.m_playSoundStartRound = true;
            
        }

        
       
	}
}
