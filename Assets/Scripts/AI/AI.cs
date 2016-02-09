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

	Dictionary<Vector3, Node> hexes;
	List<Vector3> candidates = new List<Vector3>();
	Mech target;

	public float tolerableAccuracy = 60f;

	// Use this for initialization
	void Start()
	{
		depthFirst = GameObject.Find("World Data").GetComponent<DepthFirst>();
		playerHandler = GameObject.Find("World Data").GetComponent<PlayerHandler>();
		turnHandler = GameObject.Find("World Data").GetComponent<TurnHandler>();
		hexes = GameObject.Find("World Data").GetComponent<CreateMap>().hexes;
		target = playerHandler.units[0];
	}

	public void RunAI()
	{
		turnHandler.AIRunning = true;
		playerHandler.SelectUnit(playerHandler.aiUnits[0]);
		GetCandidates();
		int canditateNumber = Random.Range(0, candidates.Count - 1);
		List<Node> path = depthFirst.GetPath(playerHandler.selected.GetCurrentCell().coord, candidates[canditateNumber]);
		Debug.Log("candidates: " + candidates.Count);
		Debug.Log("candidate number: " + canditateNumber);
		Debug.Log("path count: " + path.Count);
		StartCoroutine(playerHandler.selected.Walk(path));
		candidates.Clear();
	}

	void GetCandidates()
	{
		depthFirst.GetGrid(playerHandler.selected.GetCurrentCell().coord, playerHandler.selected.movesLeft);

		float currentCellAccuracy = playerHandler.selected.CalculateAccuracy(playerHandler.selected.GetCurrentCell().coord, target);

		// if current cell accuracy is good enough, tries to fire
		if (currentCellAccuracy >= tolerableAccuracy)
		{
			candidates.Add(playerHandler.selected.GetCurrentCell().coord);
			return;
		}

		// checks all walkable cells if they have better accuracy than current cell, then adds them as candidates
		foreach (var hex in depthFirst.costSoFar)
		{
			int costSoFar = hex.Value;
			float checkAccuracy = playerHandler.selected.CalculateAccuracy(hex.Key, target);
			if (checkAccuracy >= currentCellAccuracy)
			{
				candidates.Add(hex.Key);
			}
		}
	}
}
