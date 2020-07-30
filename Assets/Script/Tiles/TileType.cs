using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new tile", menuName = "Biocide/Tile", order = 0)]
public class TileType : Tile
{
	[Header("Custom")]
	public Vector2Int startHp = new Vector2Int(90,110);
	public int resistanceLevel = 0;
	public bool doDamage = false;
	public float damagePerTick = 10;
	public float tickCooldDown = 0.5f;
	public bool indestructible = false;
	public TileType transformTo = null;

	public TileBase indestructibleTile;
	public TileBase[] resistanceLevelSprites;
	public TileBase[] damageLevelSprites;
	public TileBase vfxDamageTile;

}
