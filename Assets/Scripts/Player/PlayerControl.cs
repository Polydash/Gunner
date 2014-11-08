using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
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
	
	//Player State
	private bool m_isGrounded = false;
	private bool m_analogJump = false;

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
		if(Input.GetButtonDown("Fire2"))
		{
			m_leftPunchPressed = true;
		}

		//Check right button
		if(Input.GetButtonDown("Fire3"))
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
			else
			{
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
			if(!m_isGrounded)
			{
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -m_kickBackY);
			}

			m_upPunchPressed = false;
		}

		//Check left punch
		if(m_leftPunchPressed)
		{
			rigidbody2D.velocity -= new Vector2(m_kickBackX, 0.0f);
			m_leftPunchPressed = false;
		}

		//Check right punch
		if(m_rightPunchPressed)
		{
			rigidbody2D.velocity += new Vector2(m_kickBackX, 0.0f);
			m_rightPunchPressed = false;
		}

		//Apply gravity
		rigidbody2D.velocity -= new Vector2(0.0f, m_gravity * Time.deltaTime);

		//Only clamp vertical velocity
		rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x,
		                                   Mathf.Max(rigidbody2D.velocity.y, -m_maxGravity));

		//Clamp velocity to maximum values
		//rigidbody2D.velocity = new Vector2(Mathf.Clamp(rigidbody2D.velocity.x, -m_maxVelX, m_maxVelX),
		//                         		   Mathf.Max(rigidbody2D.velocity.y, -m_maxGravity));
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
}
