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

	//accesors
	public Color color => _startColor;

	public EnvironementTile(TileType t, Vector3Int pos)
	{
		_tile = t;
		_position = pos;
		_hp = Random.Range(t.startHp.x, t.startHp.y);
		_startColor = GameManager.tilemap.GetColor(pos);
		_doDamage = t.doDamage;
	}

	public void UpdateToxicTiles(){
		if(_doDamage && Random.value > 0.95f){
			DoToxic(Vector3Int.left);
			DoToxic(Vector3Int.right);
			DoToxic(Vector3Int.up);
			DoToxic(Vector3Int.down);
		}
	}

	private void DoToxic(Vector3Int offset){
		var other = TileManager.GetOrCreateEnvironementTile(_position + offset);
		if (other == null) return;
		if (other._tile.indestructible == false &&_tile.resistanceLevel >= other._tile.resistanceLevel){
			other.ReceiveDamage(10);
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
