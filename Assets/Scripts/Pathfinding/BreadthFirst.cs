using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ** breadth first early exit **
// expands a frontier by adding cells to a queue and check every neighbour
// then exits if it has reached its destination
// all cells also saves its parents to be able to get a path from start to finish
public class BreadthFirst : MonoBehaviour
{
	Vector3[] directions = { new Vector3(+1, 0, -1), new Vector3(+1, -1, 0), new Vector3(0, -1, +1),
							   new Vector3(-1, 0, +1), new Vector3(-1, +1, 0), new Vector3(0, +1, -1), };

	Queue<Vector3> frontier = new Queue<Vector3>();
	Dictionary<Vector3, Node> hexes;
	List<Vector3> visited = new List<Vector3>();
	Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();

	Vector3 start = new Vector3(0, 0, 0);
	Vector3 dest = new Vector3(-1, 4, -3);

	// Use this for initialization
	void Start()
	{
		hexes = GameObject.Find("World Data").GetComponent<CreateMap>().hexes;
		frontier.Enqueue(start);
		StartCoroutine(ExploreFrontier());
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	IEnumerator ExploreFrontier()
	{
		bool done = false;
		while (frontier.Count != 0)
		{
			if (done)
				break;
			Vector3 current = frontier.Dequeue();
			foreach (Vector3 neighbour in directions)
			{
				if (hexes.ContainsKey(current + neighbour))
				{
					if (!cameFrom.ContainsKey(current + neighbour))
					{
						frontier.Enqueue(current + neighbour);
						cameFrom.Add(current + neighbour, current);
						hexes[current + neighbour].GetComponent<MeshRenderer>().material.color = Color.cyan;
						if (current + neighbour == dest)
						{
							done = true;
							break;
						}
						yield return new WaitForSeconds(0.02f);
					}
				}
			}
		}

		Vector3 loc = dest;
		hexes[loc].GetComponent<MeshRenderer>().material.color = Color.blue;

		while (loc != start)
		{
			loc = cameFrom[loc];
			hexes[loc].GetComponent<MeshRenderer>().material.color = Color.blue;
		}

		Debug.Log(hexes.Count);
		Debug.Log(visited.Count);
		Debug.Log("lololol");
		Debug.Log("end");
	}
}
