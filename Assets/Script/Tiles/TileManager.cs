using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Runtime.CompilerServices;

public class TileManager : MonoBehaviour
{
	public static TileManager instance;

	private static Dictionary<Vector3Int, EnvironementTile> tiles = new Dictionary<Vector3Int, EnvironementTile>();


	private Dictionary<Vector3Int, Coroutine> flashDictionary = new Dictionary<Vector3Int, Coroutine>();


	private void Awake()
	{
		if (instance)
		{
			Destroy(this); return;
		}
		instance = this;
	}

	public static void OnTileHit(Vector3Int position, Bullet bullet){
		
		if(!tiles.ContainsKey(position)){
			EnvironementTile tile = new EnvironementTile(GameManager.tilemap.GetTileType(position), position);
			tiles.Add(position, tile);
		}	
		tiles[position].Flash();
		if(!tiles[position].CanResist(bullet)){
			tiles[position].DealDamage(bullet.damage);
		}
	}

	public static void RemoveTile(Vector3Int position)
	{
		if (tiles.ContainsKey(position))
		{
			tiles.Remove(position);
		}
		GameManager.tilemap.SetTile(position, null);
	}



	/*public static void FlashTile(Vector3Int position, Color flashColor, float flashDuration)
	{
		if (instance.flashDictionary.ContainsKey(position))
		{
			instance.StopCoroutine(instance.flashDictionary[position]);
			instance.flashDictionary.Remove(position);
		}
		else{

		}

		var c = instance.StartCoroutine(FlashRoutine(position, flashColor, flashDuration));
		instance.flashDictionary.Add(position, c);
	}

	private static IEnumerator FlashRoutine(Vector3Int position, Color flashColor, float flashDuration)
	{
		float t = 0;
		Color normalColor = GameManager.tilemap.GetEnvironementTile(position).normalColor;
		GameManager.tilemap.SetTileFlags(position, TileFlags.None);
		while (t < 1)
		{
			Color c = Color.Lerp(flashColor, normalColor, t * t);
			
			GameManager.tilemap.SetColor(position, c);
			t += Time.deltaTime / flashDuration;
			yield return null;
		}
		GameManager.tilemap.SetColor(position, normalColor);
		instance.flashDictionary.Remove(position);
	}*/
}
