using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseHandler : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public Node FindCell()
	{
		Node node = null;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100f))
		{
			node = hit.collider.GetComponent<Node>();
		}

		if (node == null || !node.walkable) node = null;
		return node;
	}
}