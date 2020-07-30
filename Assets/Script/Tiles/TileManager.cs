using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Runtime.CompilerServices;
using System;

public class TileManager : MonoBehaviour
{
	public static TileManager instance;
	public Tilemap indestructibleTileMap;
	public Tilemap damageTilemap;

	private static Dictionary<Vector3Int, EnvironementTile> tiles = new Dictionary<Vector3Int, EnvironementTile>();
	private static List<EnvironementTile> toxicTiles = new List<EnvironementTile>();




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

	private void Start()
	{
		DisplayIndestructible();
	}

	private void DisplayIndestructible(){
		var bounds = GameManager.tilemap.cellBounds;
		for (int x = bounds.xMin; x < bounds.xMax; x++)
		{
			for (int y = bounds.yMin; y < bounds.yMax; y++)
			{
				var t = GameManager.tilemap.GetTileType(new Vector3Int(x, y, 0));
				if(t && t.indestructible){
					indestructibleTileMap.SetTile(new Vector3Int(x, y, 0),t.indestructibleTile);
				}
			}
		}
	}

	private void Update()
	{
		for(int i = 0; i< toxicTiles.Count; i++){
			toxicTiles[i].UpdateToxicTiles();
		}
	}

	public static EnvironementTile GetOrCreateEnvironementTile(Vector3Int position)
	{
		if (!GameManager.tilemap.GetTileType(position)) return null; 
		if (!tiles.ContainsKey(position))
		{
			TileType t = GameManager.tilemap.GetTileType(position);
			EnvironementTile tile = new EnvironementTile(t, position);
			tiles.Add(position, tile);
			if(t.doDamage){
				toxicTiles.Add(tile);
			}
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
		instance.indestructibleTileMap.SetTile(position, null);
		instance.damageTilemap.SetTile(position, null);
		GameManager.tilemap.SetTile(position, null);
		OnTileDestroy?.Invoke(position, 10);
	}

	public static void TransformTile(Vector3Int position, TileType type){
		instance.indestructibleTileMap.SetTile(position, null);
		instance.damageTilemap.SetTile(position, null);
		GameManager.tilemap.SetTile(position, type);
		if (tiles.ContainsKey(position))
		{
			tiles.Remove(position);
			GetOrCreateEnvironementTile(position);
		}
		
	}
}
