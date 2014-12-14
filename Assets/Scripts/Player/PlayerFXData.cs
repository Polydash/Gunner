using UnityEngine;
using System.Collections;

public class PlayerFXData : MonoBehaviour
{
	//FXType
	public enum eFXType
	{
		JUMP  = 0,
		RUN   = 1,
		TOTAL = 2
	};

	//FXSettings
	[System.Serializable]
	public struct FXSettings
	{
		public Transform prefab;
		public  float elapsed;
		public  float interval;
	};

	//List of settings
	public FXSettings[] m_fxSettings;

	//Jump
	public Transform m_FXJumpSmoke;

	//Run
	public float m_FXRunSmoke;

	public void Start()
	{
		if(m_fxSettings.Length != (int) eFXType.TOTAL)
		{
			Debug.LogError("PlayerFXData might not be organized");
		}

		for(int i=0; i<m_fxSettings.Length; ++i)
		{
			m_fxSettings[i].elapsed = 0.0f;
		}
	}

	public void Update()
	{
		for(int i=0; i<m_fxSettings.Length; ++i)
		{
			if(m_fxSettings[i].elapsed < m_fxSettings[i].interval)
			{
				m_fxSettings[i].elapsed += Time.deltaTime;
			}
		}
	}

	public void InstantiateBottom(eFXType type, Quaternion rotation)
	{
		if(m_fxSettings[(int) type].elapsed >= m_fxSettings[(int) type].interval)
		{
			m_fxSettings[(int) type].elapsed = 0.0f;
			Vector3 pos = transform.position + new Vector3(0.0f, -1.0f);
			Instantiate(m_fxSettings[(int) type].prefab, pos, rotation);
		}
	}
}
