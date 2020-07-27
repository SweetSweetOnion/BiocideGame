using UnityEngine;
using System.Collections;

public class EnvironementTile 
{
	public TileType tile;
	public Vector3Int position;
	public int hp = 100;

	public EnvironementTile(TileType t, Vector3Int pos)
	{
		tile = t;
		position = pos;
		hp = Random.Range(t.startHp.x,t.startHp.y);
	}

	public void DealDamage(int damageAmount){
		hp -= damageAmount;
		if(hp <= 0){
			TileManager.RemoveTile(position);
		}
	}

}
