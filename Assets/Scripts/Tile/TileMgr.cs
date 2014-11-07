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

	private void InstantiateTile(int i, int j)
	{
		//Instantiate tiles and indicate they are blocking
		m_tiles[j * m_width + i] = Instantiate(m_tilePrefab, new Vector3(i, j), Quaternion.identity) as Tile;
		m_tiles[j * m_width + i].SetBlocking(true);
		m_tiles[j * m_width + i].transform.parent = transform;
	}

    private void Awake()
    {
        //Init tilemap size
		m_height = (uint) LevelMgr.instance.m_levelSize * 2;
		m_width  = (uint) m_height * 16 / 9;

        //Init tile array
        m_tiles = new Tile[m_width * m_height];

		//Instantiate tiles
		for(int j=0; j<m_height; ++j)
		{
			for(int i=0; i<m_width; ++i)
			{
				if(LevelMgr.instance.m_level[(m_height - 1 - j)*m_width + i])
				{
					InstantiateTile(i, j);
				}
			}
		}
    }

    public Tile GetTile(int x, int y)
    {
        return m_tiles[y * m_width + x];
    }
}
