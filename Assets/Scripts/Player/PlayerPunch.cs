using UnityEngine;
using System.Collections;

public class PlayerPunch : MonoBehaviour
{
	private float m_punchForce;

    private PlayerScore m_PlayerScore;

	private IEnumerator WaitForSecondsUnscaled(float time)
	{
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + time)
		{
			yield return null;
		}
	}

	private IEnumerator Freeze(float waitTime)
	{
		float scale = Time.timeScale;
		Time.timeScale = 0.0f;
		yield return StartCoroutine(WaitForSecondsUnscaled(waitTime * (1.0f / scale)));
		Time.timeScale = scale;
	}

	private void Start()
	{
		//Get PlayerControl script and get parameter
		PlayerControl script = GetComponentInParent<PlayerControl>();
		m_punchForce = script.m_punchForce;

        m_PlayerScore = transform.parent.GetComponent<PlayerScore>();
        if (!m_PlayerScore)
        {
            Debug.Log("Error getting player score in playerPunch");
        }
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		//Get PlayerControl script
		PlayerControl script = GetComponentInParent<PlayerControl>();

		//If glove punches a tile
		if(script.m_punchLaunched && collision.collider.tag == "Tile")
		{
			script.m_punchLaunched = false;
			script.m_punchReturning = true;
			rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
			collider2D.enabled = false;
		}

		//If glove punches a player
		if(script.m_punchLaunched && collision.collider.tag == "Player")
		{
            //Added by Guillaume. Reset the score Timer and set the last player punched
            m_PlayerScore.m_Time = 0.0f;
            m_PlayerScore.LastPlayerTouched = collision.transform.gameObject;
           

			//Get opponent PlayerControl script
			PlayerControl opponentScript = collision.transform.GetComponent<PlayerControl>();

			//Get punch direction
			Vector2 direction = script.m_punchDirection;
			direction.Normalize();

			//If player is guarding
			if(opponentScript.m_isGuarding && ((direction.x > 0.5f && !opponentScript.m_facingRight) ||
			                                   (direction.x < -0.5f && opponentScript.m_facingRight)))
			{
				//Blocked
				collision.collider.rigidbody2D.AddForce(direction * m_punchForce / 4.0f, ForceMode2D.Impulse);
                GameObject.Find("Camera").GetComponent<SoundManager>().m_playSoundGuard = true;
			}
			else
			{
				//Break guard
				if(opponentScript.m_isGuarding)
				{
					opponentScript.BreakGuard();
				}

				//Hit him with full force
				collision.collider.rigidbody2D.AddForce(direction * m_punchForce, ForceMode2D.Impulse);
				Camera.main.GetComponent<CameraMgr>().Translate(direction);
				StartCoroutine(Freeze(0.075f));

                //Play SOund
                //play sounds
                GameObject.Find("Camera").GetComponent<SoundManager>().m_playSoundHitSuccess = true;
			}

			script.m_punchLaunched = false;
			script.m_punchReturning = true;
			rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
			collider2D.enabled = false;
		}
	}
}
