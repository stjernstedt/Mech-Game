using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerHandler : MonoBehaviour
{
	Dictionary<Vector3, Node> hexes;
	public GameObject playerPrefab;
	public GameObject AIPrefab;
	GameObject player;
	GameObject enemy;
	public List<Mech> units = new List<Mech>();
	public List<Mech> aiUnits = new List<Mech>();
	public Mech selected;
	public Vector3 startingPos = new Vector3(0, 0, 0);
	public Vector3 enemyStartingPos = new Vector3(-2, 11, -9);
	MouseHandler mouseHandler;
	TurnHandler turnHandler;
	DepthFirst depthFirst;

	Vector3 currentCell;
	List<Node> path;
	Node dest;
	Node oldDest;
	bool walking = false;
	public Action actionRunning = null;
	public bool selectingTarget = false;

	public GameObject buttonPrefab;

	public int shortRange = 2;
	public int mediumRange = 4;

	public Color shortColor;
	public Color mediumColor;
	public Color farColor;

	public Image heatGauge;

	void Awake()
	{
		turnHandler = GetComponent<TurnHandler>();
		depthFirst = GetComponent<DepthFirst>();
		mouseHandler = GetComponent<MouseHandler>();
		hexes = GetComponent<CreateMap>().hexes;
	}

	// Use this for initialization
	void Start()
	{
		float startingHeight = hexes[startingPos].GetComponent<MeshRenderer>().bounds.max.y + 0.01f;
		float enemyStartingHeight = hexes[enemyStartingPos].GetComponent<MeshRenderer>().bounds.max.y;
		player = Instantiate(playerPrefab) as GameObject;
		enemy = Instantiate(AIPrefab) as GameObject;
		units.Add(player.GetComponent<Mech>());
		aiUnits.Add(enemy.GetComponent<Mech>());
		player.name = "Player";
		enemy.name = "Enemy";
		player.transform.position = startingPos + new Vector3(0, startingHeight, 0);
		enemy.transform.position = hexes[enemyStartingPos].transform.position + new Vector3(0, enemyStartingHeight, 0);
		turnHandler.NewTurn();
		EventHandler.UnitDeathSubscribers += OnUnitDeath;
		EventHandler.ActionTakenSubscribers += OnActionTaken;
	}

	// Update is called once per frame
	void Update()
	{
		if (selected != null)
		{
			if (Input.GetMouseButtonDown(1))
			{
				if (!selected.walking)
				{
					currentCell = selected.GetCurrentCell().coord;
					path = depthFirst.GetPath(currentCell, mouseHandler.FindCell().coord);
					StartCoroutine(selected.Walk(path));
				}
			}
		}
	}

	// selects a unit and displays it's movement grid
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
		depthFirst.GetGrid(selected.GetCurrentCell().coord, selected.movesLeft);
		ColorGrid();
		UpdateUI();
	}

	public void ColorGrid()
	{
		int movesLeft = selected.movesLeft;
		foreach (Vector3 hex in depthFirst.costSoFar.Keys)
		{
			int costSoFar = depthFirst.costSoFar[hex];
			if (costSoFar != 0)
			{
				hexes[hex].GetComponent<MeshRenderer>().material.color = Color.cyan;
				//float accuracy = selected.CalculateAccuracy(movesLeft - costSoFar, null);
				//if (accuracy < 0.3)
				//	hexes[hex].GetComponent<MeshRenderer>().material.color = Color.red;
				//if (accuracy >= 0.3 && accuracy < 0.6)
				//	hexes[hex].GetComponent<MeshRenderer>().material.color = Color.yellow;
				//if (accuracy >= 0.6)
				//	hexes[hex].GetComponent<MeshRenderer>().material.color = Color.green;
			}
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

	void OnUnitDeath(Mech unit)
	{
		units.Remove(unit);
		if (units.Count < 2)
		{
			EventHandler.EndGame();
		}
	}

	void OnActionTaken(Mech unit)
	{
		UpdateUI();
	}

	void UpdateUI()
	{
		heatGauge.fillAmount = selected.heat / 100f;
	}
}