﻿using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	private enum ePunchDirection
	{
		RIGHT,
		LEFT, 
		UP,
		DOWN
	};

	//Vertical velocity
	public float m_gravity	     = 100.0f;
	public float m_jump		     = 25.0f;
	public float m_analog		 = 20.0f;
	public float m_kickBackY	 = 17.0f;
	public float m_maxGravity    = 20.0f;

	//Horizontal velocity
	public float m_inAirAccelX   = 75.0f;
	public float m_inAirDeaccelX = 20.0f;
	public float m_accelX	     = 100.0f;
	public float m_deaccelX      = 75.0f;
	public float m_kickBackX	 = 12.0f;
	public float m_maxVelX       = 12.0f;

	//Punch velocity
	private float m_time          = 0.12f;
	private float m_speedMin      = 20.0f;
	private float m_speedMax	     = 100.0f;
	private float m_returnRatio   = 0.5f;

	//Reference to player glove
	private Transform m_glove = null;

	//Player State
	private bool m_isGrounded 	  = false;
	private bool m_analogJump 	  = false;

	//==== EVIL ====
	public bool m_punchLaunched   = false;
	public bool m_punchReturning  = false;

	private float m_punchElapsed  = 0.0f;
	private Vector2 m_punchDirection;

	//Input helper variables
	private bool m_jumpPressed 		 = false;
	private bool m_jumpReleased		 = false;
	private bool m_leftPunchPressed  = false;
	private bool m_rightPunchPressed = false;
	private bool m_upPunchPressed	 = false;

	private void Start()
	{
		//Keep the player from rotating with physics
		rigidbody2D.fixedAngle = true;

		//Init reference to player glove
		m_glove = transform.GetChild(0);
	}

	private void Update()
	{
		//Check inputs (for some reason, GetButtonDown does not
		//respond properly in FixedUpdate())

		//Check jump button
		if(Input.GetButtonDown("Fire1"))
		{
			m_jumpPressed = true;
		}
		else if(Input.GetButtonUp("Fire1"))
		{
			m_jumpReleased = true;
		}

		//Check left button
		if(Input.GetButtonDown("Fire3"))
		{
			m_leftPunchPressed = true;
		}

		//Check right button
		if(Input.GetButtonDown("Fire2"))
		{
			m_rightPunchPressed = true;
		}

		//Check up button
		if(Input.GetButtonDown("Jump"))
		{
			m_upPunchPressed = true;
		}
	}

	private void FixedUpdate()
	{
		//Check if player is grounded
		if(Mathf.Abs(rigidbody2D.velocity.y) > 0.001f)
		{
			m_isGrounded = false;
		}

		//Set horizontal deacceleration/acceleration values
		float deaccelX, accelX;
		if(m_isGrounded)
		{
			deaccelX = m_deaccelX;
			accelX	 = m_accelX;
		}
		else
		{
			deaccelX = m_inAirDeaccelX;
			accelX	 = m_inAirAccelX;
		}

		//Check horizontal input
		Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis ("Vertical"));
		moveInput.x *= accelX;

		//If player is not moving, or if current velocity is above max value
		if(Mathf.Abs(moveInput.x) < 0.01f || Mathf.Abs(rigidbody2D.velocity.x) > m_maxVelX)
		{
			//Apply horizontal deacceleration
			if(rigidbody2D.velocity.x > deaccelX * Time.deltaTime)
			{
				rigidbody2D.velocity -= new Vector2(deaccelX * Time.deltaTime, 0.0f);
			}
			else if(rigidbody2D.velocity.x < -deaccelX * Time.deltaTime)
			{
				rigidbody2D.velocity += new Vector2(deaccelX * Time.deltaTime, 0.0f);
			}
			else
			{
				rigidbody2D.velocity = new Vector2(0.0f, rigidbody2D.velocity.y);
			}
		}
		else if(moveInput.x * rigidbody2D.velocity.x > 0.0f)
		{
			//Apply horizontal input, considering max speed
			rigidbody2D.velocity += new Vector2(Mathf.Clamp(moveInput.x * Time.deltaTime,
			                                                Mathf.Min(0.0f, -m_maxVelX - rigidbody2D.velocity.x),
			                                                Mathf.Max(0.0f, m_maxVelX - rigidbody2D.velocity.x)), 0.0f);
		}
		else
		{
			//Apply horizontal input
			rigidbody2D.velocity += new Vector2(moveInput.x * Time.deltaTime, 0.0f);
		}

		//Check jump
		if(m_jumpPressed)
		{
			if(m_isGrounded)
			{
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, m_jump);
				m_isGrounded = false;
				m_analogJump = true;
			}
			else if(!m_punchLaunched && !m_punchReturning)
			{
				m_punchDirection = LaunchPunch(ePunchDirection.DOWN);
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, m_kickBackY);
			}

			m_jumpPressed = false;
		}
		else if(m_jumpReleased)
		{
			m_analogJump = false;
			m_jumpReleased = false;
		}

		//Check analogic jump
		if(m_analogJump)
		{
			if(rigidbody2D.velocity.y > 0.0f)
			{
				rigidbody2D.velocity += new Vector2(0.0f, m_analog * Time.deltaTime);
			}
			else
			{
				m_analogJump = false;
			}
		}

		//Check up punch
		if(m_upPunchPressed)
		{
			if(!m_punchLaunched && !m_punchReturning)
			{
				m_punchDirection = LaunchPunch(ePunchDirection.UP);

				if(!m_isGrounded)
				{
					rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -m_kickBackY);
				}
			}

			m_upPunchPressed = false;
		}

		//Check left punch
		if(m_leftPunchPressed && !m_punchLaunched && !m_punchReturning)
		{
			m_punchDirection = LaunchPunch(ePunchDirection.LEFT);
			rigidbody2D.velocity += new Vector2(m_kickBackX, 0.0f);
			m_leftPunchPressed = false;
		}

		//Check right punch
		if(m_rightPunchPressed && !m_punchLaunched && !m_punchReturning)
		{
			m_punchDirection = LaunchPunch(ePunchDirection.RIGHT);
			rigidbody2D.velocity -= new Vector2(m_kickBackX, 0.0f);
			m_rightPunchPressed = false;
		}

		//Update punch
		if(m_punchLaunched && m_punchElapsed > m_time)
		{
			m_punchLaunched = false;
			m_punchReturning = true;
			m_glove.rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
			m_glove.collider2D.enabled = false;
		}
		
		if(m_punchLaunched)
		{
			m_punchElapsed += Time.deltaTime;
			float speed = Mathf.Lerp(m_speedMin, m_speedMax, 1.0f - m_punchElapsed / m_time);
			Debug.Log(speed);
			m_glove.rigidbody2D.velocity = speed * m_punchDirection;
		}
		
		if(m_punchReturning && m_glove.transform.localPosition.sqrMagnitude < 0.1f)
		{
			m_punchReturning = false;
			m_glove.renderer.enabled = false;
		}
		
		if(m_punchReturning)
		{
			m_glove.transform.localPosition -= m_returnRatio * m_glove.transform.localPosition;
		}

		//Apply gravity
		rigidbody2D.velocity -= new Vector2(0.0f, m_gravity * Time.deltaTime);

		//Only clamp vertical velocity
		rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x,
		                                   Mathf.Max(rigidbody2D.velocity.y, -m_maxGravity));
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		for(int i=0; i<collision.contacts.Length; ++i)
		{
			//If player gets on the ground
			if(collision.contacts[i].normal == new Vector2(0.0f, 1.0f))
			{
				//If the player is not ascending
				if(rigidbody2D.velocity.y <= 0.0f)
				{
					//Player is on the ground
					m_isGrounded = true;
					rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0.0f);
				}
			}
		}
	}

	private Vector2 LaunchPunch(ePunchDirection direction)
	{
		Vector2 speedDirection = new Vector2();

		switch(direction)
		{
		case ePunchDirection.DOWN :
			m_glove.transform.localPosition = new Vector2(0.0f, -1.0f);
			speedDirection = new Vector2(0.0f, -1.0f);
			break;

		case ePunchDirection.UP :
			m_glove.transform.localPosition = new Vector2(0.0f, 1.0f);
			speedDirection = new Vector2(0.0f, 1.0f);
			break;

		case ePunchDirection.RIGHT :
			m_glove.transform.localPosition = new Vector2(1.0f, 0.0f);
			speedDirection = new Vector2(1.0f, 0.0f);
			break;

		case ePunchDirection.LEFT :
			m_glove.transform.localPosition = new Vector2(-1.0f, 0.0f);
			speedDirection = new Vector2(-1.0f, 0.0f);
			break;

		default :
			break;
		}

		m_punchElapsed = 0.0f;
		m_punchLaunched = true;
		m_glove.renderer.enabled = true;
		m_glove.collider2D.enabled = true;

		return speedDirection;
	}
}
