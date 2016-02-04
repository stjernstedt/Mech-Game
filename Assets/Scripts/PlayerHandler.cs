using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerHandler : MonoBehaviour, IProvider
{
	Dictionary<Vector3, Node> hexes;
	public GameObject playerPrefab;
	GameObject player;
	GameObject enemy;
	public List<Mech> units = new List<Mech>();
	public Mech selected;
	public Vector3 startingPos = new Vector3(0, 0, 0);
	public Vector3 enemyStartingPos = new Vector3(-2, 11, -9);
	MouseHandler mouseHandler;
	TurnHandler turnHandler;
	//AStar astar;
	DepthFirst depthFirst;

	Vector3 currentCell;
	List<Node> path;
	//Stack<Node> path;
	Node dest;
	Node oldDest;
	public float walkSpeed = 0.5f;
	bool walking = false;
	public bool actionRunning = false;

	//int temp = 0;

	public GameObject buttonPrefab;

	public int shortRange = 2;
	public int mediumRange = 4;

	public Color shortColor;
	public Color mediumColor;
	public Color farColor;

	IList<IObserver> subscribers = new List<IObserver>();

	void Awake()
	{
		mouseHandler = GameObject.Find("World Data").GetComponent<MouseHandler>();
		turnHandler = GameObject.Find("World Data").GetComponent<TurnHandler>();
		depthFirst = GameObject.Find("World Data").GetComponent<DepthFirst>();
		hexes = GameObject.Find("World Data").GetComponent<CreateMap>().hexes;
	}

	// Use this for initialization
	void Start()
	{
		//astar = GameObject.Find("World Data").GetComponent<AStar>();
		float startingHeight = hexes[startingPos].GetComponent<MeshRenderer>().bounds.max.y;
		float enemyStartingHeight = hexes[enemyStartingPos].GetComponent<MeshRenderer>().bounds.max.y;
		player = Instantiate(playerPrefab) as GameObject;
		enemy = Instantiate(playerPrefab) as GameObject;
		units.Add(player.GetComponent<Mech>());
		units.Add(enemy.GetComponent<Mech>());
		player.name = "Player";
		enemy.name = "Enemy";
		player.transform.position = startingPos + new Vector3(0, startingHeight, 0);
		enemy.transform.position = hexes[enemyStartingPos].transform.position + new Vector3(0, enemyStartingHeight, 0);
		//SelectUnit(player.GetComponent<Mech>());
		turnHandler.NewTurn();
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

		for (int i = path.Count - 1; i >= 0; i--)
		{
			Node node = path[i];
			float cellHeight = hexes[node.coord].GetComponent<MeshRenderer>().bounds.max.y;
			selected.transform.position = node.transform.position + new Vector3(0, cellHeight, 0);
			//if (i > 0)
			//{
			//	selected.GetComponent<Mech>().movesLeft -= path[i].cost % path[i  1].cost + 1;
			//}
			yield return new WaitForSeconds(walkSpeed);
		}

		//while (path.Count > 0)
		//{
		//	Node node = path.Pop();
		//	float cellHeight = hexes[node.coord].GetComponent<MeshRenderer>().bounds.max.y;
		//	selected.transform.position = node.transform.position + new Vector3(0, cellHeight, 0);
		//	yield return new WaitForSeconds(walkSpeed);
		//}
		selected.movesLeft -= depthFirst.costSoFar[path[0].coord];
		depthFirst.Reset();
		depthFirst.GetGrid(GetCurrentCell().coord, selected.movesLeft);
		ColorGrid();
		path.Clear();
		walking = false;
		if (selected.movesLeft < 1)
		{
			NotifySubscribers();
		}
	}

	public void SelectUnit(Mech unit)
	{
		if (selected != null)
		{
			depthFirst.Reset();
			selected.GetComponent<MeshRenderer>().material.color = Color.white;
		}
		selected = unit;
		selected.GetComponent<MeshRenderer>().material.color = Color.cyan;
		PopulateActionsPanel();
		depthFirst.GetGrid(GetCurrentCell().coord, selected.movesLeft);
		ColorGrid();
	}

	void ColorGrid()
	{
		//int range = selected.GetComponent<Mech>().movesLeft;
		int movesLeft = selected.movesLeft;
		int moves = selected.moves;
		foreach (Vector3 hex in depthFirst.costSoFar.Keys)
		{
			int costSoFar = depthFirst.costSoFar[hex];
			if (costSoFar != 0)
			{
				float accuracyLoss = selected.CalculateAccuracyModifier(movesLeft - costSoFar);
				if (accuracyLoss < 0.3)
					hexes[hex].GetComponent<MeshRenderer>().material.color = Color.red;
				if (accuracyLoss >= 0.3 && accuracyLoss < 0.6)
					hexes[hex].GetComponent<MeshRenderer>().material.color = Color.yellow;
				if (accuracyLoss >= 0.6)
					hexes[hex].GetComponent<MeshRenderer>().material.color = Color.green;
			}
			//if (costSoFar <= movesLeft * 0.4)
			//	hexes[hex].GetComponent<MeshRenderer>().material.color = Color.green;
			//if (costSoFar > movesLeft * 0.4 && costSoFar <= movesLeft * 0.8)
			//	hexes[hex].GetComponent<MeshRenderer>().material.color = Color.yellow;
			//if (costSoFar > movesLeft * 0.8)
			//	hexes[hex].GetComponent<MeshRenderer>().material.color = Color.red;
		}
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

	public void Subscribe(IObserver o)
	{
		subscribers.Add(o);
	}

	void NotifySubscribers()
	{
		foreach (IObserver subscriber in subscribers)
		{
			subscriber.UnitOutOfMoves(selected);
		}
	}
}