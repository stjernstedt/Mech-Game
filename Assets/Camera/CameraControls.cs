using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour
{
	Camera cam;
	Vector3 oldPos;

	// Use this for initialization
	void Start()
	{
		cam = Camera.main;
		//cam.transform.position = new Vector3(WorldData.worldData.width / 2, WorldData.worldData.height / 2, -10);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			Vector3 mousePos = Input.mousePosition;
			oldPos = cam.ScreenToWorldPoint(mousePos);
		}

		if (Input.GetMouseButton(1))
		{
			Vector3 mousePos = Input.mousePosition;
			Vector3 pos = cam.ScreenToWorldPoint(mousePos);
			Vector3 deltaPos = new Vector3(oldPos.x - pos.x, oldPos.y - pos.y, 0);
			cam.transform.position += deltaPos;
		}

		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			Camera.main.orthographicSize++;
		}

		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			Camera.main.orthographicSize--;
		}
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 5, 20);
	}
}
