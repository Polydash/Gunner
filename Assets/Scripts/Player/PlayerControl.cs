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
	public enum ePunchDirection
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
	public float m_gravity = 100.0f;
	public float m_jump = 25.0f;
	public float m_analog = 20.0f;
	public float m_kickBackY = 17.0f;
	public float m_maxGravity = 20.0f;
	
	//Horizontal velocity
	public float m_inAirAccelX = 75.0f;
	public float m_inAirDeaccelX = 20.0f;
    public float m_guardInAirDeaccelX = 80.0f;
	public float m_accelX = 100.0f;
	public float m_deaccelX = 75.0f;
    public float m_guardDeaccelX = 150.0f;
	public float m_kickBackX = 12.0f;
	public float m_maxVelX = 12.0f;
	
	//Punch velocity
	public float m_punchTime = 0.12f;
	public float m_punchMinVel = 500.0f;
	public float m_punchMaxVel = 2000.0f;
	public float m_punchReturnVel = 50.0f;
	public float m_punchForce = 2000.0f;
    public float m_punchForceGuarded = 500.0f;
	public float m_brokenGuardTime = 0.5f;

	//Reference to player glove, player name and FX component
	private Transform m_glove = null;
	private Transform m_playerName = null;
	private PlayerFXData m_fxComponent = null;

	//Player ID
	private int m_playerID = 0;
	
	//Player State
	public  bool m_isGrounded  {get; set;}
	public  bool m_facingRight {get; set;}
	public  bool m_hasControl  {get; set;}
	public  bool m_isGuarding  {get; set;}
	private bool m_analogJump = false;
	private float m_brokenGuardElapsed;
	private float m_horizontalVelocity;
	
	//Punch State
	private float   m_punchElapsed = 0.0f;
	private float   m_punchInertia;
	public  Vector2 m_punchDirection {get; set;}
	public  bool    m_punchRequested {get; set;}
	public  bool    m_punchLaunched  {get; set;}
	public  bool    m_punchReturning {get; set;}
	public  ePunchDirection m_requestedDirection {get; set;}

	//Input helper variables
	private bool  m_jumpPressed 	  = false;
	private bool  m_jumpReleased      = false;
	private bool  m_downPunchPressed  = false;
	private bool  m_leftPunchPressed  = false;
	private bool  m_rightPunchPressed = false;
	private bool  m_upPunchPressed	  = false;
	private bool  m_rightStickCenter  = true;
	private float m_bumperThreshold   = -0.3f;
	private float m_punchDelay 		  = 0.1f;
	private Vector2 m_moveInput;

	private void Start()
	{
		//Invert kickback
		if(m_invertKickback)
		{
			m_kickbackScale = -1.0f;
		}

		//Init guard time
		m_brokenGuardElapsed = m_brokenGuardTime;

		//Give control to the player
		m_hasControl  = false;
		m_isGuarding  = false;
		m_facingRight = true;

		//Keep the player from rotating with physics
		rigidbody2D.fixedAngle = true;
		
		//Init reference to player glove
		m_glove = transform.GetChild(0);
		m_playerName = transform.GetChild(1);
		m_fxComponent = GetComponent<PlayerFXData>();
		
		//Init player ID
		m_playerID = GetComponent<PlayerID>().GetPlayerID();

		//Set collision layer
		gameObject.layer = LayerMask.NameToLayer("P" + m_playerID.ToString());
		m_glove.gameObject.layer = LayerMask.NameToLayer("P" + m_playerID.ToString() + " Glove");
	}
	
	private void Update()
	{
		//Check inputs (for some reason, GetButtonDown does not
		//respond properly in FixedUpdate())

		//Increase broken guard time
		if(m_brokenGuardElapsed <= m_brokenGuardTime)
		{
			m_brokenGuardElapsed += Time.deltaTime;
		}

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
		if(Mathf.Abs(rigidbody2D.velocity.y) > 3.0f)
		{
			m_isGrounded = false;
		}
		
		//Set horizontal deacceleration/acceleration values
		float deaccelX, accelX;
		if(m_isGrounded)
		{
			if(m_isGuarding)
			{
				deaccelX = m_guardDeaccelX;
			}
			else
			{
				deaccelX = m_deaccelX;
			}
			accelX = m_accelX;
		}
		else
		{
			if(m_isGuarding)
			{
				deaccelX = m_guardInAirDeaccelX;
			}
			else
			{
				deaccelX = m_inAirDeaccelX;
			}
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
		if(m_moveInput.x > 0.1f && !m_facingRight && !m_punchRequested && !m_punchLaunched && !m_punchReturning)
		{
            RevertFacing();
		}
		else if(m_moveInput.x < -0.1f && m_facingRight && !m_punchRequested && !m_punchLaunched && !m_punchReturning)
		{
            RevertFacing();
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
				m_fxComponent.InstantiateBottom(PlayerFXData.eFXType.JUMP, Quaternion.identity);
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
			else
			{
				m_punchRequested = false;
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

		//Save horizontal velocity for wall rebound
		m_horizontalVelocity = rigidbody2D.velocity.x;
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
					m_isGrounded = true;
					rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0.0f);
				}
			}

			//Rebound
			if(Mathf.Abs(collision.contacts[i].normal.x) > 0.9f && Mathf.Abs(collision.contacts[i].normal.y) < 0.1f)
			{
				if(Mathf.Abs(m_horizontalVelocity) > m_maxVelX/4.0f)
				{
					rigidbody2D.velocity = new Vector2(-m_horizontalVelocity/2.0f, rigidbody2D.velocity.y);
				}
			}
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		for(int i=0; i<collision.contacts.Length; ++i)
		{
			//If player gets on the ground
			if(collision.contacts[i].normal == new Vector2(0.0f, 1.0f))
			{
				//If the player is not ascending
				if(rigidbody2D.velocity.y <= 0.0f)
				{
					m_isGrounded = true;
					rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0.0f);
				}
			}
		}
	}

    private void RevertFacing()
    {
        //Face right direction
        m_facingRight = !m_facingRight;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);

        //Revert glove local scale and horizontal local position
        m_glove.transform.localScale = new Vector2(-m_glove.transform.localScale.x, m_glove.transform.localScale.y);
        m_glove.transform.localPosition = new Vector2(-m_glove.transform.localPosition.x, m_glove.transform.localPosition.y);

        //Revert playerName local scale and horizontal local position
        m_playerName.transform.localScale = new Vector2(-m_playerName.transform.localScale.x, m_playerName.transform.localScale.y);
        m_playerName.transform.localPosition = new Vector2(-m_playerName.transform.localPosition.x, m_playerName.transform.localPosition.y);
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
                GameObject.Find("Camera").GetComponent<SoundManager>().m_playSoundWoosh = true;
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
				m_glove.GetComponent<ChainControl>().SetVisible(false);
			}
		}
	}
	
	private Vector2 LaunchPunch(ePunchDirection direction)
	{
		Vector2 speedDirection = new Vector2();

		//Reset punch request
		m_punchRequested = false;

		//Determine punch direction and local position
		switch(direction)
		{
		case ePunchDirection.DOWN :
			m_glove.transform.localPosition = new Vector3(0.0f, -0.5f);
			speedDirection = new Vector2(0.0f, -1.0f);
			break;
			
		case ePunchDirection.UP :
			m_glove.transform.localPosition = new Vector2(0.0f, 0.0f);
			speedDirection = new Vector2(0.0f, 1.0f);
			break;
			
		case ePunchDirection.RIGHT :
			m_glove.transform.localPosition = new Vector2(0.0f, -0.3f);
			speedDirection = new Vector2(1.0f, 0.0f);
			break;
			
		case ePunchDirection.LEFT :
			m_glove.transform.localPosition = new Vector2(0.0f, -0.3f);
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
		m_glove.GetComponent<ChainControl>().SetVisible(true);
		m_glove.collider2D.enabled = true;
		
		return speedDirection;
	}

	public void BreakGuard()
	{
		m_isGuarding = false;
		m_brokenGuardElapsed = 0.0f;
	}

	//Coroutine needed to delay punch launch
	IEnumerator RequestPunch(ePunchDirection direction)
	{
		m_punchRequested = true;
		m_requestedDirection = direction;
		
		//Wait
		yield return new WaitForSeconds(m_punchDelay);
		
		//Then launch delayed punch
		switch(direction)
		{
		case ePunchDirection.DOWN:
			m_downPunchPressed = true;
			break;
			
		case ePunchDirection.RIGHT:
			m_rightPunchPressed = true;
			break;
			
		case ePunchDirection.UP:
			m_upPunchPressed = true;
			break;
			
		case ePunchDirection.LEFT:
			m_leftPunchPressed = true;
			break;
		}
	}

	//Input method : We use A B X Y to match punch directions
	private void CheckInputFourButtons()
	{
		//If player is not guarding and not punching
		if(!m_isGuarding && !m_punchRequested && !m_punchLaunched && !m_punchReturning)
		{
			//Check guard bumper
			if(Input.GetAxis("P" + m_playerID.ToString() + " R2") < m_bumperThreshold && m_brokenGuardElapsed > m_brokenGuardTime)
			{
				m_isGuarding = true;
			}
			
			//Check jump button
			if(Input.GetButtonDown("P" + m_playerID.ToString() + " A"))
			{
				if(m_isGrounded)
				{
					m_jumpPressed = true;
				}
				else if(!m_isGrounded)
				{
					StartCoroutine(RequestPunch(ePunchDirection.DOWN));
				}
			}
			
			//Check left button
			if(Input.GetButtonDown("P" + m_playerID.ToString() + " X"))
			{
				StartCoroutine(RequestPunch(ePunchDirection.LEFT));
			}
			
			//Check right button
			if(Input.GetButtonDown("P" + m_playerID.ToString() + " B"))
			{
				StartCoroutine(RequestPunch(ePunchDirection.RIGHT));
			}
			
			//Check up button
			if(Input.GetButtonDown("P" + m_playerID.ToString() + " Y"))
			{
				StartCoroutine(RequestPunch(ePunchDirection.UP));
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
		}
	}

	//Input method : We use X to launch the punch, considering left stick direction
	private void CheckInputX()
	{
		if(!m_isGuarding && !m_punchRequested && !m_punchLaunched && !m_punchReturning)
		{
			//Check guard bumper
			if(Input.GetAxis("P" + m_playerID.ToString() + " R2") < m_bumperThreshold && m_brokenGuardElapsed > m_brokenGuardTime)
			{
				m_isGuarding = true;
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
							StartCoroutine(RequestPunch(ePunchDirection.RIGHT));
						}
						else if(!m_isGrounded)
						{
							StartCoroutine(RequestPunch(ePunchDirection.DOWN));
						}
					}
					else
					{
						if(Vector2.Dot(direction, new Vector2(1.0f, 1.0f)) >= 0.0f)
						{
							StartCoroutine(RequestPunch(ePunchDirection.UP));
						}
						else
						{
							StartCoroutine(RequestPunch(ePunchDirection.LEFT));
						}
					}
				}
				else if(m_facingRight)
				{
					StartCoroutine(RequestPunch(ePunchDirection.RIGHT));
				}
				else
				{
					StartCoroutine(RequestPunch(ePunchDirection.LEFT));
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
		}
	}

	//Input method : We use the right stick to launch the punch, RT to jump
	private void CheckInputRightStick()
	{ 
		if(!m_isGuarding && !m_punchRequested && !m_punchLaunched && !m_punchReturning)
		{
			//Check guard bumper
			if(Input.GetAxis("P" + m_playerID.ToString() + " R2") < m_bumperThreshold && m_brokenGuardElapsed > m_brokenGuardTime)
			{
				m_isGuarding = true;
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
							StartCoroutine(RequestPunch(ePunchDirection.RIGHT));
						}
						else if(!m_isGrounded)
						{
							StartCoroutine(RequestPunch(ePunchDirection.DOWN));
						}
					}
					else
					{
						if(Vector2.Dot(direction, new Vector2(1.0f, 1.0f)) >= 0.0f)
						{
							StartCoroutine(RequestPunch(ePunchDirection.UP));
						}
						else
						{
							StartCoroutine(RequestPunch(ePunchDirection.LEFT));
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
		}
	}
}
