using UnityEngine;
using System.Collections;

public class CreateHexagon : MonoBehaviour
{
	public int width = 10;
	public int height = 10;
	public float size = 1f;
	public float hexagonHeight = 1f;

	// Use this for initialization
	void Start()
	{
		// positioning variables
		float hexHeight = size * 2;
		float hexWidth = Mathf.Sqrt(3) / 2 * hexHeight;

		Color color = new Color(0.5f, 0.4f, 0.7f);

		for (int z = 0; z < width; z++)
		{
			for (int x = 0; x < height; x++)
			{
				//Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
				hexagonHeight = Random.Range(1f, 3f);
				GameObject hexagon = CreateMesh();
				if (z % 2 == 0)
				{
					hexagon.transform.position = new Vector3(hexWidth * x, 0, hexHeight * 3 / 4 * z);
					hexagon.GetComponent<MeshRenderer>().material.color = color;
				}
				else
				{
					hexagon.transform.position = new Vector3(hexWidth * x + hexWidth / 2, 0, hexHeight * 3 / 4 * z);
					hexagon.GetComponent<MeshRenderer>().material.color = color;
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	GameObject CreateMesh()
	{
		Mesh mesh = new Mesh();
		Vector3[] vertices = new Vector3[14];
		int[] triangles = new int[3 * (6 + 12 + 6)];

		// adding all vertices
		for (int i = 0; i < vertices.Length; i++)
		{
			float angle_deg = 60 * i + 30;
			float angle_rad = Mathf.PI / 180 * angle_deg;
			if (i < 7)
			{
				vertices[i] = new Vector3(size * Mathf.Cos(angle_rad), 0, size * Mathf.Sin(angle_rad));
			}
			else
			{
				vertices[i] = new Vector3(size * Mathf.Cos(angle_rad), hexagonHeight, size * Mathf.Sin(angle_rad));
			}
		}
		vertices[0] = new Vector3(0, 0, 0);
		vertices[7] = new Vector3(0, hexagonHeight, 0);

		// bottom part of hexagon
		for (int i = 0; i < 6; i++)
		{
			int triValue = i * 3;
			triangles[triValue] = 0;
			triangles[triValue + 1] = i + 1;
			if (i < 5)
			{
				triangles[triValue + 2] = i + 2;
			}
			else
			{
				triangles[triValue + 2] = 1;
			}
		}

		// hexagon sides
		for (int i = 0; i < 6; i++)
		{
			int triValue = i * 6;
			if (i < 4)
			{
				triangles[triValue + 18] = i + 2;
				triangles[triValue + 19] = i + 8;
				triangles[triValue + 20] = i + 3;
				triangles[triValue + 21] = i + 3;
				triangles[triValue + 22] = i + 8;
				triangles[triValue + 23] = i + 9;
			}
			else if (i == 4)
			{
				triangles[triValue + 18] = 6;
				triangles[triValue + 19] = 12;
				triangles[triValue + 20] = 1;
				triangles[triValue + 21] = 1;
				triangles[triValue + 22] = 12;
				triangles[triValue + 23] = 13;
			}
			else
			{
				triangles[triValue + 18] = 1;
				triangles[triValue + 19] = 13;
				triangles[triValue + 20] = 2;
				triangles[triValue + 21] = 2;
				triangles[triValue + 22] = 13;
				triangles[triValue + 23] = 8;
			}
		}

		// upper part of hexagon
		for (int i = 0; i < 6; i++)
		{
			int triValue = i * 3 + 54;
			triangles[triValue] = 7;
			triangles[triValue + 2] = i + 8;
			if (i < 5)
			{
				triangles[triValue + 1] = i + 9;
			}
			else
			{
				triangles[triValue + 1] = 8;
			}
		}


		mesh.vertices = vertices;
		mesh.RecalculateBounds();
		mesh.triangles = triangles;
		mesh.RecalculateNormals();

		GameObject hexagon = new GameObject();
		hexagon.AddComponent<MeshRenderer>();
		hexagon.AddComponent<MeshFilter>().mesh = mesh;

		return hexagon;
	}

	GameObject Temp()
	{
		Mesh mesh = new Mesh();
		Vector3[] vertices = new Vector3[7];
		vertices[0] = new Vector3(0, 0, 0);
		int[] triangles = new int[18];
		for (int i = 0; i < 6; i++)
		{
			float angle_deg = 60 * i + 30;
			float angle_rad = Mathf.PI / 180 * angle_deg;
			vertices[i + 1] = new Vector3(size * Mathf.Cos(angle_rad), 0, size * Mathf.Sin(angle_rad));
			int triValue = i * 3;
			triangles[triValue] = 0;
			triangles[triValue + 2] = i + 1;
			if (i != 5)
			{
				triangles[triValue + 1] = i + 2;
			}
			else
			{
				triangles[triValue + 1] = 1;
			}
		}

		mesh.vertices = vertices;
		mesh.RecalculateBounds();
		mesh.triangles = triangles;
		mesh.RecalculateNormals();

		GameObject hexagon = new GameObject();
		hexagon.AddComponent<MeshRenderer>();
		hexagon.AddComponent<MeshFilter>().mesh = mesh;

		return hexagon;

	}
}
