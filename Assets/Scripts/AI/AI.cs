using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour
{
	public enum Objective
	{
		Attack,
		Flee
	}

	DepthFirst depthFirst;
	PlayerHandler playerHandler;
	TurnHandler turnHandler;

	List<Vector3> candidates = new List<Vector3>();
	Mech target;

	public float tolerableAccuracy = 60f;

	// Use this for initialization
	void Start()
	{
		depthFirst = GameObject.Find("World Data").GetComponent<DepthFirst>();
		playerHandler = GameObject.Find("World Data").GetComponent<PlayerHandler>();
		turnHandler = GameObject.Find("World Data").GetComponent<TurnHandler>();
		target = playerHandler.units[0];
	}

	public void RunAI()
	{
		turnHandler.AIRunning = true;
		playerHandler.SelectUnit(playerHandler.aiUnits[0]);
		GetCandidates();
		int canditateNumber = Random.Range(0, candidates.Count - 1);
		List<Node> path = depthFirst.GetPath(playerHandler.selected.GetCurrentCell().coord, candidates[canditateNumber]);
		StartCoroutine(playerHandler.selected.Walk(path));
		candidates.Clear();
	}

	void GetCandidates()
	{
		depthFirst.GetGrid(playerHandler.selected.GetCurrentCell().coord, playerHandler.selected.movesLeft);

		float currentCellAccuracy = playerHandler.selected.CalculateAccuracy(playerHandler.selected.GetCurrentCell().coord, target);

		// tries to fire if current cell accuracy is good enough and can see target
		if (currentCellAccuracy >= tolerableAccuracy && GetComponent<Mech>().CanSeeTarget(target))
		{
			Attack();
			candidates.Add(playerHandler.selected.GetCurrentCell().coord);
			return;
		}

		// checks all walkable cells if they have better accuracy than current cell, then adds them as candidates
		foreach (var hex in depthFirst.costSoFar)
		{
			float checkAccuracy = playerHandler.selected.CalculateAccuracy(hex.Key, target);
			if (checkAccuracy >= currentCellAccuracy)
			{
				candidates.Add(hex.Key);
			}
		}
	}

	void Attack()
	{
		Action[] actions = GetComponents<Action>();
		int chooseAction = Random.Range(0, actions.Length);
		actions[chooseAction].target = target;
		StartCoroutine(actions[chooseAction].Fire());
	}
}
