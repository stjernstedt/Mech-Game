using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TurnHandler : MonoBehaviour
{
	public Text turnText;
	List<Mech> notFinishedUnits = new List<Mech>();
	PlayerHandler playerHandler;
	int turnNumber = 0;

	void Awake()
	{
		playerHandler = GameObject.Find("World Data").GetComponent<PlayerHandler>();
	}

	// Use this for initialization
	void Start()
	{
		EventHandler.OutOfMovesSubscribers += OnUnitOutOfMoves;
		EventHandler.EndOfGameSubscribers += OnEndOfGame;
	}

	public void NewTurn()
	{
		turnNumber += 1;
		EventHandler.EndTurn();

		turnText.text = "Turn " + turnNumber;
		turnText.gameObject.SetActive(true);

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

	void OnUnitOutOfMoves(Mech unit)
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

	void OnEndOfGame()
	{
		Debug.Log("Game Over!");
	}
}