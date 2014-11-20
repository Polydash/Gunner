using UnityEngine;
using System.Collections;

public class PlayerID : MonoBehaviour {


    private PlayerManager m_scriptPM;
    private int m_playerID = 0;


	// Use this for initialization
	void Awake () 
    {
       // m_scriptPM = GameObject.Find("PlayersManager").GetComponent<PlayerManager>();
        //m_playerID = m_scriptPM.GetID();//Get the id

        //if(m_playerID == 0)//If the ID is wrong, delete the gameobject
        //{
        //    Object.Destroy(gameObject);
        //}
	}

    /*
     * This function return the ID of the player. Useful for the inputs.
     */

    public void SetID(int ID)
    {
        m_playerID = ID;
    }

    public int GetPlayerID()
    {
        return m_playerID;
    }

}
