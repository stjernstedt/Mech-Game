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
		Debug.Log("candidates: " + candidates.Count);
		int canditateNumber = Random.Range(0, candidates.Count - 1);
		Debug.Log("candidate number: " + canditateNumber);
		List<Node> path = depthFirst.GetPath(playerHandler.selected.GetCurrentCell().coord, candidates[canditateNumber]);
		Debug.Log("path count: " + path.Count);
		StartCoroutine(playerHandler.selected.Walk(path));
		candidates.Clear();
	}

	void GetCandidates()
	{
		depthFirst.GetGrid(playerHandler.selected.GetCurrentCell().coord, playerHandler.selected.movesLeft);

		if (playerHandler.selected.CalculateAccuracy(playerHandler.selected.movesLeft, target) >= tolerableAccuracy)
		{
			candidates.Add(playerHandler.selected.GetCurrentCell().coord);
			Debug.Log("can fire!");
			return;
		}

		foreach (var hex in depthFirst.costSoFar)
		{
			int costSoFar = hex.Value;
			if (playerHandler.selected.CalculateAccuracy(playerHandler.selected.movesLeft - costSoFar, target) >= tolerableAccuracy)
			{
				candidates.Add(hex.Key);
			}
		}
		// TODO change mode from fire to move if no candidate is within tolerable accuracy
	}
}
