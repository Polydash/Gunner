using UnityEngine;
using System.Collections;

public class CameraMgr : MonoBehaviour
{
    private void Awake()
    {
        //Get camera component and set ortho size
        Camera camera = GetComponent("Camera") as Camera;
        camera.orthographicSize = LevelMgr.instance.m_levelSize;

        //Set position
		transform.position = new Vector3(LevelMgr.instance.m_levelSize * 16/9, LevelMgr.instance.m_levelSize, -10);
    }
}
