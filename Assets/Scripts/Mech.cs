using UnityEngine;
using System.Collections;

public class Mech : MonoBehaviour
{
	public int hp = 30;
	public int armor = 5;
	public int moves = 5;
	public int movesLeft;
	public float accuracyLoss = 0;

	PlayerHandler playerHandler;
	Action[] actions;

	void Awake()
	{
		movesLeft = moves;
	}

	// Use this for initialization
	void Start()
	{
		playerHandler = GameObject.Find("World Data").GetComponent<PlayerHandler>();
		actions = GetComponents<Action>();
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
				playerHandler.SelectUnit(this);
			}
		}
	}


	// TODO implement way to calculate accuracy loss on a per-hex basis
	public float CalculateAccuracyLoss(int movesLeft)
	{
		//accuracyLoss = (float)movesLeft / (float)moves;

		float tempAccuracyloss = movesLeft / (float)moves;
		return tempAccuracyloss;
	}
}
