using UnityEngine;
using System.Collections;

using UnityEngine.Tilemaps;

public static class TilemapExtension
{

	public static TileType GetTileType(this Tilemap tileMap, Vector3Int position){
		var t = tileMap.GetTile(position) as TileType;
		return t;
	}

}
