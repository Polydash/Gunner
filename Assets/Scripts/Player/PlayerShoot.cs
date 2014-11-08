using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {


    public GameObject m_Glove;
    public float m_force = 5.0f;

    private PlayerControl scriptPC;

    private bool shooted = false;

    //Input helper variables
    private bool m_jumpPressed = false;
    private bool m_leftPunchPressed = false;
    private bool m_rightPunchPressed = false;
    private bool m_upPunchPressed = false;

    void Awake()
    {
        scriptPC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        if(!scriptPC)
        {
            Debug.Log("Error PlayerShoot Get PlayerControl");
        }
    }

	// Use this for initialization
	void Start () {

	}

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            m_jumpPressed = true;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            m_leftPunchPressed = true;
        }

        if (Input.GetButtonDown("Fire3"))
        {
            m_rightPunchPressed = true;
        }

        if (Input.GetButtonDown("Jump"))
        {
            m_upPunchPressed = true;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate() {
       //Set The Arrow At the good position and rotation
       // m_directionArrow.transform.position = transform.position;
        Vector2 DirectionInput = new Vector2(Input.GetAxis("RHorizontal"), Input.GetAxis("RVertical"));//Get the Right Stick Direction

       if (m_rightPunchPressed)
       {
           m_Glove.rigidbody2D.AddRelativeForce(new Vector2(-1, 0) * m_force);

           m_rightPunchPressed = false;

       }
        else if (m_leftPunchPressed)
       {
           m_Glove.rigidbody2D.AddRelativeForce(new Vector2(1, 0) * m_force);

           m_leftPunchPressed = false;
       }

        //TODO UP and DOWN

        //if (DirectionInput.sqrMagnitude > 0.1)
        //{
        //    if(!shooted)
        //    {
        //        shooted = true;
        //        
               
        //    }
        //}
        //else//if Sqrmagnitude == 0
        //{
        //    shooted = false;
        //}
 
 

	}


}
