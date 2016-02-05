using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnHandler : MonoBehaviour
{
	List<Mech> notFinishedUnits = new List<Mech>();
	PlayerHandler playerHandler;

	void Awake()
	{
		playerHandler = GameObject.Find("World Data").GetComponent<PlayerHandler>();
	}

	// Use this for initialization
	void Start()
	{
		EventHandler.OutOfMovesSubscribers += UnitOutOfMoves;
	}

	public void NewTurn()
	{
		EventHandler.EndTurn();

		foreach (Mech unit in playerHandler.units)
		{
			if (unit.movesLeft > 0)
			{
				notFinishedUnits.Add(unit);
			}
		}
		playerHandler.SelectUnit(notFinishedUnits[0]);
		Debug.Log("new turn");
	}

	public void UnitOutOfMoves(Mech unit)
	{
		notFinishedUnits.Remove(unit);
		Debug.Log("unit finished");
		if (notFinishedUnits.Count > 0)
		{
			playerHandler.SelectUnit(notFinishedUnits[0]);
		}
		else
		{
			NewTurn();
		}
	}
}