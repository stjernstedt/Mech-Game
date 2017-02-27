using UnityEngine;
using System.Collections;

public class Cannon : Action
{
	public ParticleSystem smokeTrailPrefab;
	public float speed;

	public override IEnumerator Fire()
	{
		ParticleSystem smokeTrail = Instantiate(smokeTrailPrefab);
		Vector3 origin = transform.position;
		origin += new Vector3(0, 0.4f, 0);
		float startTime = Time.time;
		float pathLength = Vector3.Distance(origin, target.transform.position + new Vector3(0, 0.4f, 0));

		bool didHit = false;
		if (CalculateHit())
		{
			smokeTrail.transform.position = origin;
			didHit = true;
		}

		float randomY = Random.Range(0, 0.2f);
		Vector3 randomV3 = new Vector3(0, randomY, 0);

		while (!smokeTrail.transform.position.Equals(target.transform.position + new Vector3(0, 0.4f, 0) + randomV3))
		{
			float distCovered = (Time.time - startTime) * speed;
			float fracPath = distCovered / pathLength;
			smokeTrail.transform.position = Vector3.Lerp(origin, target.transform.position + new Vector3(0, 0.4f, 0) + randomV3, fracPath);
			yield return null;
		}
        //smokeTrail.enableEmission = false;
        smokeTrail.Stop();
		while (smokeTrail.IsAlive())
		{
			yield return null;
		}
		Destroy(smokeTrail.gameObject);

		// BUG trying to get mech component even if hitting terrain
		if (didHit)
		{
			target.Damage(damage);
		}

		base.Fire();
	}
}
