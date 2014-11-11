using UnityEngine;
using System.Collections;

public class PlayerPunch : MonoBehaviour
{
	private float m_punchForce = 0.0f;

	private void Start()
	{
		//Get PlayerControl script
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
			//Hit him
			Vector2 velocity = rigidbody2D.velocity;
			velocity.Normalize();
			collision.collider.rigidbody2D.AddForce(velocity * 2000.0f);
			
			script.m_punchLaunched = false;
			script.m_punchReturning = true;
			rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
			collider2D.enabled = false;
		}
	}
}
