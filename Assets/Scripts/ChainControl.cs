using UnityEngine;
using System.Collections;

public class ChainControl : MonoBehaviour
{
	Transform[] m_chains;

	private void Start()
	{
		m_chains = new Transform[3];

		m_chains[0] = transform.GetChild(0);
		m_chains[1] = transform.GetChild(1);
		m_chains[2] = transform.GetChild(2);
	}

	private void Update()
	{
		m_chains[0].localPosition = -transform.localPosition / 2.0f;
		m_chains[1].localPosition = -2 * transform.localPosition / 2.0f;
		m_chains[2].localPosition = -3 * transform.localPosition / 2.0f;
	}
}
