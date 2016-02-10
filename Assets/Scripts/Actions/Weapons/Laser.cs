using UnityEngine;
using System.Collections;

public class Laser : Action
{
	public float timeVisible = 0.4f;

	public override IEnumerator Fire()
	{
		float timePassed = 0;
		Vector3 origin = transform.position;
		origin += new Vector3(0, 0.4f, 0);

		bool didHit = false;
		RaycastHit hit = unit.RaycastToTarget(target);
		if (CalculateHit())
		{
			lineRenderer.SetColors(iconColor, iconColor);
			lineRenderer.SetVertexCount(2);
			lineRenderer.SetPosition(0, origin);
			lineRenderer.SetPosition(1, hit.point);
			lineRenderer.enabled = true;
			didHit = true;
		}

		while (timeVisible > timePassed)
		{
			timePassed += Time.deltaTime;
			yield return null;
		}
		lineRenderer.enabled = false;

		// BUG trying to get mech component even if hitting terrain
		if (didHit)
		{
			target.Damage(damage);
		}

		base.Fire();
	}


}
