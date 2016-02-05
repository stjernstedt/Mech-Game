using UnityEngine;
using System.Collections;

public interface IObserver
{
	void UnitOutOfMoves(Mech unit);

	void Notify<T>();
}
