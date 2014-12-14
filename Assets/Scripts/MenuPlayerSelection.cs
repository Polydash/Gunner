using UnityEngine;
using System.Collections;

public class MenuPlayerSelection : MonoBehaviour {


    private PlayerManager m_playerManager;
    private int m_maxPlayers = -1;

    private int pointCount = 10;

    public int pointCountMin = 10;
    public int pointCountMax = 100;

    private float ratioX = 1366.0f / Screen.width ;
    private float ratioY =  768.0f / Screen.height;

    float baseGuiFontSize = 0.0f;

    public float fontRatio = 5.0f;

    public GUISkin mySkin;

	// Use this for initialization
	void Start ()
    {
       // Random.seed = (int)Time.time;

        m_playerManager = GameObject.Find("PlayersManager").GetComponent<PlayerManager>();
        if (!m_playerManager)
        {
            Debug.Log("Can't get PlayerManager in MenuPlayerSelection");
        }

        m_maxPlayers = m_playerManager.GetMaxPlayer();

	}
	
	// Update is called once per frame
	void Update ()
    {
        //Update ratio
        //ratioX = 1366.0f / Screen.width ;
        //ratioY =  768.0f / Screen.height;

        ratioX = Screen.width/  1366.0f;
        ratioY = Screen.width / 768.0f ;


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

                int levelNumber = 0;

                for (int j = 0; j < 100; ++j)
                {
                    levelNumber = Random.Range(1, 3);
                    print(levelNumber);
                }
                
                m_playerManager.currentLevel = levelNumber;
                string level = "Level " + levelNumber.ToString();
                
                Application.LoadLevel("Level 1");
            }
        }

        //Change the point count
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
        GUI.skin = mySkin;
        //myStyle.fontSize = (int)(baseGuiFontSize * fontRatio);
        Color oldColor = GUI.color;
        for (int i = 0; i < m_maxPlayers; ++i)
        {
            if (i == 0)
            {
                GUI.color = Color.cyan;
   
            }
            if (i == 1)
            {
                GUI.color = Color.red;
            }
            if (i == 2)
            {
                GUI.color = Color.yellow;
            }
            if (i == 3)
            {
                GUI.color = Color.green;
            }

            GUI.Box(new Rect(10.0f * ratioX, 50.0f + (i * 30.0f * ratioY), 200.0f * ratioX, 20.0f * ratioY), "Player " + (i + 1).ToString() + " " + (m_playerManager.GetPlayerTab()[i] ? "Ready !" : " not Ready."));
        }
        GUI.color = oldColor;


        GUI.Box(new Rect(Screen.width - 200.0f * ratioX, 50.0f * ratioY, 200.0f * ratioX, 30.0f * ratioY), "Points to win : " + pointCount.ToString());

        GUI.Box(new Rect(10.0f * ratioX, Screen.height - 50.0f * ratioY, 200.0f * ratioX, 30.0f * ratioY), "Press start to Begin");
    
    
    }




}
