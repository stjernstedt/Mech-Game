using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mech : MonoBehaviour
{
	public int hp = 30;
	public int armor = 5;
	public int moves = 5;
	public int movesLeft;
	public int heat = 0;
	public int cooling = 10;
	public float baseAccuracy = 100f;
	public float waitBetweenWalk = 0.2f;

	public bool walking = false;

	PlayerHandler playerHandler;
	DepthFirst depthFirst;
	TurnHandler turnHandler;
	Dictionary<Vector3, Node> hexes;

	void Awake()
	{
		EventHandler.NewTurnSubscribers += OnNewTurn;
	}

	// Use this for initialization
	void Start()
	{
		hexes = GameObject.Find("World Data").GetComponent<CreateMap>().hexes;
		playerHandler = GameObject.Find("World Data").GetComponent<PlayerHandler>();
		depthFirst = GameObject.Find("World Data").GetComponent<DepthFirst>();
		turnHandler = GameObject.Find("World Data").GetComponent<TurnHandler>();
	}

	public void Damage(int damage)
	{
		hp -= damage - armor;
		if (hp < 1)
		{
			OnDeath();
		}
	}

	public void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (!playerHandler.actionRunning && !playerHandler.aiUnits.Contains(this))
			{
				playerHandler.SelectUnit(this);
			}
		}
	}

	// returns accuracy based on distance to target and how far the unit has moved
	public float CalculateAccuracy(Vector3 fromHex, Mech target)
	{
		int calculatedMovesLeft = movesLeft - depthFirst.costSoFar[fromHex];
		float movementPenalty = (moves - calculatedMovesLeft) * 2f;
		float distancePenalty = DistanceToTarget(fromHex, target.GetCurrentCell().coord) * 5f;
		float accuracy = baseAccuracy - movementPenalty - distancePenalty;

		//Debug.Log("movepen: " + movementPenalty);
		//Debug.Log("distpen: " + distancePenalty);
		//Debug.Log("acc: " + accuracy);

		return accuracy;
	}

	int DistanceToTarget(Vector3 origin, Vector3 destination)
	{
		int x = (int)Mathf.Abs(destination.x - origin.x);
		int y = (int)Mathf.Abs(destination.y - origin.y);
		int z = (int)Mathf.Abs(destination.z - origin.z);

		return Mathf.Max(x, y, z);
	}

	public bool CanSeeTarget(Mech target)
	{
		Vector3 origin = transform.position;
		origin += new Vector3(0, 0.4f, 0);

		Ray ray = new Ray(origin, target.transform.position + new Vector3(0, 0.4f, 0) - origin);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100f))
		{
			if (hit.collider.GetComponent<Mech>() == target)
			{
				return true;
			}
		}
		return false;
	}

	public RaycastHit RaycastToTarget(Mech target)
	{
		Vector3 origin = transform.position;
		origin += new Vector3(0, 0.4f, 0);

		Ray ray = new Ray(origin, target.transform.position + new Vector3(0, 0.4f, 0) - origin);
		RaycastHit hit;
		Physics.Raycast(ray, out hit, 100f);

		return hit;
	}

	public Node GetCurrentCell()
	{
		Node node = null;
		Ray ray = new Ray(transform.position, new Vector3(0, -1, 0));
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100f))
		{
			node = hit.collider.GetComponent<Node>();
		}

		return node;
	}

	public IEnumerator Walk(List<Node> path)
	{
		walking = true;

		for (int i = path.Count - 1; i >= 0; i--)
		{
			Node node = path[i];
			float cellHeight = hexes[node.coord].GetComponent<MeshRenderer>().bounds.max.y;
			transform.position = node.transform.position + new Vector3(0, cellHeight, 0);
			yield return new WaitForSeconds(waitBetweenWalk);
		}

		movesLeft -= depthFirst.costSoFar[path[0].coord];
		depthFirst.Reset();
		depthFirst.GetGrid(GetCurrentCell().coord, movesLeft);
		playerHandler.ColorGrid();
		path.Clear();
		walking = false;
		if (movesLeft < 1)
		{
			//EventHandler.OutOfMoves(selected);
		}
		if (turnHandler.AIRunning)
		{
			EventHandler.AIDone();
			turnHandler.AIRunning = false;
		}
	}

	void OnNewTurn()
	{
		movesLeft = moves;
	}

	void OnDeath()
	{
		EventHandler.UnitDeath(this);
		Destroy(gameObject);
	}

	void DissipateHeat()
	{
		heat -= cooling;
		Mathf.Clamp(heat, 0, 100);
	}
}