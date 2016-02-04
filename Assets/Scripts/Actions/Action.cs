using UnityEngine;
using System.Collections;

public abstract class Action : MonoBehaviour
{
	protected GameObject target;

	public int damage = 10;

	public Sprite icon;
	public Color iconColor;

	public bool running = false;

	protected PlayerHandler playerHandler;
	protected LineRenderer lineRenderer;
	protected Mech unit;

	// Use this for initialization
	public virtual void Start()
	{
		playerHandler = GameObject.Find("World Data").GetComponent<PlayerHandler>();
		lineRenderer = GameObject.Find("World Data").GetComponent<LineRenderer>();
		unit = GetComponent<Mech>();
	}

	// Update is called once per frame
	void Update()
	{
		if (running)
		{
			if (Input.GetMouseButtonDown(0))
			{
				lineRenderer.material.color = Color.green;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100f))
				{
					if (hit.collider.GetComponent<Mech>() != null)
					{
						target = hit.collider.gameObject;
					}
				}
				StartCoroutine(Fire());
			}
		}
	}

	public void Execute()
	{
		running = true;
		playerHandler.actionRunning = true;
	}

	public abstract IEnumerator Fire();

	public bool CalculateHit()
	{
		float diceRoll = Random.Range(0f, 1f);
		float accuracyModifier = unit.CalculateAccuracyModifier(unit.movesLeft);

		if (diceRoll < 1f * accuracyModifier)
		{
			return true;
		}
		return false;
	}
}
