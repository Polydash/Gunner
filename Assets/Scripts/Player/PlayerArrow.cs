using UnityEngine;
using System.Collections;

public class PlayerArrow : MonoBehaviour {

    public GameObject m_directionArrow;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Set The Arrow At the good position and rotation
        // m_directionArrow.transform.position = transform.position;
        Vector2 DirectionInput = new Vector2(Input.GetAxis("RVertical"), Input.GetAxis("RHorizontal"));//Get the Right Stick Direction
        DirectionInput.Normalize();//Normalize it

        // Debug.Log(DirectionInputCpy);
        float angle = Vector2.Angle(new Vector2(1, 0), DirectionInput);
        float sign = Mathf.Sign(Vector3.Dot(Vector3.forward, Vector3.Cross(new Vector2(1, 0), DirectionInput)));
        angle = ((angle * sign) + 180) % 360;// Put the angle between 0 and 360 degrees

        if (DirectionInput.sqrMagnitude > 0.1)
        {
            m_directionArrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
	}
}
