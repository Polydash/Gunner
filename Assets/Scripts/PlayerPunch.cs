using UnityEngine;
using System.Collections;

public class PlayerPunch : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		PlayerControl script = GetComponentInParent<PlayerControl>();

		if(script.m_punchLaunched && collision.collider.tag == "Tile")
		{
			script.m_punchLaunched = false;
			script.m_punchReturning = true;
			rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
			collider2D.enabled = false;
		}
	}
}
