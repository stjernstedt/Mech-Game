using UnityEngine;
using System.Collections;

public class Laser : Action
{
	GameObject target;

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

	public override void Execute()
	{
		running = true;
		playerHandler.actionRunning = true;
	}

	public IEnumerator Fire()
	{
		float timeVisible = 0.9f;
		float timePassed = 0;
		Vector3 origin = transform.position;
		origin += new Vector3(0, 0.4f, 0);

		Ray ray = new Ray(origin, target.transform.position + new Vector3(0, 0.4f, 0) - origin);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100f))
		{
			lineRenderer.SetVertexCount(2);
			lineRenderer.SetPosition(0, origin);
			lineRenderer.SetPosition(1, hit.point);
			lineRenderer.enabled = true;
		}

		while (timeVisible > timePassed)
		{
			timePassed += Time.deltaTime;
			yield return null;
		}
		lineRenderer.enabled = false;
		running = false;
		playerHandler.actionRunning = false;
	}


}
