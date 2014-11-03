using UnityEngine;
using System.Collections;

public class GameMgr : MonoBehaviour
{
    //Level size (must be a multiple of 3)
    public int m_levelSize = 9;

    //Singleton variable
    private static GameMgr s_instance = null;

    //GameManager singleton declaration
    public static GameMgr instance
    {
        get
        {
            //Get instance in current scene
            if (s_instance == null)
            {
                s_instance = FindObjectOfType(typeof(GameMgr)) as GameMgr;
            }

            return s_instance;
        }
    }

    //Make sure the instance is set to null
    private void OnApplicationQuit()
    {
        s_instance = null;
    }
}
