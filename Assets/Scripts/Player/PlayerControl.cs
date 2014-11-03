using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	//Vertical velocity
	public float m_gravity	     = 100.0f;
	public float m_jump		     = 25.0f;
	public float m_maxGravity    = 20.0f;

	//Horizontal velocity
	public float m_inAirAccelX   = 75.0f;
	public float m_inAirDeaccelX = 20.0f;
	public float m_accelX	     = 100.0f;
	public float m_deaccelX      = 75.0f;
	public float m_maxVelX       = 12.0f;
	
	//Player State
	private bool m_isGrounded = false;

	//Input helper variables
	private bool m_jumpPressed = false;

	private void Start()
	{
		//Keep the player from rotating with physics
		rigidbody2D.fixedAngle = true;
	}

	private void Update()
	{
		//Check jump
		if(Input.GetButtonDown("Fire1"))
		{
			m_jumpPressed = true;
		}
	}

	private void FixedUpdate()
	{
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

		//Apply horizontal input
		rigidbody2D.velocity += new Vector2(moveInput.x * Time.deltaTime, 0.0f);

		if(Mathf.Abs(moveInput.x) < 0.01f)
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

		//Check jump
		if(m_jumpPressed)
		{
			if(m_isGrounded)
			{
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, m_jump);
				m_isGrounded = false;
			}

			m_jumpPressed = false;
		}

		//Apply gravity
		rigidbody2D.velocity -= new Vector2(0.0f, m_gravity * Time.deltaTime);

		//Clamp velocity to maximum values
		rigidbody2D.velocity = new Vector2(Mathf.Clamp(rigidbody2D.velocity.x, -m_maxVelX, m_maxVelX),
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
				}
			}
		}
	}
}
