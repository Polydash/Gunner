using UnityEngine;
using System.Collections;

public class TileMgr : MonoBehaviour
{
    //Tile prefab
    public Tile m_tilePrefab;

    //Tile array
    private Tile[] m_tiles;

    //Tilemap size
    private uint m_width;
    private uint m_height;

    private void Awake()
    {
        //Init tilemap size
		m_height = (uint) GameMgr.instance.m_levelSize * 2;
		m_width  = (uint) m_height * 4 / 3;

        //Init tile array
        m_tiles = new Tile[m_width * m_height];

        //Temporary : Instantiate tiles on borders
        for(int i=0; i<m_width; ++i)
        {
            for(int j=0; j<m_height; ++j)
            {
				//Instantiate borders blocks
                if(i == 0 || j == 0 || i == (m_width - 1) || j == (m_height - 1))
                {
                    //Instantiate tiles and indicate they are blocking
                    m_tiles[j * m_width + i] = Instantiate(m_tilePrefab, new Vector3(i, j), Quaternion.identity) as Tile;
                    m_tiles[j * m_width + i].SetBlocking(true);
                    m_tiles[j * m_width + i].transform.parent = transform;
                }
            }
        }
    }

    public Tile GetTile(int x, int y)
    {
        return m_tiles[y * m_width + x];
    }
}
