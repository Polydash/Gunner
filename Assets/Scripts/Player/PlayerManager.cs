using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour 
{
    public bool _debug = false;

    public GameObject playerPrefab;

    //Number of player Max
    private const int m_maxPlayers = 4;

    private bool[] m_playersIDs = new bool[m_maxPlayers];

    private bool m_inGame = false;

    //Put to false when entering ingame, put to true when exiting in game
    private bool m_needIDGeneration = true;

    //Spawner Tab
    private GameObject[] m_SpawnerTab;

    public int m_PointCount { get; set; }

    public bool m_playerVictory = false;

    SpawnerManager SpawnManager;

    void Awake()
    {
        //Keep the manager everywhere
        DontDestroyOnLoad(transform.gameObject);

       for(int i = 0; i < m_maxPlayers; ++i)
       {
           m_playersIDs[i] = false;//Put everything to false
       }

        if(_debug)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; ++i)
             {
                 players[i].GetComponent<PlayerID>().SetID(i + 1);
             }

        }

       // SpawnManager = GameObject.Find("SpawnerManager").GetComponent<SpawnerManager>();

    }

    void Update()
    {
        if(!_debug && m_needIDGeneration && m_inGame)
        {
            m_needIDGeneration = false;
            
            //Get the spawners
            m_SpawnerTab = GameObject.FindGameObjectsWithTag("Spawner");

            for (int i = 0; i < m_maxPlayers; ++i)
            {
                if (m_playersIDs[i])//If the player is here
                {
                    GameObject player = Object.Instantiate(playerPrefab, m_SpawnerTab[i].transform.position, Quaternion.identity) as GameObject;//Instanciate the player at a spawner
                    player.GetComponent<PlayerID>().SetID(i + 1);//Set his ID
                    player.GetComponent<Twinkle>().enabled = true;

                   
                    //return i + 1;//Return the id of the player
                }
            }
            Debug.Log("Instanciation");
            GameObject.Find("SpawnerManager").GetComponent<SpawnerManager>().enabled = true;
        }


        //Check every frame if there is a winner
        //if (m_inGame)
        //{

        //}

    }

    public void ResetToMenu()
    {
        m_inGame = false;
        m_needIDGeneration = true;
        m_playerVictory = false;

    }

    public bool[] GetPlayerTab()
    {
        return m_playersIDs;
    }

    public bool IsInGame()
    {
        return m_inGame;
    }

    public void SetInGame(bool inGame)
    {
        m_inGame = inGame;
    }

    public int GetCurrentPlayerNumber()
    {
        int playerNumber = 0;
        for (int i = 0; i < m_maxPlayers; ++i)
        {
            if (m_playersIDs[i])
                ++playerNumber;
        }
        return playerNumber;
    }


    /*
     * This function returns an ID if there is one disponible, otherwise, it returns 0
     * Called in the script PlayerID in the player prefab
     */
    public int GetID()
    {
        for (int i = 0; i < m_maxPlayers; ++i)
        {
           if(!m_playersIDs[i])//If there is some place
           {
               m_playersIDs[i] = true;

               return i + 1;//Return the id of the player
           }
        }

        return 0;//if there is no more place
    }


    /*
    * This function retrieves an ID put in the parameter
    * Called in the script PlayerID in the function OnDestroy()
    */
    public void GetIDBack(int ID)
    {
        if(ID > 0 && ID <= m_maxPlayers)
        {
            m_playersIDs[ID - 1] = false;
        }
    }

    public int GetMaxPlayer()
    {
        return m_maxPlayers;
    }
}
