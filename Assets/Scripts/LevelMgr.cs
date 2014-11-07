using UnityEngine;
using System.Collections;

public class LevelMgr : MonoBehaviour
{
    //Level size (must be a multiple of 9)
    public int m_levelSize = 9;

	//Level structure
	public bool[] m_level;

    //Singleton variable
    private static LevelMgr s_instance = null;

    //GameManager singleton declaration
    public static LevelMgr instance
    {
        get
        {
            //Get instance in current scene
            if (s_instance == null)
            {
                s_instance = FindObjectOfType(typeof(LevelMgr)) as LevelMgr;
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
