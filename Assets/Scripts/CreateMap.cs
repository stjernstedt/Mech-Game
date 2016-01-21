using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateMap : MonoBehaviour
{
	public Layout layout;
	public int width = 10;
	public int height = 10;
	public int radius = 2;
	public Vector2 s = new Vector2(0.5f, 0.5f);
	public GameObject hexagonPrefab;

	Texture2D noiseTex;
	Color[] pixels;
	public float scale = 1f;

	public Dictionary<Vector3, Node> hexes;

	// Use this for initialization
	void Awake()
	{
		//noiseTex = new Texture2D(width, height);
		//pixels = new Color[noiseTex.width * noiseTex.height];
		//CreateNoise();

		hexes = new Dictionary<Vector3, Node>();
		layout = new Layout(Layout.pointy, s, new Vector3(0, 0, 0));

		for (int x = -radius; x <= radius; x++)
		{
			int yStart = Mathf.Max(-radius, -x - radius);
			int yEnd = Mathf.Min(radius, -x + radius);
			for (int y = yStart; y <= yEnd; y++)
			{
				GameObject hex = Instantiate(hexagonPrefab);
				int z = -(x + y);
				Vector3 coord = new Vector3(x, y, z);
				hex.AddComponent<Node>().coord = coord;
				hexes.Add(coord, hex.GetComponent<Node>());
			}
		}

		GameObject cellHolder = new GameObject("Cells");
		foreach (var hex in hexes)
		{
			Node node = hex.Value.GetComponent<Node>();
			node.transform.SetParent(cellHolder.transform);

			float heightValue = Random.Range(1f, 10f);
			if (heightValue > 7f && heightValue < 8f)
			{
				node.transform.localScale = new Vector3(1, 1, 1.3f);
				node.cost *= 2;
			}
			if (heightValue >= 8f && heightValue < 9f)
			{
				node.transform.localScale = new Vector3(1, 1, 1.6f);
				node.cost *= 3;
			}
			if (heightValue >= 9f)
			{
				node.transform.localScale = new Vector3(1, 1, 3f);
				node.walkable = false;
			}


			node.transform.position = Layout.HexToPixel(layout, node);
		}
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

public struct Orientation
{
	public Orientation(float f0, float f1, float f2, float f3, float b0, float b1, float b2, float b3, float start_angle)
	{
		this.f0 = f0;
		this.f1 = f1;
		this.f2 = f2;
		this.f3 = f3;
		this.b0 = b0;
		this.b1 = b1;
		this.b2 = b2;
		this.b3 = b3;
		this.start_angle = start_angle;
	}
	public readonly float f0;
	public readonly float f1;
	public readonly float f2;
	public readonly float f3;
	public readonly float b0;
	public readonly float b1;
	public readonly float b2;
	public readonly float b3;
	public readonly float start_angle;
}

public struct Layout
{
	public Layout(Orientation orientation, Vector2 size, Vector3 origin)
	{
		this.orientation = orientation;
		this.size = size;
		this.origin = origin;
	}
	public readonly Orientation orientation;
	public readonly Vector2 size;
	public readonly Vector3 origin;
	static public Orientation pointy = new Orientation(Mathf.Sqrt(3.0f), Mathf.Sqrt(3.0f) / 2.0f, 0.0f, 3.0f / 2.0f, Mathf.Sqrt(3.0f) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f, 0.5f);
	static public Orientation flat = new Orientation(3.0f / 2.0f, 0.0f, Mathf.Sqrt(3.0f) / 2.0f, Mathf.Sqrt(3.0f), 2.0f / 3.0f, 0.0f, -1.0f / 3.0f, Mathf.Sqrt(3.0f) / 3.0f, 0.0f);

	static public Vector3 HexToPixel(Layout layout, Node h)
	{
		Orientation M = layout.orientation;
		Vector2 size = layout.size;
		Vector2 origin = layout.origin;
		float x = (M.f0 * h.coord.x + M.f1 * h.coord.z) * size.x;
		float y = (M.f2 * h.coord.x + M.f3 * h.coord.z) * size.y;
		return new Vector3(x + origin.x, 0, y + origin.y);
	}

	static public Vector3 PixelToHex(Layout layout, Vector3 p)
	{
		Orientation M = layout.orientation;
		Vector2 size = layout.size;
		Vector2 origin = layout.origin;
		Vector2 pt = new Vector2((p.x - origin.x) / size.x, (p.z - origin.y) / size.y);
		float x = M.b0 * pt.x + M.b1 * pt.y;
		float y = M.b2 * pt.x + M.b3 * pt.y;
		x = Mathf.Round(x);
		y = Mathf.Round(y);
		return new Vector3((int)x, (int)y, (int)(-x - y));
	}
	//static public Vector3 PixelToHex(Layout layout, Vector3 p)
	//{
	//	Vector2 size = layout.size;
	//	float x = (p.x * Mathf.Sqrt(3) / 3 - p.z / 3) / size.x;
	//	float y = p.z * 2 / 3 / size.x;

	//	return new Vector3((int)Mathf.Round(x), (int)Mathf.Round(y), (int)Mathf.Round(-(x + y)));
	//}

	//static public Vector3 PixelToHex(Layout layout, Vector3 p)
	//{
	//	Vector2 size = layout.size;
	//	float x = p.x * 2 / 3 / size.x;
	//	float y = (-p.x / 3 + Mathf.Sqrt(3) / 3 * p.z) / size.x;

	//	return new Vector3((int)Mathf.Round(x), (int)Mathf.Round(y), (int)Mathf.Round(-(x + y)));
	//}


}