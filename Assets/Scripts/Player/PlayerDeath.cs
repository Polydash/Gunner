using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour {

    public int yDeath = -20;

    private bool m_isDead = false;

    private GameObject m_SpawnManager;

	// Use this for initialization
	void Start () 
    {
        m_SpawnManager = GameObject.Find("SpawnerManager");
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!m_isDead && transform.position.y < yDeath)
        {
            m_isDead = true;
            m_SpawnManager.GetComponent<SpawnerManager>().NotifyDeath();
        }
	}

    public bool IsDead()
    {
        return m_isDead;
    }

    public void Reset()
    {
        m_isDead = false;
    }
}
