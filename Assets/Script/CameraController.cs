﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{

	public PlayerController player;
	private Camera cam;
	public Vector2 speed = new Vector2(100, 5);
	public float backgroundSize = 120;
	public Transform[] parralax;
	public float[] parralaxMultiplier;
	//private float[] movementX;

	private Vector3 targetPos;
	private Vector3 lastCamPos;


	private Vector2 shakeDuration;
	public float maxDuration = 10;
	public float maxShake = 0;
	public AnimationCurve shakeAmountOverTime;

	private void OnEnable()
	{
		player.OnTeleport += Player_onTeleport;
		TileManager.OnTileDamage += OnTileDamage;
		TileManager.OnTileDestroy += OnTileDestroy;
		player.OnLanded += OnLanded;
	}

	private void OnLanded(float fallDuration)
	{
		AddShake(0, fallDuration *2);
	}

	private void OnTileDestroy(Vector3Int position, float experience)
	{
		AddShake(0.1f);
	}

	private void OnTileDamage(Vector3Int position, EnvironementTile envTile)
	{
		AddShake(0.01f);
	}

	private void OnDisable()
	{
		player.OnTeleport -= Player_onTeleport;
		TileManager.OnTileDamage -= OnTileDamage;
		TileManager.OnTileDestroy -= OnTileDestroy;
	}


	private void Player_onTeleport(Vector2 old, Vector2 newPos)
	{
		Vector2 pos = targetPos;
		Vector2 dir = pos - old;
		Vector2 dir2 = transform.position - targetPos;
		targetPos = newPos + dir;
		transform.position = new Vector3(targetPos.x + dir2.x, targetPos.y + dir2.y, transform.position.z);

		lastCamPos = transform.position;
		for (int i = 0; i < parralax.Length; i++)
		{
			parralax[i].position += new Vector3(-backgroundSize, 0, 0);
		}
	}

	// Use this for initialization
	void Start()
	{
		SetCamToTarget();
		//movementX = new float[parralax.Length];
	}

	// Update is called once per frame
	void LateUpdate()
	{
		targetPos.z = transform.position.z;

		if (player.transform.position.x > targetPos.x) targetPos.x = player.transform.position.x;

		if (player.isGrounded)
		{
			targetPos.y = player.transform.position.y;
		}

		Vector3 v = transform.position;
		v.x = Mathf.MoveTowards(v.x, targetPos.x, Time.deltaTime * speed.x);
		v.y = Mathf.MoveTowards(v.y, targetPos.y, Time.deltaTime * speed.y);
		transform.position = v;
		//transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed)
		UpdateShake();
		UpdateParralax();
	}

	private void UpdateParralax()
	{
		Vector3 deltaMovement = transform.position - lastCamPos;
		deltaMovement.y = 0;
		deltaMovement.z = 0;
		for (int i = 0; i < parralax.Length; i++)
		{
			parralax[i].position += deltaMovement * parralaxMultiplier[i];
			/*movementX[i] += deltaMovement.x * parralaxMultiplier[i];
			if (movementX[i] > backgroundSize)
			{
				parralax[i].position += new Vector3(-backgroundSize, 0, 0);
			}*/
			float off = parralax[i].position.x - transform.position.x;
			if (Mathf.Abs(off) > backgroundSize / 2)
			{
				parralax[i].position -= new Vector3(Mathf.Sign(off) * backgroundSize, 0, 0);
			}
		}

		lastCamPos = transform.position;

	}

	private void UpdateShake()
	{
		if(shakeDuration.x > 0 || shakeDuration.y >0)
		{
			float quakeX = shakeAmountOverTime.Evaluate(shakeDuration.x/maxDuration) * maxShake;
			float quakeY = shakeAmountOverTime.Evaluate(shakeDuration.y / maxDuration) * maxShake;
			Vector3 shakeDir = new Vector3(Random.Range(-1f, 1f) * quakeX, Random.Range(-1f, 1f), 0) * quakeY;
			transform.position += shakeDir ;
			shakeDuration.x -= Time.deltaTime;
			shakeDuration.y -= Time.deltaTime;
		}
		else{
			shakeDuration = Vector2.zero;
		}
	}

	private void SetCamToTarget()
	{
		targetPos = player.transform.position;
	}

	public void AddShake(float duration)
	{
		shakeDuration += new Vector2(duration,duration);
		shakeDuration = Vector2.ClampMagnitude(shakeDuration, maxDuration);
	}
	public void AddShake(float dx, float dY)
	{
		shakeDuration += new Vector2(dx, dY);
		shakeDuration.x = Mathf.Clamp(shakeDuration.x, 0, maxDuration);
		shakeDuration.y = Mathf.Clamp(shakeDuration.y, 0, maxDuration);

	}
}
