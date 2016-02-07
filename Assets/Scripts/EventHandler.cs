using UnityEngine;
using System.Collections;

public class EventHandler : MonoBehaviour
{
	public delegate void EndOfTurn();
	public static event EndOfTurn EndOfTurnSubscribers;

	public delegate void EndOfGame();
	public static event EndOfGame EndOfGameSubscribers;

	public delegate void OutOfMoves(Mech unit);
	public static event OutOfMoves OutOfMovesSubscribers;

	public delegate void DeathOfUnit(Mech unit);
	public static event DeathOfUnit DeathOfUnitSubscribers;

	public delegate void ActionTaken(Mech unit);
	public static event ActionTaken ActionTakenSubscribers;

	public delegate void TeamTurnEnded(Team team);
	public static event TeamTurnEnded TeamTurnEndedSubscribers;

	public static void EndTurn()
	{
		EndOfTurnSubscribers();
	}

	public static void EndGame()
	{
		EndOfGameSubscribers();
	}

	public static void UnitOutOfMoves(Mech unit)
	{
		OutOfMovesSubscribers(unit);
	}

	public static void UnitDeath(Mech unit)
	{
		DeathOfUnitSubscribers(unit);
	}

	public static void TakeAction(Mech unit)
	{
		ActionTakenSubscribers(unit);
	}

	public static void TeamEndTurn(Team team)
	{
		TeamTurnEndedSubscribers(team);
	}
}