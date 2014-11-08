using UnityEngine;
using System.Collections;
using System.IO;
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
			script.m_analog		= EditorGUILayout.FloatField("Analogic Jump Impulse", script.m_analog);
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

		//Save button
		if(GUILayout.Button("Save", GUILayout.Width(50.0f)))
		{
			string path = EditorUtility.SaveFilePanelInProject("Save player control parameters", "player", "plr", "Please enter a configuration name");
			
			if(path.Length > 0)
			{
				string content = "";

				content += script.m_gravity.ToString() + " ";
				content += script.m_jump.ToString() + " ";
				content += script.m_analog.ToString() + " ";
				content += script.m_kickBackY.ToString() + " ";
				content += script.m_maxGravity.ToString() + " ";
				content += script.m_inAirAccelX.ToString() + " ";
				content += script.m_inAirDeaccelX.ToString() + " ";
				content += script.m_accelX.ToString() + " ";
				content += script.m_deaccelX.ToString() + " ";
				content += script.m_kickBackX.ToString() + " ";
				content += script.m_maxVelX.ToString();

				File.WriteAllText(path, content);
				AssetDatabase.Refresh();
			}
		}

		//Load button
		if(GUILayout.Button("Load", GUILayout.Width(50.0f)))
		{
			string path = EditorUtility.OpenFilePanel("Load player control parameters", Application.dataPath, "plr");
			
			if(path.Length > 0)
			{
				string[] content = File.ReadAllLines(path);
				content = content[0].Split(' ');

				script.m_gravity       = float.Parse(content[0]);
				script.m_jump          = float.Parse(content[1]);
				script.m_analog        = float.Parse(content[2]);
				script.m_kickBackY     = float.Parse(content[3]);
				script.m_maxGravity    = float.Parse(content[4]);
				script.m_inAirAccelX   = float.Parse(content[5]);
				script.m_inAirDeaccelX = float.Parse(content[6]);
				script.m_accelX		   = float.Parse(content[7]);
				script.m_deaccelX	   = float.Parse(content[8]);
				script.m_kickBackX	   = float.Parse(content[9]);
				script.m_maxVelX	   = float.Parse(content[10]);
			}
		}

		//Apply changes
		if(GUI.changed)
		{
			EditorUtility.SetDirty(script);
		}
	}
}
