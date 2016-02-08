using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// *** Dijsktra's algorithm ***
// calculates best path depending on cost of cells and expands more towards cells where cost is lower

public class Dijkstra : MonoBehaviour
{
	PriorityQueue<Vector3> frontier = new PriorityQueue<Vector3>();
	Dictionary<Vector3, Node> hexes;
	Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();
	Dictionary<Vector3, int> costSoFar = new Dictionary<Vector3, int>();

	Vector3 start = new Vector3(0, 0, 0);
	Vector3 dest = new Vector3(-1, 4, -3);

	PlayerHandler playerHandler;

	// Use this for initialization
	void Start()
	{
		hexes = GameObject.Find("World Data").GetComponent<CreateMap>().hexes;
		playerHandler = GameObject.Find("World Data").GetComponent<PlayerHandler>();
		//frontier.Enqueue(start, 0);
		//cameFrom[start] = start;
		//costSoFar[start] = 0;
		//StartCoroutine(ExploreFrontier());
	}

	public void GetGrid()
	{
		start = playerHandler.selected.GetCurrentCell().coord;
		frontier.Enqueue(start, 0);
		cameFrom[start] = start;
		costSoFar[start] = 0;

		bool done = false;
		while (frontier.Count != 0)
		{
			if (done)
				break;
			Vector3 current = frontier.Dequeue();
			List<Vector3> neighbours = hexes[current].GetComponent<Node>().neighbours;
			foreach (Vector3 neighbour in neighbours)
			{
				if (hexes.ContainsKey(neighbour))
				{
					int newCost = costSoFar[current] + hexes[neighbour].cost;
					if ((!costSoFar.ContainsKey(neighbour) || newCost < costSoFar[neighbour]) && newCost < playerHandler.selected.GetComponent<Mech>().moves)
					{
						costSoFar[neighbour] = newCost;
						int priority = newCost;
						frontier.Enqueue(neighbour, priority);
						cameFrom[neighbour] = current;
						hexes[neighbour].GetComponent<MeshRenderer>().material.color = Color.cyan;
						if (neighbour == dest)
						{
							done = true;
							break;
						}
					}
				}
			}
		}
	}

	IEnumerator ExploreFrontier()
	{
		bool done = false;
		while (frontier.Count != 0)
		{
			if (done)
				break;
			Vector3 current = frontier.Dequeue();
			List<Vector3> neighbours = hexes[current].GetComponent<Node>().neighbours;
			foreach (Vector3 neighbour in neighbours)
			{
				if (hexes.ContainsKey(neighbour))
				{
					int newCost = costSoFar[current] + hexes[neighbour].cost;
					if (!costSoFar.ContainsKey(neighbour) || newCost < costSoFar[neighbour])
					{
						costSoFar[neighbour] = newCost;
						int priority = newCost;
						frontier.Enqueue(neighbour, priority);
						cameFrom[neighbour] = current;
						hexes[neighbour].GetComponent<MeshRenderer>().material.color = Color.cyan;
						if (neighbour == dest)
						{
							done = true;
							break;
						}
						yield return new WaitForSeconds(0.2f);
					}
				}
			}
		}

		Vector3 loc = dest;
		hexes[loc].GetComponent<MeshRenderer>().material.color = Color.blue;

		while (loc != start)
		{
			Debug.Log(costSoFar[loc]);
			loc = cameFrom[loc];
			hexes[loc].GetComponent<MeshRenderer>().material.color = Color.blue;
		}

		Debug.Log("end");
	}
}