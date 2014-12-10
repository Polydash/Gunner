using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
	private Animator m_animator;

	void Start()
	{
		m_animator = GetComponent<Animator>();
		if(m_animator == null)
		{
			Debug.LogError("Failed to get animator");
		}
	}

	void Update()
	{
		m_animator.SetFloat("VelY", rigidbody2D.velocity.y);
	}
}
