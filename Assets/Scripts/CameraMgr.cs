using UnityEngine;
using System.Collections;

public class CameraMgr : MonoBehaviour
{
	public int m_size = 9;

	public int m_width  = 1024;
	public int m_height = 576;

//	private float m_shakeElapsed = 0.0f;
//	private float m_shakeTime = 0.0f;
//	private float m_shakeIntensity = 0.4f;

	private float   m_shakeValue = 2*Mathf.PI;
	private Vector2 m_shakeTranslate;


    private void Awake()
    {
        /*//Get camera component and set ortho size
        Camera camera = GetComponent("Camera") as Camera;
        camera.orthographicSize = LevelMgr.instance.m_levelSize;

        //Set position
		transform.position = new Vector3(LevelMgr.instance.m_levelSize * 16/9, LevelMgr.instance.m_levelSize, -10);*/
		
		//Set resolution
		//Screen.SetResolution(m_width, m_height, false);

		//Set ortho size
		camera.orthographicSize = m_size;
		
		//Set position
		transform.position = GetPosition();
    }

	private Vector3 GetPosition()
	{
		return new Vector3(m_size * 16/9, m_size, -10);
	}

	private void Update()
	{
//		transform.position = GetPosition();
//
//		if(m_shakeElapsed < m_shakeTime)
//		{
//			m_shakeElapsed += Time.deltaTime;
//			transform.position += new Vector3(Random.Range(-m_shakeIntensity, m_shakeIntensity), Random.Range(-m_shakeIntensity, m_shakeIntensity), 0.0f);
//		}

		transform.position = GetPosition();

		if(m_shakeValue <= Mathf.PI/2.0f)
		{
			m_shakeValue += Time.deltaTime * 25.0f;
			transform.position += Mathf.Sin(Mathf.PI/2.0f + m_shakeValue) * new Vector3(m_shakeTranslate.x, m_shakeTranslate.y);
			transform.position += new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f));
		}
	}

	public void Shake(float time)
	{
//		if(m_shakeElapsed < m_shakeTime)
//		{
//			if(m_shakeTime - m_shakeElapsed < time)
//			{
//				m_shakeTime = time;
//				m_shakeElapsed = 0.0f;
//			}
//		}
//		else
//		{
//			m_shakeTime = time;
//			m_shakeElapsed = 0.0f;
//		}
	}

	public void Translate(Vector2 target)
	{
		if(m_shakeValue > Mathf.PI/2.0f)
		{
			m_shakeValue = 0.0f;
			m_shakeTranslate = target;
		}
	}
}
