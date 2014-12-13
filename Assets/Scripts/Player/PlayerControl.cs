using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	//Enumeration defining a control type
	public enum eControlType
	{
		FOUR_BUTTONS,
		X,
		RIGHT_STICK
	};
	
	//Enumeration defining a punch direction
	private enum ePunchDirection
	{
		RIGHT,
		LEFT, 
		UP,
		DOWN
	};
	
	//Defines a control type
	public eControlType m_controlType;
	
	//Invert kickback helpers
	public bool m_invertKickback = false;
	private float m_kickbackScale = 1.0f;
	
	//Vertical velocity
	public float m_gravity	     = 100.0f;
	public float m_jump		     = 25.0f;
	public float m_analog		 = 20.0f;
	public float m_kickBackY	 = 17.0f;
	public float m_maxGravity    = 20.0f;
	
	// velocity
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
	public  bool m_isGrounded  {get; set;}
	public  bool m_facingRight {get; set;}
	public  bool m_hasControl  {get; set;}
	public  bool m_isGuarding  {get; set;}
	private bool m_analogJump = false;
	
	//Punch State
	private float   m_punchElapsed = 0.0f;
	private float   m_punchInertia;
	public  Vector2 m_punchDirection {get; set;}
	public  bool    m_punchLaunched  {get; set;}
	public  bool    m_punchReturning {get; set;}
	
	//Input helper variables
	private bool m_jumpPressed 		 = false;
	private bool m_jumpReleased		 = false;
	private bool m_downPunchPressed  = false;
	private bool m_leftPunchPressed  = false;
	private bool m_rightPunchPressed = false;
	private bool m_upPunchPressed	 = false;
	private bool m_rightStickCenter  = true;
	private float m_bumperThreshold  = -0.3f;
	private Vector2 m_moveInput;
	
	private void Start()
	{
		//Invert kickback
		if(m_invertKickback)
		{
			m_kickbackScale = -1.0f;
		}

		//Give control to the player
		m_hasControl  = true;
		m_isGuarding  = false;
		m_facingRight = true;

		//Keep the player from rotating with physics
		rigidbody2D.fixedAngle = true;
		
		//Init reference to player glove
		m_glove = transform.GetChild(0);
		
		//Init player ID
		m_playerID = GetComponent<PlayerID>().GetPlayerID();
	}
	
	private void CheckInputFourButtons()
	{
		//If player is not guarding
		if(!m_isGuarding)
		{
			//Check guard bumper
			if(Input.GetAxis("P" + m_playerID.ToString() + " R2") < m_bumperThreshold)
			{
				m_isGuarding = true;
				renderer.material.color = Color.red;
			}

			//Check jump button
			if(Input.GetButtonDown("P" + m_playerID.ToString() + " A"))
			{
				if(m_isGrounded)
				{
					m_jumpPressed = true;
				}
				else
				{
					m_downPunchPressed = true;
				}
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

		//Check input release
		if(Input.GetButtonUp("P" + m_playerID.ToString() + " A"))
		{
			m_jumpReleased = true;
		}

		if(Input.GetAxis("P" + m_playerID.ToString() + " R2") > m_bumperThreshold)
		{
			m_isGuarding = false;
			renderer.material.color = Color.white;
		}
	}
	
	private void CheckInputX()
	{
		if(!m_isGuarding)
		{
			//Check guard bumper
			if(Input.GetAxis("P" + m_playerID.ToString() + " R2") < m_bumperThreshold)
			{
				m_isGuarding = true;
				renderer.material.color = Color.red;
			}

			if(Input.GetButtonDown("P" + m_playerID.ToString() + " A"))
			{
				m_jumpPressed = true;
			}
			
			if(Input.GetButtonDown("P" + m_playerID.ToString() + " X"))
			{
				Vector2 direction = new Vector2(Input.GetAxis("P" + m_playerID.ToString() + " LHorizontal"),
				                                Input.GetAxis("P" + m_playerID.ToString() + " LVertical"));
				
				if(direction.sqrMagnitude > 0.5f)
				{
					if(Vector2.Dot(direction, new Vector2(1.0f, -1.0f)) >= 0.0f)
					{
						if(Vector2.Dot(direction, new Vector2(1.0f, 1.0f)) >= 0.0f)
						{
							m_rightPunchPressed = true;
						}
						else
						{
							m_downPunchPressed = true;
						}
					}
					else
					{
						if(Vector2.Dot(direction, new Vector2(1.0f, 1.0f)) >= 0.0f)
						{
							m_upPunchPressed = true;
						}
						else
						{
							m_leftPunchPressed = true;
						}
					}
				}
				else if(m_facingRight)
				{
					m_rightPunchPressed = true;
				}
				else
				{
					m_leftPunchPressed = true;
				}
			}
		}

		//Check input release
		if(Input.GetButtonUp("P" + m_playerID.ToString() + " A"))
		{
			m_jumpReleased = true;
		}

		if(Input.GetAxis("P" + m_playerID.ToString() + " R2") > m_bumperThreshold)
		{
			m_isGuarding = false;
			renderer.material.color = Color.white;
		}
	}
	
	private void CheckInputRightStick()
	{ 
		if(!m_isGuarding)
		{
			//Check guard bumper
			if(Input.GetAxis("P" + m_playerID.ToString() + " R2") < m_bumperThreshold)
			{
				m_isGuarding = true;
				renderer.material.color = Color.red;
			}

			if(Input.GetButtonDown("P" + m_playerID.ToString() + " R1"))
			{
				m_jumpPressed = true;
			}
			
			Vector2 direction = new Vector2(Input.GetAxis("P" + m_playerID.ToString() + " RHorizontal"),
			                                Input.GetAxis("P" + m_playerID.ToString() + " RVertical"));
			
			if(direction.sqrMagnitude > 0.75f)
			{
				if(m_rightStickCenter)
				{
					if(Vector2.Dot(direction, new Vector2(1.0f, -1.0f)) >= 0.0f)
					{
						if(Vector2.Dot(direction, new Vector2(1.0f, 1.0f)) >= 0.0f)
						{
							m_rightPunchPressed = true;
						}
						else
						{
							m_downPunchPressed = true;
						}
					}
					else
					{
						if(Vector2.Dot(direction, new Vector2(1.0f, 1.0f)) >= 0.0f)
						{
							m_upPunchPressed = true;
						}
						else
						{
							m_leftPunchPressed = true;
						}
					}
					
					m_rightStickCenter = false;
				}
			}
			else
			{
				m_rightStickCenter = true;
			}
		}

		//Check input release
		if(Input.GetButtonUp("P" + m_playerID.ToString() + " R1"))
		{
			m_jumpReleased = true;
		}

		if(Input.GetAxis("P" + m_playerID.ToString() + " R2") > m_bumperThreshold)
		{
			m_isGuarding = false;
			renderer.material.color = Color.white;
		}
	}
	
	private void Update()
	{
		//Check inputs (for some reason, GetButtonDown does not
		//respond properly in FixedUpdate())
		
		//Only check inputs if player has control
		if(m_hasControl)
		{	
			switch(m_controlType)
			{
			case eControlType.FOUR_BUTTONS :
				CheckInputFourButtons();
				break;
				
			case eControlType.X :
				CheckInputX();
				break;
				
			case eControlType.RIGHT_STICK :
				CheckInputRightStick();
				break;
				
			default :
				break;
			}
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


		if(m_hasControl && !m_isGuarding)
		{
			//Check horizontal input
			m_moveInput = new Vector2(Input.GetAxis("P" + m_playerID.ToString() + " LHorizontal"), 0.0f);
		}
		else
		{
			//Reset horizontal input
			m_moveInput = new Vector2(0.0f, 0.0f);
		}

		//Revert player
		if(m_moveInput.x > 0.1f && !m_facingRight)
		{
			//Face right direction
			m_facingRight = true;
			transform.localScale = new Vector2(1.0f, transform.localScale.y);
			
			//Set glove local scale accordingly
			float scale = Mathf.Abs(m_glove.transform.localScale.x);
			m_glove.transform.localScale = new Vector2(scale, m_glove.transform.localScale.y);
			
			//And revert horizontal position
			m_glove.transform.localPosition = new Vector2(-m_glove.transform.localPosition.x, m_glove.transform.localPosition.y);
		}
		else if(m_moveInput.x < -0.1f && m_facingRight)
		{
			//Face left direction
			m_facingRight = false;
			transform.localScale = new Vector2(-1.0f, transform.localScale.y);
			
			//Set glove local scale accordingly
			float scale = Mathf.Abs(m_glove.transform.localScale.x);
			m_glove.transform.localScale = new Vector2(-scale, m_glove.transform.localScale.y);
			
			//And revert horizontal position
			m_glove.transform.localPosition = new Vector2(-m_glove.transform.localPosition.x, m_glove.transform.localPosition.y);
		}
		
		//Multiply by acceleration value
		m_moveInput.x *= accelX;
		
		//If player is not moving, or if current velocity is above max value
		if(Mathf.Abs(m_moveInput.x) < 0.01f || Mathf.Abs(rigidbody2D.velocity.x) > m_maxVelX)
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
		else if(m_moveInput.x * rigidbody2D.velocity.x > 0.0f)
		{
			//Apply horizontal input, considering max speed
			rigidbody2D.velocity += new Vector2(Mathf.Clamp(m_moveInput.x * Time.deltaTime,
			                                                Mathf.Min(0.0f, -m_maxVelX - rigidbody2D.velocity.x),
			                                                Mathf.Max(0.0f, m_maxVelX - rigidbody2D.velocity.x)), 0.0f);
		}
		else
		{
			//Apply horizontal input
			rigidbody2D.velocity += new Vector2(m_moveInput.x * Time.deltaTime, 0.0f);
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
		
		//Check down punch
		if(m_downPunchPressed)
		{
			if(!m_isGrounded && !m_punchLaunched && !m_punchReturning)
			{
				m_punchDirection = LaunchPunch(ePunchDirection.DOWN);
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, m_kickbackScale * m_kickBackY);
			}
			
			m_downPunchPressed = false;
		}
		
		//Check up punch
		if(m_upPunchPressed)
		{
			if(!m_punchLaunched && !m_punchReturning)
			{
				m_punchDirection = LaunchPunch(ePunchDirection.UP);
				
				if(!m_isGrounded)
				{
					rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, m_kickbackScale * -m_kickBackY);
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
				Vector2 kickback = new Vector2(m_kickbackScale * m_kickBackX, 0.0f);
				rigidbody2D.velocity += kickback;
			}
			
			m_leftPunchPressed = false;
		}
		
		//Check right punch
		if(m_rightPunchPressed)
		{
			if(!m_punchLaunched && !m_punchReturning)
			{
				m_punchDirection = LaunchPunch(ePunchDirection.RIGHT);
				Vector2 kickback = new Vector2(m_kickbackScale * m_kickBackX, 0.0f);
				rigidbody2D.velocity -= kickback;
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
				m_glove.rigidbody2D.velocity = new Vector2(m_punchInertia, 0.0f) + speed * m_punchDirection * Time.deltaTime;
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
			m_glove.transform.localPosition = new Vector3(0.0f, -1.5f);
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
		
		//Consider the facing direction
		m_glove.transform.localPosition = new Vector2(transform.localScale.x*m_glove.transform.localPosition.x, m_glove.transform.localPosition.y);
		
		//Init punch parameters
		m_punchElapsed  = 0.0f;
		Vector2 inertia = Vector2.Dot(rigidbody2D.velocity, speedDirection) * speedDirection;
		m_punchInertia  = inertia.x;
		m_punchLaunched = true;
		m_glove.renderer.enabled = true;
		m_glove.collider2D.enabled = true;
		
		return speedDirection;
	}
}
