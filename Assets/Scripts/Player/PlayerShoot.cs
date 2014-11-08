using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {

    public GameObject m_directionArrow;
    public GameObject m_bullet;

    public float m_BulletSpawnDistanceFromPlayer = 3.0f;
    public float m_initialBulletForce = 100.0f;
    public int m_numberOfBullets = 3;
    public float m_shootAngle = 10.0f; //In Degrees


    private bool shooted = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate() {
        //Set The Arrow At the good position and rotation
       // m_directionArrow.transform.position = transform.position;
        Vector2 DirectionInput = new Vector2(Input.GetAxis("RHorizontal"), Input.GetAxis("RVertical"));//Get the Right Stick Direction
        DirectionInput.Normalize();//Normalize it

        float angle = Vector2.Angle(new Vector2(1, 0), DirectionInput);
        float sign = Mathf.Sign(Vector3.Dot(Vector3.forward, Vector3.Cross(new Vector2(1, 0), DirectionInput)));
        angle = ((angle * sign) + 180) % 360;// Put the angle between 0 and 360 degrees

        if (DirectionInput.sqrMagnitude > 0.1)
        {
             m_directionArrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if(!shooted)
            {
                shooted = true;
                Debug.Log(DirectionInput);
                for(int i = 0; i < m_numberOfBullets; ++i)
                {
                    Vector2 newPosition = new Vector2(transform.position.x + (DirectionInput.x * m_BulletSpawnDistanceFromPlayer) ,transform.position.y + (DirectionInput.y * m_BulletSpawnDistanceFromPlayer ));
                    GameObject newBullet = Object.Instantiate(m_bullet, -newPosition, Quaternion.identity) as GameObject;
                    //newBullet.rigidbody2D.AddRelativeForce(DirectionInput * m_initialBulletForce);
                }
               

            }
        }
        else//if Sqrmagnitude == 0
        {
            shooted = false;
        }
 
 

	}


}
