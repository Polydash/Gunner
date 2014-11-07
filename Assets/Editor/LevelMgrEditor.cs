using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

[CustomEditor(typeof(LevelMgr))]
public class LevelMgrEditor : Editor
{
	private void ResizeLevel(LevelMgr script, int oldSize, int newSize, bool copyOldData = false)
	{
		//Get level size
		script.m_levelSize = newSize;
		
		//Compute area size
		uint height = (uint) script.m_levelSize * 2;
		uint width  = (uint) height * 16 / 9;
		
		if(script.m_level == null || (oldSize != script.m_levelSize && script.m_levelSize > 0))
		{
			//Allocate array
			bool[] newLevel = new bool[height * width]; 

			if(copyOldData)
			{
				//Copy as much data as possible
				for(int i=0; i<height*width && i<script.m_level.Length; ++i)
				{
					newLevel[i] = script.m_level[i];
				}
			}
			
			//Assign new array
			script.m_level = newLevel;
		}
	}

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

		//Resize level if necessary
		ResizeLevel(script, oldSize, script.m_levelSize, true);

		//Draw checkboxes
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
		if(GUILayout.Button("Clear", GUILayout.Width(50.0f)))
		{
			for(int i=0; i<height*width; ++i)
			{
				script.m_level[i] = false;
			}
		}

		//Save button
		if(GUILayout.Button("Save", GUILayout.Width(50.0f)))
		{
			string path = EditorUtility.SaveFilePanelInProject("Save level", "level", "lvl", "Please enter a level name");

			if(path.Length > 0)
			{
				string content = "";
				content += script.m_levelSize.ToString() + "\n";

				for(int i=0; i<height*width; ++i)
				{
					if(script.m_level[i])
					{
						content += "1";
					}
					else
					{
						content += "0";
					}
				}

				File.WriteAllText(path, content);
				AssetDatabase.Refresh();
			}
		}

		//Load button
		if(GUILayout.Button("Load", GUILayout.Width(50.0f)))
		{
			string path = EditorUtility.OpenFilePanel("Load level", Application.dataPath, "lvl");
	
			if(path.Length > 0)
			{
				string[] content = File.ReadAllLines(path);

				oldSize = script.m_levelSize;
				script.m_levelSize = int.Parse(content[0]);

				ResizeLevel(script, oldSize, script.m_levelSize);

				height = (uint) script.m_levelSize * 2;
				width  = (uint) height * 16 / 9;

				for(int i=0; i<height*width; ++i)
				{
					if(content[1][i] == '1')
					{
						script.m_level[i] = true;
					}
					else
					{
						script.m_level[i] = false;
					}
				}
			}
		}

		//Apply changes
		if(GUI.changed)
		{
			EditorUtility.SetDirty(script);
		}
	}
}
