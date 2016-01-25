using UnityEngine;
using System.Collections;

public class ButtonData : MonoBehaviour
{
	public Action action;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Execute()
	{
		action.Execute();
	}
}
