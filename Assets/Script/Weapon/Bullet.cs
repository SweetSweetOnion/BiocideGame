﻿using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
	public float lifetime = 50;
	public float speed = 10;
	public float gravity = 10;
	public Vector2 direction = new Vector2(1, 0);

	private Rigidbody2D rb;
	private bool applyForce = true;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if(applyForce){
			rb.AddForce( direction * speed, ForceMode2D.Impulse);
			applyForce = false;
		}

		rb.AddForce(Vector2.down * gravity * Time.deltaTime);

		lifetime -= Time.deltaTime;
		if(lifetime < 0){
			Destroy(this.gameObject);
		}
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		lifetime = 0;
		/*var tile = collision.gameObject.GetComponent<Tile>();
		if(tile){
			Debug.Log("yo");
		}*/
		Vector3 hitPosition = Vector3.zero;
		if (GameManager.tilemap != null && GameManager.tilemap.gameObject == collision.gameObject)
		{
			foreach (ContactPoint2D hit in collision.contacts)
			{
				hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
				hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
				
				Vector3Int coord = GameManager.tilemap.WorldToCell(hitPosition);
				
				TileType t = GameManager.tilemap.GetEnvironementTile(coord);
				if(t){
					TileManager.OnTileHit(coord);
				}
			}
		}

	}
}
