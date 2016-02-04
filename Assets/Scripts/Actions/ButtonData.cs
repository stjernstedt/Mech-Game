using UnityEngine;
using System.Collections;

public class ButtonData : MonoBehaviour
{
	public Action action;
	public PlayerHandler playerHandler;

	// Use this for initialization
	void Start()
	{
		playerHandler = GameObject.FindObjectOfType<PlayerHandler>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Execute()
	{
		if (!playerHandler.actionRunning)
			action.Execute();
	}
}
