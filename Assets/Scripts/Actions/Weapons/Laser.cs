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

		Debug.Log(target);
		Debug.Log(target.transform.position);
		Ray ray = new Ray(origin, target.transform.position + new Vector3(0, 0.4f, 0) - origin);
		RaycastHit hit;
		bool didHit = false;
		if (Physics.Raycast(ray, out hit, 100f))
		{
			if (CalculateHit())
			{
				lineRenderer.SetColors(iconColor, iconColor);
				lineRenderer.SetVertexCount(2);
				lineRenderer.SetPosition(0, origin);
				lineRenderer.SetPosition(1, hit.point);
				lineRenderer.enabled = true;
				didHit = true;
			}
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
			hit.collider.GetComponent<Mech>().Damage(damage);
		}

		base.Fire();
	}


}
