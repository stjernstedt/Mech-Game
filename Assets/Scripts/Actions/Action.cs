using UnityEngine;
using System.Collections;

public abstract class Action : MonoBehaviour
{
	public Sprite icon;
	public Color iconColor;

	public bool running = false;

	protected PlayerHandler playerHandler;
	protected LineRenderer lineRenderer;

	// Use this for initialization
	public virtual void Start()
	{
		playerHandler = GameObject.Find("World Data").GetComponent<PlayerHandler>();
		lineRenderer = GameObject.Find("World Data").GetComponent<LineRenderer>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public abstract void Execute();
}
