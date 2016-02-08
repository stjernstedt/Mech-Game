using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// class that explores first child of every node until it runs out of moves
// then goes to the next child and does the same until no nodes are left
public class DepthFirst : MonoBehaviour
{
	Dictionary<Vector3, Node> hexes;
	public Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();
	public Dictionary<Vector3, int> costSoFar = new Dictionary<Vector3, int>();

	public int maxMoves = 5;

	// Use this for initialization
	void Awake()
	{
		hexes = GameObject.Find("World Data").GetComponent<CreateMap>().hexes;
	}

	public void GetGrid(Vector3 start, int range)
	{
		cameFrom[start] = start;
		costSoFar[start] = 0;
		GoDeeper(start, range);
	}

	void GoDeeper(Vector3 currentCell, int range)
	{
		foreach (Vector3 neighbour in hexes[currentCell].neighbours)
		{
			if (hexes.ContainsKey(neighbour))
			{
				if (hexes[neighbour].walkable)
				{
					int newCost = costSoFar[currentCell] + Mathf.Clamp(Mathf.Abs(hexes[neighbour].cost + 1 - hexes[currentCell].cost), 1, 1000);
					if (!costSoFar.ContainsKey(neighbour) || newCost < costSoFar[neighbour])
					{
						if (newCost <= range)
						{
							cameFrom[neighbour] = currentCell;

							costSoFar[neighbour] = newCost;
							hexes[neighbour].costSoFar = newCost;

							GoDeeper(neighbour, range);
						}
					}
				}
			}
		}

	}

	public void Reset()
	{
		foreach (Vector3 hex in cameFrom.Keys)
		{
			hexes[hex].GetComponent<MeshRenderer>().material.color = Color.white;
		}

		costSoFar.Clear();
		cameFrom.Clear();
	}

	public List<Node> GetPath(Vector3 start, Vector3 dest)
	{
		// TODO add checks if destination is in range
		List<Node> path = new List<Node>();
		path.Add(hexes[dest]);
		Stack<Node> pathStack = new Stack<Node>();
		pathStack.Push(hexes[dest]);


		Vector3 current = dest;
		while (current != start)
		{
			path.Add(hexes[cameFrom[current]]);
			current = cameFrom[current];
		}

		return path;
	}
}
