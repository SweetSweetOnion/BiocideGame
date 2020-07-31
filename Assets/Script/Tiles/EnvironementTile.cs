using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Tilemaps;

public class EnvironementTile
{
	private TileType _tile;
	private Vector3Int _position;
	private float _hp;
	private Color _startColor;
	private bool _doDamage;
	private float _tickTime;
	//accesors
	public Color color => _startColor;
	private int flashCount = 0;

	public EnvironementTile(TileType t, Vector3Int pos)
	{
		_tile = t;
		_position = pos;
		_hp = Random.Range(t.startHp.x, t.startHp.y);
		_startColor = TileManager.mainTilemap.GetColor(pos);
		_doDamage = t.doDamage;
		_tickTime = Random.Range(0, _tile.tickCooldDown);
		if (_tile.indestructible && !_tile.doDamage)
			TileManager.resistanceTilemap.SetTile(pos, _tile.indestructibleTile);
	}

	public void UpdateToxicTiles(){
		_tickTime += Time.deltaTime;
		if(_doDamage && _tickTime > _tile.tickCooldDown){
			DoToxic(Vector3Int.left);
			DoToxic(Vector3Int.right);
			DoToxic(Vector3Int.up);
			DoToxic(Vector3Int.down);
			_tickTime = 0;
		}
	}

	private void DoToxic(Vector3Int offset){
		var other = TileManager.GetOrCreateEnvironementTile(_position + offset);
		if (other == null) return;
		if (other._tile.indestructible == false &&_tile.resistanceLevel >= other._tile.resistanceLevel){
			other.ReceiveDamage(_tile.damagePerTick);
		}
	}

	public void ReceiveDamage(float damageAmount)
	{
		
		_hp -= damageAmount;
		
		if (_hp <= 0)
		{
			if(_tile.transformTo){
				TileManager.TransformTile(_position, _tile.transformTo);
			}else{
				TileManager.RemoveTile(_position);
			}
		}
		else{
			Flash();
			float normDamage = _tile.startHp.y / _hp;
			int damageId = 0;
			if (normDamage > 0.4f) damageId = 1;
			if (normDamage > 0.7f) damageId = 2;

			TileManager.damageTilemap.SetTile(_position, _tile.damageLevelSprites[damageId]);
		}
	}

	public void WalkByPlayer(PlayerController player)
	{
		if (_doDamage)
			player.playerHealth.ReceivedDamage(1, TileManager.mainTilemap.GetCellCenterWorld(_position));
	}

	public bool CanResist(Bullet b)
	{
		return _tile.resistanceLevel > b.weaponLevel || _tile.indestructible;
	}

	private void Flash(){
		TileManager.instance.StartCoroutine(FlashRoutine());
		flashCount++;
	}

	private IEnumerator FlashRoutine()
	{
		TileManager.vfxTile.SetTile(_position, _tile.vfxDamageTile);
		yield return new WaitForSeconds(0.02f);
		TileManager.vfxTile.SetTile(_position, null);
	}

}
