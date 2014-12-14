using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
	private Animator m_animator;
	private PlayerControl m_playerControl;

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

		if(running)
		{
			if(m_playerControl.m_facingRight)
			{
				GetComponent<PlayerFXData>().InstantiateBottom(PlayerFXData.eFXType.RUN, Quaternion.identity);
			}
			else
			{
				GetComponent<PlayerFXData>().InstantiateBottom(PlayerFXData.eFXType.RUN, Quaternion.Euler(new Vector3(0.0f, 180.0f)));
			}
		}

		bool punching = m_playerControl.m_punchRequested ||
						m_playerControl.m_punchLaunched  ||
						m_playerControl.m_punchReturning;

		m_animator.SetBool("PunchingSide", punching && 
		                   				   (m_playerControl.m_requestedDirection == PlayerControl.ePunchDirection.RIGHT ||
		 									m_playerControl.m_requestedDirection == PlayerControl.ePunchDirection.LEFT));

		m_animator.SetBool("PunchingUp", punching && m_playerControl.m_requestedDirection == PlayerControl.ePunchDirection.UP);
		m_animator.SetBool("PunchingDown", punching && m_playerControl.m_requestedDirection == PlayerControl.ePunchDirection.DOWN);

		m_animator.SetBool("Guarding", m_playerControl.m_isGuarding);
	}
}
