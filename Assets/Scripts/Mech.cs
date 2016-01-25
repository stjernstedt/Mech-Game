using UnityEngine;
using System.Collections;

public class Mech : MonoBehaviour
{
	int hp = 30;
	int armor = 5;

	PlayerHandler playerHandler;
	Action[] actions;

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
				playerHandler.SelectUnit(gameObject);
			}
		}
	}
}
