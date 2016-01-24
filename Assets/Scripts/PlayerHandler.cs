using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHandler : MonoBehaviour
{
	Dictionary<Vector3, Node> hexes;
	public GameObject playerPrefab;
	GameObject player;
	GameObject enemy;
	GameObject selected;
	public Vector3 startingPos = new Vector3(0, 0, 0);
	public Vector3 enemyStartingPos = new Vector3(-2, 11, -9);
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
		mouseHandler = GameObject.Find("World Data").GetComponent<MouseHandler>();
		astar = GameObject.Find("World Data").GetComponent<AStar>();
		hexes = GameObject.Find("World Data").GetComponent<CreateMap>().hexes;
		float startingHeight = hexes[startingPos].GetComponent<MeshRenderer>().bounds.max.y;
		float enemyStartingHeight = hexes[enemyStartingPos].GetComponent<MeshRenderer>().bounds.max.y;
		player = Instantiate(playerPrefab) as GameObject;
		enemy = Instantiate(playerPrefab) as GameObject;
		player.name = "Player";
		enemy.name = "Enemy";
		player.transform.position = startingPos + new Vector3(0, startingHeight, 0);
		enemy.transform.position = hexes[enemyStartingPos].transform.position + new Vector3(0, enemyStartingHeight, 0);
	}

	// Update is called once per frame
	void Update()
	{
		if (selected != null)
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
	}

	Node GetCurrentCell()
	{
		Node node = null;
		Ray ray = new Ray(selected.transform.position, new Vector3(0, -1, 0));
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
			selected.transform.position = node.transform.position + new Vector3(0, cellHeight, 0);
			yield return new WaitForSeconds(walkSpeed);
		}
		path.Clear();
		walking = false;
	}

	public void Attack()
	{
		StartCoroutine(Fire());
	}

	public IEnumerator Fire()
	{
		LineRenderer lineRenderer = GetComponent<LineRenderer>();

		float timeVisible = 0.2f;
		float timePassed = 0;
		//float playerHeight = player.GetComponent<MeshRenderer>().bounds.max.y;
		//Vector3 origin = player.transform.FindChild("Canon").transform.position + new Vector3(0, playerHeight, 0);
		Vector3 origin = player.transform.position;
		Vector3 target = enemy.transform.position;
		origin += new Vector3(0, 0.4f, 0);
		target += new Vector3(0, 0.4f, 0);
		Debug.Log(target);

		Ray ray = new Ray(origin, target - origin);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100f))
		{
			lineRenderer.SetVertexCount(2);
			lineRenderer.SetPosition(0, origin);
			//lineRenderer.SetPosition(1, target);
			lineRenderer.SetPosition(1, hit.point);
			lineRenderer.enabled = true;
			Debug.Log("fire");
		}
		else
		{
			Debug.Log("miss");
		}
		Debug.Log(hit.collider.name);

		while (timeVisible > timePassed)
		{
			timePassed += Time.deltaTime;
			yield return null;
		}
		lineRenderer.enabled = false;

	}

	public void SelectUnit(GameObject unit)
	{
		if (selected != null)
		{
			selected.GetComponent<MeshRenderer>().material.color = Color.white;
		}
		selected = unit;
		selected.GetComponent<MeshRenderer>().material.color = Color.cyan;
	}
}