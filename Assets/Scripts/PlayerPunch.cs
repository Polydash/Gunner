using UnityEngine;
using System.Collections;

public class PlayerPunch : MonoBehaviour
{
	public float m_time     = 3.0f;
	public float m_speedMin = 0.0f;
	public float m_speedMax	= 1.0f;
	public float m_returnRatio = 0.5f;

	//State
	private bool m_isLaunched   = false;
	private bool m_isReturning  = false;
	private float m_elapsedTime = 0.0f;

	//Input helper variables
	private bool m_jumpPressed 		 = false;
	private bool m_leftPunchPressed  = false;
	private bool m_rightPunchPressed = false;
	private bool m_upPunchPressed	 = false;

	void Update()
	{
		//Check inputs (for some reason, GetButtonDown does not
		//respond properly in FixedUpdate())
		
		//Check jump button
		if(Input.GetButtonDown("Fire1"))
		{
			m_jumpPressed = true;
		}
		
		//Check left button
		if(Input.GetButtonDown("Fire3"))
		{
			m_leftPunchPressed = true;
		}
		
		//Check right button
		if(Input.GetButtonDown("Fire2") && !m_isReturning && !m_isLaunched)
		{
			m_elapsedTime = 0.0f;
			m_isLaunched = true;
			m_rightPunchPressed = true;
			transform.localPosition = new Vector2(1.0f, 0.0f);
			renderer.enabled = true;
			collider2D.enabled = true;
		}
		
		//Check up button
		if(Input.GetButtonDown("Jump"))
		{
			m_upPunchPressed = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(m_isLaunched && collision.collider.tag == "Tile")
		{
			m_isLaunched = false;
			m_isReturning = true;
			rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
			renderer.enabled = false;
			collider2D.enabled = false;
		}
	}

	private void FixedUpdate()
	{
		if(m_isLaunched && m_elapsedTime > m_time)
		{
			m_isLaunched = false;
			m_isReturning = true;
			rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
			collider2D.enabled = false;
		}

		if(m_isLaunched)
		{
			m_elapsedTime += Time.deltaTime;
			float speed = Mathf.Lerp(m_speedMin, m_speedMax, 1.0f - m_elapsedTime / m_time);
			rigidbody2D.velocity = new Vector2(speed, 0.0f);
		}

		if(m_isReturning && transform.localPosition.sqrMagnitude < 0.1f)
		{
			m_isReturning = false;
			renderer.enabled = false;
		}

		if(m_isReturning)
		{
			transform.localPosition -= m_returnRatio * transform.localPosition;
		}
	}
}
