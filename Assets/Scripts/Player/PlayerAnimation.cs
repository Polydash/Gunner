using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
	private Animator m_animator;
	private PlayerControl m_playerControl;

	private bool m_running = false;

	void Start()
	{
		m_animator = GetComponent<Animator>();
		if(m_animator == null)
		{
			Debug.LogError("Failed to get animator");
		}

		m_playerControl = GetComponent<PlayerControl>();
		if(m_playerControl == null)
		{
			Debug.LogError("Failed to get Player Control script");
		}
	}

	void Update()
	{
		m_animator.SetFloat("VelY", rigidbody2D.velocity.y);
		m_animator.SetBool("Grounded", m_playerControl.m_isGrounded);

		bool running = m_playerControl.m_isGrounded &&
					   ((m_playerControl.m_facingRight && rigidbody2D.velocity.x > 0.01f) ||
			 		   (!m_playerControl.m_facingRight && rigidbody2D.velocity.x < -0.01f));

		m_animator.SetBool("Running", running);
	}
}
