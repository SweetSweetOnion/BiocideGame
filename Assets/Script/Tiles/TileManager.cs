using UnityEngine;
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
	public static Tilemap scriptTilemap => instance._scriptTilemap;

	public TileType defautTiletype;
	public Color backgroundTileColor;


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
	[SerializeField]
	private Tilemap _scriptTilemap;



	private static Dictionary<Vector3Int, EnvironementTile> tiles = new Dictionary<Vector3Int, EnvironementTile>();
	private static List<EnvironementTile> toxicTiles = new List<EnvironementTile>();




	public delegate void TileEvent(Vector3Int position);
	public delegate void TileEventExperience(Vector3Int position, float experience);
	public delegate void CompleteTileEvent(Vector3Int position, EnvironementTile envTile);
	public static event CompleteTileEvent OnTileDamage;
	public static event TileEventExperience OnTileDestroy;
	public delegate void TileHit(Vector3 bulletHitPoint, Vector3Int cellCoordinates, bool receiveDamage, float normHp);
	public static event TileHit OnTileHitByBullet;

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
		var bounds = _scriptTilemap.cellBounds;
		for (int x = bounds.xMin; x < bounds.xMax; x++)
		{
			for (int y = bounds.yMin; y < bounds.yMax; y++)
			{
				var pos = new Vector3Int(x, y, 0);
				var t = GetTileType(pos);
				if (t )
				{
					if(t.indestructible){
						resistanceTilemap.SetTile(pos, t.indestructibleTile);
					}else{
						if(t.resistanceLevel == 1){
							resistanceTilemap.SetTile(pos, t.resistanceLevelSprites[0]);
						}else 
						if(t.resistanceLevel > 1)
							resistanceTilemap.SetTile(pos, t.resistanceLevelSprites[1]);
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

	private static TileType GetTileType(Vector3Int pos){
		if (!mainTilemap.HasTile(pos)) return null;
		var t = scriptTilemap.GetTileType(pos);
		if (!t) return instance.defautTiletype;
		return t;
	}
	
	public static EnvironementTile GetOrCreateEnvironementTile(Vector3Int position)
	{
		if (!GetTileType(position)) return null;
		if (!tiles.ContainsKey(position))
		{
			TileType t = GetTileType(position);
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
		TileManager.GetOrCreateEnvironementTile(position).GetNormHp();
		bool b = false;
		//Debug.DrawRay(mainTilemap.GetCellCenterWorld(position), Vector3.up * 10, Color.red,1) ;
		var t = GetOrCreateEnvironementTile(position);
		if (t == null) return;
		if (!t.CanResist(bullet))
		{
			t.ReceiveDamage(bullet.damage);
			OnTileDamage?.Invoke(position, t);
			b = true;
		}
		OnTileHitByBullet?.Invoke(bullet.transform.position, position, b,t.GetNormHp());
        TileAudioManager.instance.PostTileHitSound(position, t.GetNormHp(), b);

	}

	public static void OnTilesWalkedOn(Vector3Int[] positions, PlayerController player)
	{
		foreach (Vector3Int pos in positions)
		{
			var t = GetOrCreateEnvironementTile(pos);
			if (t != null){
				t.WalkByPlayer(player);
			}
				
		}

	}

	public static void RemoveTile(Vector3Int position)
	{
		backgroundTilemap.SetTile(position, GetTileType(position).backgroundTile);

		if (tiles.ContainsKey(position))
		{
			tiles.Remove(position);
		}

		/*mainTilemap.SetTileFlags(position, TileFlags.None);
		mainTilemap.SetColliderType(position, Tile.ColliderType.None);
		mainTilemap.SetColor(position, instance.backgroundTileColor);*/
		
		resistanceTilemap.SetTile(position, null);
		damageTilemap.SetTile(position, null);
		mainTilemap.SetTile(position, null);
		scriptTilemap.SetTile(position, null);
		OnTileDestroy?.Invoke(position, 10);
        TileAudioManager.instance.PostTileDestroySound(position);
    }

	public static void TransformTile(Vector3Int position, TileType type)
	{
		resistanceTilemap.SetTile(position, null);
		damageTilemap.SetTile(position, type.toxicTile);
		scriptTilemap.SetTile(position, type);
		if (tiles.ContainsKey(position))
		{
			tiles.Remove(position);
			GetOrCreateEnvironementTile(position);
		}
		//mainTilemap.SetTile(position, type.toxicTile);
	}

}
