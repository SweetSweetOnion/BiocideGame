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

	//accesors
	public Color color => _startColor;

	public EnvironementTile(TileType t, Vector3Int pos)
	{
		_tile = t;
		_position = pos;
		_hp = Random.Range(t.startHp.x,t.startHp.y);
		_startColor = GameManager.tilemap.GetColor(pos);
	}

	public void DealDamage(float damageAmount){
		_hp -= damageAmount;
		if(_hp <= 0){
			TileManager.RemoveTile(_position);
		}
	}

	public void SetColor(Color c){
		GameManager.tilemap.SetTileFlags(_position, TileFlags.None);
		GameManager.tilemap.SetColor(_position, c);

	}

	public void Flash(){
		if(flashRoutine != null){
			TileManager.instance.StopCoroutine(flashRoutine);
		}
		flashRoutine = TileManager.instance.StartCoroutine(FlashRoutine());
	}

	private IEnumerator FlashRoutine(){
		SetColor(Color.white);
		yield return new WaitForSeconds(0.1f);
		SetColor(_startColor);
	}

}
