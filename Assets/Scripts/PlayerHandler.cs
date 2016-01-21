using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHandler : MonoBehaviour
{
	Dictionary<Vector3, Node> hexes;
	public GameObject playerPrefab;
	GameObject player;
	public Vector3 startingPos = new Vector3(0, 0, 0);
	CreateMap map;
	MouseHandler mouseHandler;
	AStar astar;

	Vector3 currentCell;
	Stack<Node> path;
	Node dest;
	Node oldDest;
	public float walkSpeed = 0.5f;
	bool walking = false;

	int temp = 0;

	// Use this for initialization
	void Start()
	{
		map = GameObject.Find("World Data").GetComponent<CreateMap>();
		mouseHandler = GameObject.Find("World Data").GetComponent<MouseHandler>();
		astar = GameObject.Find("World Data").GetComponent<AStar>();
		hexes = GameObject.Find("World Data").GetComponent<CreateMap>().hexes;
		float startingHeight = hexes[startingPos].GetComponent<MeshRenderer>().bounds.max.y;
		player = Instantiate(playerPrefab) as GameObject;
		player.transform.position = startingPos + new Vector3(0, startingHeight, 0);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			if (!walking)
				currentCell = GetCurrentCell().coord;
		}
		if (Input.GetMouseButton(1))
		{
			if (!walking)
			{
				oldDest = dest;
				dest = mouseHandler.FindCell();
				if (dest != null && !dest.Equals(oldDest))
				{
					path = astar.CalculatePath(currentCell, dest.coord);
				}
			}
		}

		if (Input.GetMouseButtonUp(1))
		{
			if (!walking && dest != null)
				StartCoroutine(Walk());
			astar.resetPath();
			temp++;

		}
	}

	Node GetCurrentCell()
	{
		Node node = null;
		Ray ray = new Ray(player.transform.position, -player.transform.up);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100f))
		{
			node = hit.collider.GetComponent<Node>();
		}

		return node;
	}

	IEnumerator Walk()
	{
		walking = true;
		while (path.Count > 0)
		{
			Node node = path.Pop();
			float cellHeight = hexes[node.coord].GetComponent<MeshRenderer>().bounds.max.y;
			player.transform.position = node.transform.position + new Vector3(0, cellHeight, 0);
			yield return new WaitForSeconds(walkSpeed);
		}
		path.Clear();
		walking = false;
	}
}