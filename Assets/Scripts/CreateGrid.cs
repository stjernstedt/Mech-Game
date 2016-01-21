using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateGrid : MonoBehaviour
{
	public int width = 10;
	public int height = 10;
	public float cellWidth = 1f;
	public float cellHeight = 1f;
	public Dictionary<Vector3, GameObject> cells = new Dictionary<Vector3,GameObject>();

	public GameObject cellPrefab;

	Texture2D noiseTex;
	Color[] pixels;
	public float scale = 1f;

	// Use this for initialization
	void Awake()
	{
		noiseTex = new Texture2D(width, height);
		pixels = new Color[noiseTex.width * noiseTex.height];
		CreateNoise();

		float hexHeight = 1;
		float hexWidth = Mathf.Sqrt(3) / 2 * hexHeight;

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				GameObject hexagon = Instantiate(cellPrefab);
				if (y % 2 == 0)
				{
					hexagon.transform.position = new Vector3(hexWidth * x, 0, hexHeight * 3 / 4 * y);
				}
				else
				{
					hexagon.transform.position = new Vector3(hexWidth * x + hexWidth / 2, 0, hexHeight * 3 / 4 * y);
				}


				if (noiseTex.GetPixel(x, y).r < 0.3f)
				{
					hexagon.transform.localScale += new Vector3(0, 0, -0.5f);
					hexagon.GetComponent<MeshRenderer>().material.color = Color.blue;
				}

				if (noiseTex.GetPixel(x, y).r > 0.4f)
				{
					hexagon.transform.localScale += new Vector3(0, 0, 0.5f);
				}
				if (noiseTex.GetPixel(x, y).r >= 0.7f)
				{
					hexagon.transform.localScale += new Vector3(0, 0, 0.5f);
				}
				if (noiseTex.GetPixel(x, y).r >= 0.8f)
				{
					hexagon.transform.localScale += new Vector3(0, 0, 1);
				}

				cells.Add(hexagon.AddComponent<Node>().coord = new Vector3(x, y, -(x + y)), hexagon);
				//cells.Add(hexagon.AddComponent<Node>().coord = new Vector3(x, y-x, y), hexagon);
				int lol = Random.Range(1, 10);
				if (lol > 8)
				{
					hexagon.GetComponent<Node>().cost = 3;
					hexagon.GetComponent<MeshRenderer>().material.color = Color.gray;
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	void CreateNoise()
	{
		float xOrg = Random.Range(0f, 1000f);
		float yOrg = Random.Range(0f, 1000f);
		float y = 0f;
		while (y < noiseTex.height)
		{
			float x = 0f;
			while (x < noiseTex.width)
			{
				float xCoord = xOrg + x / noiseTex.width * scale;
				float yCoord = yOrg + y / noiseTex.height * scale;
				float sample = Mathf.PerlinNoise(xCoord, yCoord);
				pixels[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
				x++;
			}
			y++;
		}
		noiseTex.SetPixels(pixels);
		noiseTex.Apply();
	}
}
