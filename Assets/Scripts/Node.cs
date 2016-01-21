using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
	public Vector3 _coord;
	public Vector3 coord
	{
		get
		{
			return _coord;
		}
		set
		{
			_coord = value;
			CalculateNeighbours();
		}
	}
	public int cost = 1;
	public int fromStart = 0;
	public int toEnd = 0;
	public int score = 0;
	public bool walkable = true;
	public int order = 0;
	public List<Vector3> neighbours = new List<Vector3>();

	Vector3[] directions = { new Vector3(+1, 0, -1), new Vector3(+1, -1, 0), new Vector3(0, -1, +1),
							   new Vector3(-1, 0, +1), new Vector3(-1, +1, 0), new Vector3(0, +1, -1), };

	//public void OnMouseEnter()
	//{
	//	if (walkable)
	//	{
	//		GameObject.Find("World Data").GetComponent<AStar>().CalculatePath(new Vector3(0, 0, 0), coord); 
	//	}
	//}

	void CalculateNeighbours()
	{
		foreach (Vector3 direction in directions)
		{
			neighbours.Add(coord + direction);
		}
	}
}
