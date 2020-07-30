using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Runtime.CompilerServices;
using System;

public class TileManager : MonoBehaviour
{
	public static TileManager instance;

	private static Dictionary<Vector3Int, EnvironementTile> tiles = new Dictionary<Vector3Int, EnvironementTile>();


	private Dictionary<Vector3Int, Coroutine> flashDictionary = new Dictionary<Vector3Int, Coroutine>();


	public delegate void TileEvent(Vector3Int position);
	public delegate void TileEventExperience(Vector3Int position, float experience);
	public delegate void CompleteTileEvent(Vector3Int position, EnvironementTile envTile);
	public static event CompleteTileEvent OnTileDamage;
	public static event TileEventExperience OnTileDestroy;

	private void Awake()
	{
		if (instance)
		{
			Destroy(this); return;
		}
		instance = this;
	}

	private void Update()
	{
		
	}

	public static EnvironementTile GetOrCreateEnvironementTile(Vector3Int position)
	{
		if (!GameManager.tilemap.GetTileType(position)) return null; 
		if (!tiles.ContainsKey(position))
		{
			EnvironementTile tile = new EnvironementTile(GameManager.tilemap.GetTileType(position), position);
			tiles.Add(position, tile);
		}
		return tiles[position];
	}

	public static void OnTileHit(Vector3Int position, Bullet bullet)
	{
		var t = GetOrCreateEnvironementTile(position);
		if (!t.CanResist(bullet))
		{
			t.ReceiveDamage(bullet.damage);
			OnTileDamage?.Invoke(position, t);
		}
	}

	public static void OnTilesWalkedOn(Vector3Int[] positions, PlayerController player)
	{
		foreach(Vector3Int pos in positions){
			var t = GetOrCreateEnvironementTile(pos);
			if (t != null)
				t.WalkByPlayer(player);
		}

	}

	public static void RemoveTile(Vector3Int position)
	{
		if (tiles.ContainsKey(position))
		{
			tiles.Remove(position);
		}
		GameManager.tilemap.SetTile(position, null);
		OnTileDestroy?.Invoke(position, 10);
	}

	public static void TransformTile(Vector3Int position, TileType type){
		GameManager.tilemap.SetTile(position, type);
		if (tiles.ContainsKey(position))
		{
			tiles.Remove(position);
			GetOrCreateEnvironementTile(position);
		}
		
	}
}
