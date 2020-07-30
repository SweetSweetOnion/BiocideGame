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

	public void UpdateTile(){
		if(_doDamage){
			TileManager.GetOrCreateEnvironementTile(_position + Vector3Int.left)?.ReceiveDamage(1 * Time.deltaTime);
			TileManager.GetOrCreateEnvironementTile(_position + Vector3Int.right)?.ReceiveDamage(1 * Time.deltaTime);
			TileManager.GetOrCreateEnvironementTile(_position + Vector3Int.up)?.ReceiveDamage(1 * Time.deltaTime);
			TileManager.GetOrCreateEnvironementTile(_position + Vector3Int.down)?.ReceiveDamage(1 * Time.deltaTime);
		}
	}

	public void ReceiveDamage(float damageAmount)
	{
		//Flash();
		_hp -= damageAmount;
		if (_hp <= 0)
		{
			if(_tile.transformTo){
				TileManager.TransformTile(_position, _tile.transformTo);
			}else{
				TileManager.RemoveTile(_position);
			}
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

	private IEnumerator FlashRoutine()
	{
		SetColor(Color.white);
		yield return new WaitForSeconds(0.02f);
		SetColor(_startColor);
	}

}
