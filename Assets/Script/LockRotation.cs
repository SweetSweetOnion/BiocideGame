﻿using UnityEngine;
using System.Collections;

public class LockRotation : MonoBehaviour
{
	private void LateUpdate()
	{
		transform.rotation = Quaternion.identity;
	}
}
