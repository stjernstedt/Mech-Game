using UnityEngine;
using UnityEngine.UI;
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

	public void Execute()
	{
		if (!playerHandler.actionRunning)
			action.Execute();
	}
}