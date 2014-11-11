using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	//Enumeration defining a punch direction
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
	public float m_punchTime   	  = 0.12f;
	public float m_punchMinVel 	  = 500.0f;
	public float m_punchMaxVel    = 2000.0f;
	public float m_punchReturnVel = 50.0f;
	public float m_punchForce	  = 2000.0f;

	//Reference to player glove
	private Transform m_glove = null;

	//Player ID
	private int m_playerID = 0;

	//Player State
	private bool m_isGrounded 	 = false;
	private bool m_analogJump 	 = false;

	//Punch State
	private float m_punchElapsed = 0.0f;
	private Vector2 m_punchDirection;
	public bool m_punchLaunched  {get; set;}
	public bool m_punchReturning {get; set;}

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

		//Init player ID
		m_playerID = GetComponent<PlayerID>().GetPlayerID();
	}

	private void Update()
	{
		//Check inputs (for some reason, GetButtonDown does not
		//respond properly in FixedUpdate())

		//Check jump button
		if(Input.GetButtonDown("P" + m_playerID.ToString() + " A"))
		{
			m_jumpPressed = true;
		}
		else if(Input.GetButtonUp("P" + m_playerID.ToString() + " A"))
		{
			m_jumpReleased = true;
		}

		//Check left button
		if(Input.GetButtonDown("P" + m_playerID.ToString() + " X"))
		{
			m_leftPunchPressed = true;
		}

		//Check right button
		if(Input.GetButtonDown("P" + m_playerID.ToString() + " B"))
		{
			m_rightPunchPressed = true;
		}

		//Check up button
		if(Input.GetButtonDown("P" + m_playerID.ToString() + " Y"))
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
		Vector2 moveInput = new Vector2(Input.GetAxis("P" + m_playerID.ToString() + " Horizontal"), 0.0f);
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
		if(m_leftPunchPressed)
		{
			if(!m_punchLaunched && !m_punchReturning)
			{
				m_punchDirection = LaunchPunch(ePunchDirection.LEFT);
				rigidbody2D.velocity += new Vector2(m_kickBackX, 0.0f);
			}

			m_leftPunchPressed = false;
		}

		//Check right punch
		if(m_rightPunchPressed)
		{
			if(!m_punchLaunched && !m_punchReturning)
			{
				m_punchDirection = LaunchPunch(ePunchDirection.RIGHT);
				rigidbody2D.velocity -= new Vector2(m_kickBackX, 0.0f);
			}

			m_rightPunchPressed = false;
		}

		//Update punch movement
		UpdatePunch();

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

	//This method updates the punch velocity, and should only be used in FixedUpdate
	private void UpdatePunch()
	{
		//If punch is launched
		if(m_punchLaunched)
		{
			//Update its speed if it is not done
			if(m_punchElapsed < m_punchTime)
			{
				m_punchElapsed += Time.deltaTime;
				float speed = Mathf.Lerp(m_punchMinVel, m_punchMaxVel, 1.0f - m_punchElapsed / m_punchTime);
				m_glove.rigidbody2D.velocity = speed * m_punchDirection * Time.deltaTime;
			}
			//Or make it return back to the player
			else
			{
				m_punchLaunched = false;
				m_punchReturning = true;
				m_glove.rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
				m_glove.collider2D.enabled = false;
			}
		}

		//If punch is returning
		if(m_punchReturning)
		{
			//Update its position if it is not done
			if(m_punchReturnVel * Time.deltaTime < m_glove.transform.localPosition.magnitude)
			{
				m_glove.transform.localPosition -= m_punchReturnVel * Time.deltaTime * Vector3.Normalize(m_glove.transform.localPosition);
			}
			//Or make it available to the player for another shot
			else
			{
				m_punchReturning = false;
				m_glove.renderer.enabled = false;
			}
		}
	}

	private Vector2 LaunchPunch(ePunchDirection direction)
	{
		Vector2 speedDirection = new Vector2();

		//Determine punch direction and local position
		switch(direction)
		{
		case ePunchDirection.DOWN :
			m_glove.transform.localPosition = new Vector2(0.0f, -1.5f);
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

		//Init punch parameters
		m_punchElapsed = 0.0f;
		m_punchLaunched = true;
		m_glove.renderer.enabled = true;
		m_glove.collider2D.enabled = true;

		return speedDirection;
	}
}
