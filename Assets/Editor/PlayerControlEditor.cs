using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PlayerControl))]
public class PlayerControlEditor : Editor
{
	//Variables indicating folder states
	private bool m_vertical   = false;
	private bool m_horizontal = false;

	public override void OnInspectorGUI()
	{
		//Get associated script
		PlayerControl script = (PlayerControl) target;

		GUI.changed = false;

		//Vertical settings folder
		m_vertical = EditorGUILayout.Foldout(m_vertical, "Vertical movement");
		if(m_vertical)
		{
			script.m_gravity    = EditorGUILayout.FloatField("Gravity", script.m_gravity);
			script.m_jump		= EditorGUILayout.FloatField("Jump Impulse", script.m_jump);
			script.m_kickBackY  = EditorGUILayout.FloatField("Punch Kickback", script.m_kickBackY);
			script.m_maxGravity = EditorGUILayout.FloatField("Max Falling Speed", script.m_maxGravity);
		}

		//Horizontal settings folder
		m_horizontal = EditorGUILayout.Foldout(m_horizontal, "Horizontal movement");
		if(m_horizontal)
		{
			//In air settings
			EditorGUILayout.LabelField("In Air", EditorStyles.boldLabel);
			script.m_inAirAccelX   = EditorGUILayout.FloatField("Acceleration", script.m_inAirAccelX);
			script.m_inAirDeaccelX = EditorGUILayout.FloatField("Deacceleration", script.m_inAirDeaccelX);
			EditorGUILayout.Separator();

			//On ground settings
			EditorGUILayout.LabelField("On Ground", EditorStyles.boldLabel);
			script.m_accelX   = EditorGUILayout.FloatField("Acceleration", script.m_accelX);
			script.m_deaccelX = EditorGUILayout.FloatField("Deacceleration", script.m_deaccelX);
			EditorGUILayout.Separator();

			//Other settings
			script.m_kickBackX = EditorGUILayout.FloatField("Punch Kickback", script.m_kickBackX);
			script.m_maxVelX   = EditorGUILayout.FloatField("Max Speed", script.m_maxVelX);
		}

		//Apply changes
		if(GUI.changed)
		{
			EditorUtility.SetDirty(script);
		}
	}
}
