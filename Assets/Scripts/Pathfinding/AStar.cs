using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour
{

	PriorityQueue<Vector3> frontier = new PriorityQueue<Vector3>();
	Dictionary<Vector3, Node> hexes;
	Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();
	Dictionary<Vector3, int> costSoFar = new Dictionary<Vector3, int>();

	List<Node> previousPath = new List<Node>();

	public Color pathColor = new Color(0.5f, 0.5f, 1);

	// Use this for initialization
	void Start()
	{
		hexes = GameObject.Find("World Data").GetComponent<CreateMap>().hexes;
	}

	public List<Node> CalculatePath(Vector3 start, Vector3 dest)
	{
		// resets color on previous marked cells
		resetPath();

		// adds starting cell to queue
		frontier.Enqueue(start, 0);
		cameFrom[start] = start;
		costSoFar[start] = 0;

		bool done = false;
		while (frontier.Count != 0)
		{
			if (done)
				break;
			// picks the next cell in the priorityqueue and gets its neighbours
			Vector3 current = frontier.Dequeue();
			List<Vector3> neighbours = hexes[current].GetComponent<Node>().neighbours;
			foreach (Vector3 neighbour in neighbours)
			{

				if (hexes.ContainsKey(neighbour))
				{

					if (hexes[neighbour].walkable)
					{
						// calculates cost of walked path so far and adds next cells cost to it
						int newCost = costSoFar[current] + hexes[neighbour].cost;
						// checks if cell is already in the path and if it is checks if the new path is cheaper
						if (!costSoFar.ContainsKey(neighbour) || newCost < costSoFar[neighbour])
						{
							costSoFar[neighbour] = newCost;
							// calculates the priority of the cell and adds it to the priorityqueue
							int priority = newCost + EstimateDistance(neighbour, dest);
							frontier.Enqueue(neighbour, priority);
							cameFrom[neighbour] = current;
							//hexes[neighbour].GetComponent<MeshRenderer>().material.color = Color.cyan;
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

		Vector3 loc = dest;

		// path to return, as stack since cells are added from destination to start
		Stack<Node> pathTemp = new Stack<Node>();

		hexes[loc].GetComponent<MeshRenderer>().material.color = pathColor;
		pathTemp.Push(hexes[loc]);
		previousPath.Add(hexes[loc]);

		do
		{
			loc = cameFrom[loc];
			hexes[loc].GetComponent<MeshRenderer>().material.color = pathColor;
			pathTemp.Push(hexes[loc]);
			previousPath.Add(hexes[loc]);

		} while (loc != start);

		List<Node> path = new List<Node>();
		while (pathTemp.Count > 0)
		{
			path.Add(pathTemp.Pop());
		}

		return path;
	}

	public void resetPath()
	{
		frontier.Clear();
		cameFrom.Clear();
		costSoFar.Clear();

		if (previousPath.Count > 0)
		{
			foreach (Node node in previousPath)
			{
				node.GetComponent<MeshRenderer>().material.color = Color.white;
			}
			previousPath.Clear();
		}
	}

	int EstimateDistance(Vector3 origin, Vector3 destination)
	{
		int x = (int)Mathf.Abs(destination.x - origin.x);
		int y = (int)Mathf.Abs(destination.y - origin.y);
		int z = (int)Mathf.Abs(destination.z - origin.z);

		return Mathf.Max(x, y, z);
	}

}

// queue class that orders items after their priority
public class PriorityQueue<T>
{
	List<KeyValuePair<T, int>> elements = new List<KeyValuePair<T, int>>();

	public int Count
	{
		get { return elements.Count; }
	}

	public void Enqueue(T item, int priority)
	{
		elements.Add(new KeyValuePair<T, int>(item, priority));
	}

	public T Dequeue()
	{
		int bestIndex = 0;

		for (int i = 0; i < elements.Count; i++)
		{
			if (elements[i].Value < elements[bestIndex].Value)
			{
				bestIndex = i;
			}
		}

		T bestItem = elements[bestIndex].Key;
		elements.RemoveAt(bestIndex);
		return bestItem;
	}

	public void Clear()
	{
		elements.Clear();
	}
}