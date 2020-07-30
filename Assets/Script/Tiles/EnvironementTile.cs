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
	private Coroutine flashRoutine;
	private bool _doDamage;
	private float _tickTime;
	//accesors
	public Color color => _startColor;

	public EnvironementTile(TileType t, Vector3Int pos)
	{
		_tile = t;
		_position = pos;
		_hp = Random.Range(t.startHp.x, t.startHp.y);
		_startColor = GameManager.tilemap.GetColor(pos);
		_doDamage = t.doDamage;
		_tickTime = Random.Range(0, _tile.tickCooldDown);
		if (_tile.indestructible)
			TileManager.instance.indestructibleTileMap.SetTile(pos, _tile.indestructibleTile);
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
		TileManager.instance.damageTilemap.SetTile(_position, _tile.damageLevelSprites[0]);
		if (_hp <= 0)
		{
			if(_tile.transformTo){
				TileManager.TransformTile(_position, _tile.transformTo);
			}else{
				TileManager.RemoveTile(_position);
			}
			StopFlash();
		}
		else{
			Flash();
		}
	}

	public void WalkByPlayer(PlayerController player)
	{
		if (_doDamage)
			player.playerHealth.ReceivedDamage(1, GameManager.tilemap.GetCellCenterWorld(_position));
	}

	public bool CanResist(Bullet b)
	{
		return _tile.resistanceLevel > b.weaponLevel;
	}

	public void SetColor(Color c)
	{
		GameManager.tilemap.SetTileFlags(_position, TileFlags.None);
		GameManager.tilemap.SetColor(_position, c);

	}

	public void Flash()
	{
		if (flashRoutine != null)
		{
			TileManager.instance.StopCoroutine(flashRoutine);
		}
		flashRoutine = TileManager.instance.StartCoroutine(FlashRoutine());
	}

	public void StopFlash(){
		if(flashRoutine!= null)
		TileManager.instance.StopCoroutine(flashRoutine);
	}

	private IEnumerator FlashRoutine()
	{
		SetColor(Color.white);
		yield return new WaitForSeconds(0.02f);
		SetColor(_startColor);
	}

}
