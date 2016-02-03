using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnHandler : MonoBehaviour, IObserver
{
	List<Mech> notFinishedUnits = new List<Mech>();
	//List<Mech> finishedUnits = new List<Mech>();
	PlayerHandler playerHandler;

	void Awake()
	{
		playerHandler = GameObject.Find("World Data").GetComponent<PlayerHandler>();
	}
	
	// Use this for initialization
	void Start()
	{
		playerHandler.Subscribe(this);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void NewTurn()
	{
		foreach (Mech unit in playerHandler.units)
		{
			unit.movesLeft = unit.moves;
			notFinishedUnits.Add(unit);
		}
		playerHandler.SelectUnit(notFinishedUnits[0]);
		Debug.Log("new turn");
	}

	public void UnitOutOfMoves(Mech unit)
	{
		//finishedUnits.Add(unit);
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
