using UnityEngine;
using System.Collections;

public class PlayerPunch : MonoBehaviour
{
	private float m_punchForce;

	private void Start()
	{
		//Get PlayerControl script and get parameter
		PlayerControl script = GetComponentInParent<PlayerControl>();
		m_punchForce = script.m_punchForce;
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
			}
			else
			{
				//Hit him with full force
				collision.collider.rigidbody2D.AddForce(direction * m_punchForce);
			}

			script.m_punchLaunched = false;
			script.m_punchReturning = true;
			rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
			collider2D.enabled = false;
		}
	}
}
