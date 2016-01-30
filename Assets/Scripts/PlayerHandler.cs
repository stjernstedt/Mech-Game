using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerHandler : MonoBehaviour
{
	Dictionary<Vector3, Node> hexes;
	public GameObject playerPrefab;
	GameObject player;
	GameObject enemy;
	public GameObject selected;
	public Vector3 startingPos = new Vector3(0, 0, 0);
	public Vector3 enemyStartingPos = new Vector3(-2, 11, -9);
	MouseHandler mouseHandler;
	AStar astar;
	DepthFirst depthFirst;

	Vector3 currentCell;
	//List<Node> path;
	Stack<Node> path;
	Node dest;
	Node oldDest;
	public float walkSpeed = 0.5f;
	bool walking = false;
	public bool actionRunning = false;

	int temp = 0;

	public GameObject buttonPrefab;

	public int shortRange = 2;
	public int mediumRange = 4;

	public Color shortColor;
	public Color mediumColor;
	public Color farColor;


	void Awake()
	{
	}

	// Use this for initialization
	void Start()
	{
		mouseHandler = GameObject.Find("World Data").GetComponent<MouseHandler>();
		astar = GameObject.Find("World Data").GetComponent<AStar>();
		depthFirst = GameObject.Find("World Data").GetComponent<DepthFirst>();
		hexes = GameObject.Find("World Data").GetComponent<CreateMap>().hexes;
		float startingHeight = hexes[startingPos].GetComponent<MeshRenderer>().bounds.max.y;
		float enemyStartingHeight = hexes[enemyStartingPos].GetComponent<MeshRenderer>().bounds.max.y;
		player = Instantiate(playerPrefab) as GameObject;
		enemy = Instantiate(playerPrefab) as GameObject;
		player.name = "Player";
		enemy.name = "Enemy";
		player.transform.position = startingPos + new Vector3(0, startingHeight, 0);
		enemy.transform.position = hexes[enemyStartingPos].transform.position + new Vector3(0, enemyStartingHeight, 0);
		SelectUnit(player);

	}

	// Update is called once per frame
	void Update()
	{
		if (selected != null)
		{
			if (Input.GetMouseButtonDown(1))
			{
				if (!walking)
				{
					currentCell = GetCurrentCell().coord;
					path = depthFirst.GetPath(currentCell, mouseHandler.FindCell().coord);
					StartCoroutine(Walk());
				}
			}
			//if (Input.GetMouseButton(1))
			//{
			//	if (!walking)
			//	{
			//		oldDest = dest;
			//		dest = mouseHandler.FindCell();
			//		if (dest != null && !dest.Equals(oldDest))
			//		{
			//			path = astar.CalculatePath(currentCell, dest.coord);
			//		}
			//		for (int i = 0; i < path.Count; i++)
			//		{
			//			if (i <= shortRange)
			//				hexes[path[i].coord].GetComponent<MeshRenderer>().material.color = shortColor;
			//			if (i > shortRange && i <= mediumRange)
			//				hexes[path[i].coord].GetComponent<MeshRenderer>().material.color = mediumColor;
			//			if (i > mediumRange)
			//				hexes[path[i].coord].GetComponent<MeshRenderer>().material.color = farColor;
			//		}
			//	}
			//}

			//if (Input.GetMouseButtonUp(1))
			//{
			//	if (!walking && dest != null)
			//		StartCoroutine(Walk());
			//	astar.resetPath();
			//	temp++;
			//}
		}
	}

	public Node GetCurrentCell()
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

		//for (int i = 0; i < path.Count; i++)
		//{

		//	Node node = path[i];
		//	float cellHeight = hexes[node.coord].GetComponent<MeshRenderer>().bounds.max.y;
		//	selected.transform.position = node.transform.position + new Vector3(0, cellHeight, 0);
		//	yield return new WaitForSeconds(walkSpeed);

		//}
		//while (path.Count > 0 && selected.GetComponent<Mech>().moves > 0)
		//{
		//	Node node = path.Pop();
		//	float cellHeight = hexes[node.coord].GetComponent<MeshRenderer>().bounds.max.y;
		//	selected.transform.position = node.transform.position + new Vector3(0, cellHeight, 0);
		//	yield return new WaitForSeconds(walkSpeed);
		//}
		while (path.Count > 0)
		{
			Node node = path.Pop();
			float cellHeight = hexes[node.coord].GetComponent<MeshRenderer>().bounds.max.y;
			selected.transform.position = node.transform.position + new Vector3(0, cellHeight, 0);
			yield return new WaitForSeconds(walkSpeed);
		}

		depthFirst.Reset();
		depthFirst.GetGrid(GetCurrentCell().coord);
		path.Clear();
		walking = false;
	}

	public void SelectUnit(GameObject unit)
	{
		if (selected != null)
		{
			depthFirst.Reset();
			selected.GetComponent<MeshRenderer>().material.color = Color.white;
		}
		selected = unit;
		selected.GetComponent<MeshRenderer>().material.color = Color.cyan;
		PopulateActionsPanel();
		depthFirst.GetGrid(GetCurrentCell().coord);

	}

	void PopulateActionsPanel()
	{
		GameObject panel = GameObject.Find("ActionsPanel");
		foreach (Transform button in panel.transform)
		{
			Destroy(button.gameObject);
		}

		foreach (Action action in selected.GetComponents<Action>())
		{
			GameObject button = Instantiate(buttonPrefab);
			button.GetComponent<ButtonData>().action = action;
			button.transform.SetParent(panel.transform);
			button.GetComponent<Image>().sprite = action.icon;
			button.GetComponent<Image>().color = action.iconColor;
		}
	}
}