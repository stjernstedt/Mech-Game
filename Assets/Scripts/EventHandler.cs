using UnityEngine;
using System.Collections;

public class EventHandler : MonoBehaviour
{
	public delegate void NewTurnDelegate();
	public static event NewTurnDelegate NewTurnSubscribers;

	public delegate void EndGameDelegate();
	public static event EndGameDelegate EndGameSubscribers;

	public delegate void OutOfMovesDelegate(Mech unit);
	public static event OutOfMovesDelegate OutOfMovesSubscribers;

	public delegate void UnitDeathDelegate(Mech unit);
	public static event UnitDeathDelegate UnitDeathSubscribers;

	public delegate void ActionTakenDelegate(Mech unit);
	public static event ActionTakenDelegate ActionTakenSubscribers;

	public delegate void AIDoneDelegate();
	public static event AIDoneDelegate AIDoneSubscribers;

	public static void NewTurn()
	{
		NewTurnSubscribers();
	}

	public static void EndGame()
	{
		EndGameSubscribers();
	}

	public static void OutOfMoves(Mech unit)
	{
		OutOfMovesSubscribers(unit);
	}

	public static void UnitDeath(Mech unit)
	{
		UnitDeathSubscribers(unit);
	}

	public static void ActionTaken(Mech unit)
	{
		ActionTakenSubscribers(unit);
	}

	public static void AIDone()
	{
		AIDoneSubscribers();
	}
}