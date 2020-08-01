using UnityEngine;
using System.Collections;

public class Decoration : MonoBehaviour
{
	public Vector3 bottomPoint;
	public bool doDamage;
	private Vector3Int pos;
	private void OnEnable()
	{

		//TileManager.OnTileDamage += OnTileDamage ;
		TileManager.OnTileDestroy += OnTileDestroy;
	}

	

	private void OnDisable()
	{
		//TileManager.OnTileDamage -= OnTileDamage;
		TileManager.OnTileDestroy -= OnTileDestroy;

	}

	private void Start()
	{
		FindTile();
	}

	private void FindTile(){
		 pos = TileManager.mainTilemap.WorldToCell(transform.position + bottomPoint);
	}


	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(transform.position, transform.position + bottomPoint);
	}


	private void OnTileDestroy(Vector3Int position, float experience)
	{
		if(pos == position){
			Destroy(gameObject);
		}
	}
	/*private void OnTileDamage(Vector3Int position, EnvironementTile envTile)
	{
		if (pos == position)
		{
			Destroy(gameObject);
		}
	}*/

	private void OnTriggerStay2D(Collider2D other)
	{
		if (!doDamage) return;	
		PlayerHealth hp = other.GetComponent<PlayerHealth>();
		if(hp){
			hp.ReceivedDamage(1, transform.position + bottomPoint);
		}
	}

}
