using UnityEngine;
using System.Collections;

public class SameColorAsParent : MonoBehaviour
{
	private SpriteRenderer rend;
	private SpriteRenderer parent;

	private void Awake()
	{
		parent = transform.parent.GetComponent<SpriteRenderer>();
		rend = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		rend.color = parent.color;
	}
}
