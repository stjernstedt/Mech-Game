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
	public bool AIRunning = false;

	void Awake()
	{
		playerHandler = GetComponent<PlayerHandler>();
	}

	// Use this for initialization
	void Start()
	{
		EventHandler.OutOfMovesSubscribers += OnUnitOutOfMoves;
		EventHandler.EndGameSubscribers += OnEndOfGame;
		EventHandler.AIDoneSubscribers += NewTurn;
	}

	public void NewTurn()
	{
		turnNumber += 1;
		EventHandler.NewTurn();

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

	public void EndTurn()
	{
		GetComponent<AI>().RunAI();
		// waits for callback from AI
		//NewTurn();
	}

	void OnUnitOutOfMoves(Mech unit)
	{
		notFinishedUnits.Remove(unit);
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