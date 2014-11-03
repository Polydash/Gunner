﻿using UnityEngine;
using System.Collections;

public class CameraMgr : MonoBehaviour
{
    private void Awake()
    {
        //Get camera component and set ortho size
        Camera camera = GetComponent("Camera") as Camera;
        camera.orthographicSize = GameMgr.instance.m_levelSize;

        //Set position
		transform.position = new Vector3(GameMgr.instance.m_levelSize * 4/3, GameMgr.instance.m_levelSize, -10);
    }
}
