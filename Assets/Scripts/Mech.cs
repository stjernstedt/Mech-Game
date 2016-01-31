using UnityEngine;
using System.Collections;

public class Mech : MonoBehaviour
{
	public int hp = 30;
	public int armor = 5;
	public int moves = 5;
	public int movesLeft;

	PlayerHandler playerHandler;
	Action[] actions;

	// Use this for initialization
	void Start()
	{
		playerHandler = GameObject.Find("World Data").GetComponent<PlayerHandler>();
		actions = GetComponents<Action>();
		movesLeft = moves;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Damage(int damage)
	{
		hp -= damage - armor;
	}

	public void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (!playerHandler.actionRunning)
			{
				playerHandler.SelectUnit(gameObject);
			}
		}
	}
}
