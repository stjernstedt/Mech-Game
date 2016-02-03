using UnityEngine;
using System.Collections;

public interface IProvider
{
	void Subscribe(IObserver observer);
}
