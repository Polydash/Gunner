using UnityEngine;
using System.Collections;

public class PlayRandomAmbiance : MonoBehaviour {


    public AudioClip[] Sounds;

    public int min = 5;
    public int max = 20;


    private bool m_soundPlayed = false;
    private int m_nextSoundTime = -1;

    float m_time = 0.0f;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_nextSoundTime == -1)
        {
            m_nextSoundTime = Random.Range(min, max);
        }

        if (m_nextSoundTime != -1 && m_time >= m_nextSoundTime)
        {
            audio.PlayOneShot(Sounds[Random.Range(0, Sounds.Length - 1)]);
            m_nextSoundTime = -1;
            m_time = 0.0f;
            print("Play Sound motherfucker");
        }

        if (m_time < m_nextSoundTime && m_nextSoundTime != -1)
        {
            m_time += Time.deltaTime;
        }

       // print(m_nextSoundTime + " - " + m_time + " ");
	
	}
}
