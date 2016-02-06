﻿using UnityEngine;
using System.Collections;

public abstract class Action : MonoBehaviour
{
	protected GameObject target;

	public int damage = 10;
	public int heatGenerated = 10;

	public Sprite icon;
	public Color iconColor;

	protected PlayerHandler playerHandler;
	protected LineRenderer lineRenderer;
	protected Mech unit;

	// Use this for initialization
	public virtual void Start()
	{
		playerHandler = GameObject.FindObjectOfType<PlayerHandler>();
		lineRenderer = GameObject.FindObjectOfType<LineRenderer>();
		unit = GetComponent<Mech>();
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
						target = hit.collider.gameObject;
					}
				}
				if (target != null)
				{
					StartCoroutine(Fire());
					playerHandler.selectingTarget = false;
				}
			}
		}
	}

	public void Execute()
	{
		playerHandler.selectingTarget = true;
		playerHandler.actionRunning = this;
	}

	public virtual IEnumerator Fire()
	{
		playerHandler.actionRunning = null;
		target = null;
		EventHandler.TakeAction(unit);
		return null;
	}

	public bool CalculateHit()
	{
		float diceRoll = Random.Range(0f, 1f);
		float accuracyModifier = unit.CalculateAccuracyModifier(unit.movesLeft);

		if (diceRoll < 1f * accuracyModifier)
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
}
