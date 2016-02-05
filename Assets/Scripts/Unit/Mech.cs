using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mech : MonoBehaviour
{
	public int hp = 30;
	public int armor = 5;
	public int moves = 5;
	public int movesLeft;

	PlayerHandler playerHandler;

	void Awake()
	{
		EventHandler.EndTurnSubscribers += OnNewTurn;
	}

	// Use this for initialization
	void Start()
	{
		playerHandler = GameObject.Find("World Data").GetComponent<PlayerHandler>();
	}

	public void Damage(int damage)
	{
		hp -= damage - armor;
		if (hp < 1)
		{
			OnDeath();
		}
	}

	public void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (!playerHandler.actionRunning)
			{
				playerHandler.SelectUnit(this);
			}
		}
	}

	public float CalculateAccuracyModifier(int movesLeft)
	{
		return (float)movesLeft / (float)moves;
	}

	void OnNewTurn()
	{
		Debug.Log("unit reset");
		movesLeft = moves;
	}

	void OnDeath()
	{
		Destroy(gameObject);
	}
}