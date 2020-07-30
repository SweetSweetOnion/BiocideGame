﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System;
using DG.Tweening.Core.Easing;

public class TileManager : MonoBehaviour
{
	public static TileManager instance;
	public static Tilemap mainTilemap => instance._mainTilemap;
	public static Tilemap resistanceTilemap => instance._resistanceTilemap;
	public static Tilemap damageTilemap => instance._damageTilemap;
	public static Tilemap vfxTile => instance._vfxTile;
	public static Tilemap backgroundTilemap => instance._backgroundTilemap;

	[SerializeField]
	private Tilemap _mainTilemap;
	[SerializeField]
	private Tilemap _resistanceTilemap;
	[SerializeField]
	private Tilemap _damageTilemap;
	[SerializeField]
	private Tilemap _vfxTile;
	[SerializeField]
	private Tilemap _backgroundTilemap;



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
		DisplayResistance();
	}

	private void DisplayResistance()
	{
		var bounds = mainTilemap.cellBounds;
		for (int x = bounds.xMin; x < bounds.xMax; x++)
		{
			for (int y = bounds.yMin; y < bounds.yMax; y++)
			{
				var pos = new Vector3Int(x, y, 0);
				var t = mainTilemap.GetTileType(pos);
				if (t )
				{
					if(t.indestructible){
						resistanceTilemap.SetTile(pos, t.indestructibleTile);
					}else{
						if(t.resistanceLevel> 0 && t.resistanceLevel < t.resistanceLevelSprites.Length)
							resistanceTilemap.SetTile(pos, t.resistanceLevelSprites[t.resistanceLevel-1]);
					}
				}
			}
		}
	}

	private void Update()
	{
		for (int i = 0; i < toxicTiles.Count; i++)
		{
			toxicTiles[i].UpdateToxicTiles();
		}
	}

	public static EnvironementTile GetOrCreateEnvironementTile(Vector3Int position)
	{
		if (!mainTilemap.GetTileType(position)) return null;
		if (!tiles.ContainsKey(position))
		{
			TileType t = mainTilemap.GetTileType(position);
			EnvironementTile tile = new EnvironementTile(t, position);
			tiles.Add(position, tile);
			if (t.doDamage)
			{
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
		foreach (Vector3Int pos in positions)
		{
			var t = GetOrCreateEnvironementTile(pos);
			if (t != null)
				t.WalkByPlayer(player);
		}

	}

	public static void RemoveTile(Vector3Int position)
	{
		backgroundTilemap.SetTile(position, mainTilemap.GetTileType(position).backgroundTile);

		if (tiles.ContainsKey(position))
		{
			tiles.Remove(position);
		}
		
		resistanceTilemap.SetTile(position, null);
		damageTilemap.SetTile(position, null);
		mainTilemap.SetTile(position, null);
		OnTileDestroy?.Invoke(position, 10);
	}

	public static void TransformTile(Vector3Int position, TileType type)
	{
		resistanceTilemap.SetTile(position, null);
		damageTilemap.SetTile(position, null);
		mainTilemap.SetTile(position, type);
		if (tiles.ContainsKey(position))
		{
			tiles.Remove(position);
			GetOrCreateEnvironementTile(position);
		}
	}

}
