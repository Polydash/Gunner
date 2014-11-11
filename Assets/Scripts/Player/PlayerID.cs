using UnityEngine;
using System.Collections;

public class PlayerID : MonoBehaviour {


    private PlayerManager m_scriptPM;
    private int m_playerID = 0;


	// Use this for initialization
	void Awake () 
    {
        m_scriptPM = GameObject.Find("PlayersManager").GetComponent<PlayerManager>();
        m_playerID = m_scriptPM.GetID();//Get the id

        if(m_playerID == 0)//If the ID is wrong, delete the gameobject
        {
            Object.Destroy(gameObject);
        }
	}

    /*
     * This function return the ID of the player. Useful for the inputs.
     */
    public int GetPlayerID()
    {
        return m_playerID;
    }


    //When the player is destroy, it return its ID to the player manager to notify there is a new slot disponible
    void OnDestroy()
    {
        m_scriptPM.GetIDBack(m_playerID);
    }
}
