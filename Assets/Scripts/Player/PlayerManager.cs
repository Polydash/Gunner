using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    //Number of player Max
    private const int m_maxPlayers = 4;

    private bool[] m_playersIDs = new bool[m_maxPlayers];

    void Awake()
    {
       for(int i = 0; i < m_maxPlayers; ++i)
       {
           m_playersIDs[i] = false;//Put everything to false
       }
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
