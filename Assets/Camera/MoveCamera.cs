using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
	public float speed = 10;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			Vector3 direction = -transform.right;
			direction.y = 0;
			direction.Normalize();
			Camera.main.transform.Translate(direction * Time.deltaTime * speed, Space.World);
		}
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			Vector3 direction = transform.right;
			direction.y = 0;
			direction.Normalize();
			Camera.main.transform.Translate(direction * Time.deltaTime * speed, Space.World);
		}
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
		{
			Vector3 direction = transform.forward;
			direction.y = 0;
			direction.Normalize();
			Camera.main.transform.Translate(direction * Time.deltaTime * speed, Space.World);
		}
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
		{
			Vector3 direction = -transform.forward;
			direction.y = 0;
			direction.Normalize();
			Camera.main.transform.Translate(direction * Time.deltaTime * speed, Space.World);
		}

		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			Camera.main.transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime * 6));
		}

		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			Camera.main.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime * 6));
		}
	}
}
