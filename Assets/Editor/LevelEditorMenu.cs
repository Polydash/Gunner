using UnityEngine;
using UnityEditor;
using System.Collections;

public class LevelEditorMenu : EditorWindow
{
	Color m_color = Color.red;

	[MenuItem ("Tools/Level Editor")]
	static void Init()
	{
		LevelEditorMenu window = EditorWindow.GetWindow<LevelEditorMenu>();
		window.title = "Level Editor";
	}

	private void OnEnable()
	{
	}

	private void OnGUI()
	{
		Event evt = Event.current;

		if(evt.type == EventType.mouseDown)
		{
			m_color = Color.blue;
		}
		else if(evt.type == EventType.mouseUp)
		{
			m_color = Color.red;
		}

		EditorGUI.DrawRect(new Rect(0, 0, evt.mousePosition.x, evt.mousePosition.y), m_color);
		Repaint();
	}
}
