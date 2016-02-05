using UnityEngine;
using System.Collections;

public class EventHandler : MonoBehaviour
{
	public delegate void EndOfTurn();
	public static event EndOfTurn EndTurnSubscribers;

	public delegate void OutOfMoves(Mech unit);
	public static event OutOfMoves OutOfMovesSubscribers;

	public static void EndTurn()
	{
		EndTurnSubscribers();
	}

	public static void UnitOutOfMoves(Mech unit)
	{
		OutOfMovesSubscribers(unit);
	}
}