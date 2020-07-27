using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new tile", menuName = "Biocide/Tile", order = 0)]
public class TileType : Tile
{
	public Vector2Int startHp = new Vector2Int(90,110);
	
}
