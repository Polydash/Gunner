using UnityEngine;
using System.Collections;

public class PlayerPunch : MonoBehaviour
{
	private float m_punchForce;

    private PlayerScore m_PlayerScore;

	private void Start()
	{
		//Get PlayerControl script and get parameter
		PlayerControl script = GetComponentInParent<PlayerControl>();
		m_punchForce = script.m_punchForce;

        m_PlayerScore = transform.parent.GetComponent<PlayerScore>();
        if (!m_PlayerScore)
            print("PAS OK");
        else
            print("OK");

        print("HEERERERERER");
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
                 m_PlayerScore.m_AddTouchGuardScore = true;
			}
			else
			{
				//Hit him with full force
				collision.collider.rigidbody2D.AddForce(direction * m_punchForce);
                m_PlayerScore.m_AddTouchScore = true;
			}

			script.m_punchLaunched = false;
			script.m_punchReturning = true;
			rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
			collider2D.enabled = false;
		}
	}
}
