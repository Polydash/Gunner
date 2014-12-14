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
    public AudioClip[] SoundsStartRoundArbitre;

    //FeedbackSounds
    public AudioClip[] SuccessPunchSounds;
    public bool m_playSoundHitSuccess { get; set; }

    public AudioClip[] WallPunchSounds;
    public bool m_playSoundHitWall { get; set; }

    public AudioClip[] SoundGuard;
    public bool m_playSoundGuard { get; set; }

    public AudioClip[] SoundWoosh;
    public bool m_playSoundWoosh { get; set; }


    private bool m_soundPlayed = false;
    private int m_nextSoundTime = -1;

    float m_time = 0.0f;

    public bool m_playSoundStartGame {get; set;}
    public bool m_playSoundStartRound { get; set; }

    //

	// Use this for initialization
	void Start () 
    {
        m_playSoundStartGame = false;
        m_playSoundStartRound = false;

        m_playSoundHitSuccess = false;
        m_playSoundHitWall = false;

        m_playSoundWoosh = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Play Game Sounds
        if (m_playSoundStartGame)
        {
            audio.PlayOneShot(SoundStartGame);
            audio.PlayOneShot(SoundsStartRoundArbitre[Random.Range(0, SoundsStartRoundArbitre.Length)]); 
            m_playSoundStartGame = false;
        }

        if (m_playSoundStartRound)
        {
            audio.PlayOneShot(SoundStartRound);
            audio.PlayOneShot(SoundsStartRoundArbitre[Random.Range(0, SoundsStartRoundArbitre.Length)]); 
            m_playSoundStartRound = false;
        }

        if (m_playSoundHitSuccess)
        {
            audio.PlayOneShot(SuccessPunchSounds[Random.Range(0, SuccessPunchSounds.Length)]);
            m_playSoundHitSuccess = false;
        }

        if (m_playSoundHitWall)
        {
            audio.PlayOneShot(WallPunchSounds[Random.Range(0, WallPunchSounds.Length)]);
            m_playSoundHitWall = false;
        }

        if (m_playSoundGuard)
        {
            audio.PlayOneShot(SoundGuard[Random.Range(0, SoundGuard.Length)]);
            m_playSoundGuard = false;
        }

        if (m_playSoundWoosh)
        {
            audio.PlayOneShot(SoundWoosh[Random.Range(0, SoundWoosh.Length)]);
            m_playSoundWoosh = false;
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
