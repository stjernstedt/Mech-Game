﻿using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	GameObject player;

	// Use this for initialization
	void Start()
	{
		player = GameObject.Find("Player");
	}

	// Update is called once per frame
	void Update()
	{
		if ((Vector2)player.transform.position != (Vector2)Camera.main.transform.position)
		{
			Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.transform.position.z);
			Camera.main.transform.position = pos;
		}
	}
}
