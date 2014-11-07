using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelMgr))]
public class LevelMgrEditor : Editor
{
	public override void OnInspectorGUI()
	{
		//Get associated script
		LevelMgr script = (LevelMgr) target;

		GUI.changed = false;

		//Get level size
		int oldSize = script.m_levelSize;
		script.m_levelSize = EditorGUILayout.IntField("Level Size", script.m_levelSize);

		//Compute area size
		uint height = (uint) script.m_levelSize * 2;
		uint width  = (uint) height * 16 / 9;

		if(script.m_level == null || (oldSize != script.m_levelSize && script.m_levelSize > 0))
		{
			//Allocate array
			bool[] newLevel = new bool[height * width]; 

			//Copy as much data as possible
			for(int i=0; i<height*width && i<script.m_level.Length; ++i)
			{
				newLevel[i] = script.m_level[i];
			}

			//Assign new array
			script.m_level = newLevel;
		}

		for(int i=0; i<height; ++i)
		{
			EditorGUILayout.BeginHorizontal();
			for(int j=0; j<width; ++j)
			{
				script.m_level[i*width + j] = EditorGUILayout.Toggle(script.m_level[i*width + j], GUILayout.Width(10.0f), GUILayout.Height(12.0f));
			}
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.Separator();

		//Clear button
		if(GUILayout.Button("Clear",  GUILayout.Width(50.0f)))
		{
			for(int i=0; i<height*width; ++i)
			{
				script.m_level[i] = false;
			}
		}

		//Apply changes
		if(GUI.changed)
		{
			EditorUtility.SetDirty(script);
		}
	}
}
