using UnityEngine;
using System.Collections;

//Script setting player's edgeCollider2D points 
public class PlayerHitbox : MonoBehaviour
{
	[System.Serializable]
	public class Hitbox
	{
		public Vector2 m_center = new Vector2(0.5f, 0.5f);
		public Vector2 m_size	= new Vector2(1.0f, 1.0f);
	}

	public Hitbox m_hitbox;

	//On start, set player's hitbox points
	void Start()
	{
		//Convert hitbox center from [0.0f, 1.0f] to [-1.0f, 1.0f]
		m_hitbox.m_center = m_hitbox.m_center*2.0f - new Vector2(1.0f, 1.0f);

		Vector2[] vertices = new Vector2[8];
		
		vertices[0] = m_hitbox.m_center - m_hitbox.m_size;
		vertices[1] = new Vector2(m_hitbox.m_center.x + m_hitbox.m_size.x, m_hitbox.m_center.y - m_hitbox.m_size.y);
		vertices[2] = new Vector2(m_hitbox.m_center.x + m_hitbox.m_size.x, m_hitbox.m_center.y - m_hitbox.m_size.y);
		vertices[3] = m_hitbox.m_center + m_hitbox.m_size;
		vertices[4] = m_hitbox.m_center + m_hitbox.m_size;
		vertices[5] = new Vector2(m_hitbox.m_center.x - m_hitbox.m_size.x, m_hitbox.m_center.y + m_hitbox.m_size.y);
		vertices[6] = new Vector2(m_hitbox.m_center.x - m_hitbox.m_size.x, m_hitbox.m_center.y + m_hitbox.m_size.y);
		vertices[7] = m_hitbox.m_center - m_hitbox.m_size;
		
		EdgeCollider2D collider = collider2D as EdgeCollider2D;
		collider.points = vertices;

		//Convert hitbox center back
		m_hitbox.m_center = m_hitbox.m_center/2.0f + new Vector2(0.5f, 0.5f);
	}
}
