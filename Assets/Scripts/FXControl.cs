using UnityEngine;
using System.Collections;

public class FXControl : MonoBehaviour
{
	private float m_elapsedTime = 0.0f;
	public  float m_maxTime;

    private Animator m_animator;

	void Start()
	{
        m_animator = GetComponent<Animator>();
	}

	void Update()
	{
		if(m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
		{
			GameObject.Destroy(gameObject);
		}
	}
}
