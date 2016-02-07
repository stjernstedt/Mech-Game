using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TurnHandler : MonoBehaviour
{
	public Text turnText;
	List<Mech> notFinishedUnits = new List<Mech>();
	List<Team> notFinishedTeams = new List<Team>();
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

		turnText.text = "Turn " + turnNumber;
		turnText.gameObject.SetActive(true);

		foreach (Team team in playerHandler.teams)
		{
			notFinishedTeams.Add(team);
		}
		playerHandler.currentTeam = notFinishedTeams[0];
		playerHandler.SelectUnit(playerHandler.currentTeam.units[0]);
		//foreach (Mech unit in playerHandler.units)
		//{
		//	if (unit.movesLeft > 0)
		//	{
		//		notFinishedUnits.Add(unit);
		//	}
		//}
		//playerHandler.SelectUnit(notFinishedUnits[0]);
		Debug.Log("new turn");
	}

	public void EndTurn()
	{
		if (playerHandler.currentTeam != null)
		{
			notFinishedTeams.Remove(playerHandler.currentTeam);
		}

		if (notFinishedTeams.Count < 1)
		{
			EventHandler.EndTurn();
			NewTurn();
		}
		else
		{
			playerHandler.currentTeam = notFinishedTeams[0];
			playerHandler.SelectUnit(playerHandler.currentTeam.units[0]);
		}
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
		turnText.text = "GAME OVER";
		turnText.gameObject.SetActive(true);
		Debug.Log("Game Over!");
	}
}