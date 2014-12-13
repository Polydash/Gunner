using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    //SOUNDS
    //Ambiance Sound
    public AudioClip[] AmbianceSounds;

    public int min = 5;
    public int max = 20;

    //Game Sounds
    public AudioClip SoundStartGame;
    public AudioClip SoundStartRound;

    //FeedbackSounds

   


    private bool m_soundPlayed = false;
    private int m_nextSoundTime = -1;

    float m_time = 0.0f;

    public bool m_playSoundStartGame {get; set;}
    public bool m_playSoundStartRound { get; set; }

	// Use this for initialization
	void Start () 
    {
        m_playSoundStartGame = false;
        m_playSoundStartRound = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Play Game Sounds
        if (m_playSoundStartGame)
        {
            audio.PlayOneShot(SoundStartGame);
            m_playSoundStartGame = false;
        }

        if (m_playSoundStartRound)
        {
            audio.PlayOneShot(SoundStartRound);
            m_playSoundStartRound = false;
        }






        //Play Ambiance Sounds
        if (m_nextSoundTime == -1)
        {
            m_nextSoundTime = Random.Range(min, max);
        }

        if (m_nextSoundTime != -1 && m_time >= m_nextSoundTime)
        {
            audio.PlayOneShot(AmbianceSounds[Random.Range(0, AmbianceSounds.Length - 1)]);
            m_nextSoundTime = -1;
            m_time = 0.0f;
        }
        if (m_time < m_nextSoundTime && m_nextSoundTime != -1)
        {
            m_time += Time.deltaTime;
        }

	}
}
