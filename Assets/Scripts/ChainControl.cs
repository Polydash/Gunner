using UnityEngine;
using System.Collections;

public class ChainControl : MonoBehaviour
{
	Transform[] m_chains;

	public Vector2 m_centerOffset;

	private void Start()
	{
		m_chains = new Transform[3];

		m_chains[0] = transform.GetChild(0);
		m_chains[1] = transform.GetChild(1);
		m_chains[2] = transform.GetChild(2);
	}

	private void Update()
	{
		Vector2 playerPos = transform.parent.position;
		playerPos += m_centerOffset;
		Vector2 gloveDist = new Vector2(transform.position.x, transform.position.y) - playerPos;

		for(int i=0; i<m_chains.Length; ++i)
		{
			m_chains[i].position = playerPos + ((float)i+1)/(m_chains.Length+1) * gloveDist;
		}
	}

	public void SetVisible(bool enable)
	{
		m_chains[0].renderer.enabled = enable;
		m_chains[1].renderer.enabled = enable;
		m_chains[2].renderer.enabled = enable;
	}
}
