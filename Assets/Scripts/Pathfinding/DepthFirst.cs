using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// class that explores first child of every node until it runs out of moves
// then goes to the next child and does the same until no nodes are left
public class DepthFirst : MonoBehaviour
{
	Dictionary<Vector3, Node> hexes;
	Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();
	Dictionary<Vector3, int> costSoFar = new Dictionary<Vector3, int>();

	Vector3 start = new Vector3(0, 0, 0);

	int moved = 0;
	public int maxMoves = 5;

	// Use this for initialization
	void Awake()
	{
		hexes = GameObject.Find("World Data").GetComponent<CreateMap>().hexes;
	}

	public void GetGrid(Vector3 start)
	{
		cameFrom[start] = start;
		costSoFar[start] = 0;
		moved = 0;
		GoDeeper(start);
	}

	void GoDeeper(Vector3 currentCell)
	{
		moved += 1;
		foreach (Vector3 neighbour in hexes[currentCell].neighbours)
		{
			if (hexes.ContainsKey(neighbour))
			{
				if (hexes[neighbour].walkable)
				{
					int newCost = costSoFar[currentCell] + hexes[neighbour].cost;
					if (!costSoFar.ContainsKey(neighbour) || newCost < costSoFar[neighbour])
					{
						cameFrom[neighbour] = currentCell;

						costSoFar[neighbour] = newCost;
						if (moved <= maxMoves)
						{
							GoDeeper(neighbour);
						}
						ColorHex(neighbour);
					}
				}
			}
		}
		moved -= 1;

	}

	void ColorHex(Vector3 hex)
	{
		if (costSoFar[hex] <= 2)
			hexes[hex].GetComponent<MeshRenderer>().material.color = Color.green;
		if (costSoFar[hex] > 2 && costSoFar[hex] <= 4)
			hexes[hex].GetComponent<MeshRenderer>().material.color = Color.yellow;
		if (costSoFar[hex] > 4 && costSoFar[hex] <= 5)
			hexes[hex].GetComponent<MeshRenderer>().material.color = Color.red;
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

	public Stack<Node> GetPath(Vector3 start, Vector3 dest)
	{
		// TODO add checks if destination is in range

		Stack<Node> path = new Stack<Node>();
		path.Push(hexes[dest]);

		Vector3 current = dest;
		while (current != start)
		{
			path.Push(hexes[cameFrom[current]]);
			current = cameFrom[current];
		}

		return path;
	}
}
