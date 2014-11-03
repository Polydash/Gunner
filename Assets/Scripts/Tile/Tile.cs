using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    //Is the tile colliding ?
	private bool m_isBlocking = false;

	private void Awake()
	{
        //Note : this part is commented, because Tiles now
        //use the sprite rendering system.

        //Generate geometry
		//InitMesh();
	}

	public void SetBlocking(bool blocking)
	{
		//Check if there is a difference
		if(m_isBlocking != blocking)
		{
            //Enable/Disable collider
            BoxCollider2D collider = GetComponent("BoxCollider2D") as BoxCollider2D;
            collider.enabled = blocking;

			//Set new value
			m_isBlocking = blocking;
		}
	}

	public bool IsBlocking()
	{
		return m_isBlocking;
	}

    //Generate tile geometry
	private void InitMesh(bool withNormals = false)
	{
        //Create a mesh
        Mesh mesh = new Mesh();

        //Init vertices
		Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(0.0f, 0.0f, 0.0f);
        vertices[1] = new Vector3(0.0f, 1.0f, 0.0f);
        vertices[2] = new Vector3(1.0f, 1.0f, 0.0f);
        vertices[3] = new Vector3(1.0f, 0.0f, 0.0f);
        mesh.vertices = vertices;

        //Init UV coordinates
        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(0.0f, 0.0f);
        uv[1] = new Vector2(0.0f, 1.0f);
        uv[2] = new Vector2(1.0f, 1.0f);
        uv[3] = new Vector2(1.0f, 0.0f);
        mesh.uv = uv;

        //Init triangle indexes
        int[] indexes = new int[6];
        indexes[0] = 0;
        indexes[1] = 1;
        indexes[2] = 2;
        indexes[3] = 0;
        indexes[4] = 2;
        indexes[5] = 3;
        mesh.triangles = indexes;

        //Init normals if necessary
        if(withNormals)
        {
            Vector3[] normals = new Vector3[4];
            normals[0] = new Vector3(0.0f, 0.0f, -1.0f);
            normals[1] = new Vector3(0.0f, 0.0f, -1.0f);
            normals[2] = new Vector3(0.0f, 0.0f, -1.0f);
            normals[3] = new Vector3(0.0f, 0.0f, -1.0f);
            mesh.normals = normals;
        }
		
        //Set mesh
		mesh.RecalculateBounds();		
		MeshFilter filter = GetComponent<MeshFilter>();
		if (filter != null)
		{
			filter.sharedMesh = mesh;
		}
	}
}
	