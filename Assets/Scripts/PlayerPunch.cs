using UnityEngine;
using System.Collections;

public class PlayerPunch : MonoBehaviour
{
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
			script.m_punchLaunched = false;
			script.m_punchReturning = true;
			rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
			collider2D.enabled = false;
		}
	}
}
