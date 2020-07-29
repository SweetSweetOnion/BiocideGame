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

	public static void OnTileHit(Vector3Int position, Bullet bullet){
		
		if(!tiles.ContainsKey(position)){
			EnvironementTile tile = new EnvironementTile(GameManager.tilemap.GetTileType(position), position);
			tiles.Add(position, tile);
		}	
		tiles[position].Flash();
		if(!tiles[position].CanResist(bullet)){
			tiles[position].DealDamage(bullet.damage);
			OnTileDamage?.Invoke(position,tiles[position]);
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
}
