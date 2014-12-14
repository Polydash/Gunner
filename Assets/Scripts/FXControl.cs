using UnityEngine;
using System.Collections;

public class FXControl : MonoBehaviour
{
	private float m_elapsedTime = 0.0f;
	public  float m_maxTime;

	void Start()
	{		
	}

	void Update()
	{
		if(m_elapsedTime > m_maxTime)
		{
			GameObject.Destroy(gameObject);
		}

		m_elapsedTime += Time.deltaTime;
	}
}
