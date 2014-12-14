using UnityEngine;
using System.Collections;

public class FXControl : MonoBehaviour
{
	private float m_elapsedTime = 0.0f;

	void Update()
	{
		if(m_elapsedTime > GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length)
		{
			GameObject.Destroy(gameObject);
		}

		m_elapsedTime += Time.deltaTime;
	}
}
