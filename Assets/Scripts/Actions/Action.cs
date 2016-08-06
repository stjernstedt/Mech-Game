using UnityEngine;
using System.Collections;

public abstract class Action : MonoBehaviour
{
	public Mech target;

	public int damage = 10;
	public int heatGenerated = 10;
	public int cooldown = 1;
	public int cooldownTimer = 0;

	public Sprite icon;
	public Color iconColor;

	protected PlayerHandler playerHandler;
	protected LineRenderer lineRenderer;
	protected Mech unit;

	// Use this for initialization
	public virtual void Start()
	{
		playerHandler = GameObject.Find("World Data").GetComponent<PlayerHandler>();
		lineRenderer = GameObject.FindObjectOfType<LineRenderer>();
		unit = GetComponent<Mech>();
		EventHandler.NewTurnSubscribers += TickCooldown;
	}

	// Update is called once per frame
	void Update()
	{
		if (playerHandler.selectingTarget && playerHandler.actionRunning == this && target == null)
		{
			if (Input.GetMouseButtonDown(0))
			{
				playerHandler.actionRunning = this;
				lineRenderer.material.color = Color.green;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100f))
				{
					if (hit.collider.GetComponent<Mech>() != null)
					{
						target = hit.collider.GetComponent<Mech>();
					}
				}
				if (target != null)
				{
					if (unit.CanSeeTarget(target))
					{
						StartCoroutine(Fire());
						playerHandler.selectingTarget = false;
					}
					else
					{
						// TODO move to reset function?
						Debug.Log("can't see target!");
						playerHandler.actionRunning = null;
						target = null;
						cooldownTimer = 0;
					}
				}
			}
		}
	}

	public void Execute()
	{
		if (cooldownTimer == 0)
		{
			cooldownTimer = cooldown;
			playerHandler.selectingTarget = true;
			playerHandler.actionRunning = this;
		}
		else
		{
			Debug.Log("on cooldown!");
		}
	}

	public virtual IEnumerator Fire()
	{
		Debug.Log("fired");
		playerHandler.actionRunning = null;
		target = null;
		EventHandler.ActionTaken(this);
		return null;
	}

	public bool CalculateHit()
	{
		float diceRoll = Random.Range(0f, 1f);
		float accuracy = unit.CalculateAccuracy(unit.GetCurrentCell().coord, target);

		if (diceRoll < accuracy)
		{
			GenerateHeat();
			return true;
		}
		return false;
	}

	public void GenerateHeat()
	{
		unit.heat += heatGenerated;
	}

	void TickCooldown()
	{
		if (cooldownTimer > 0)
		{
			cooldownTimer -= 1;
		}
	}
}