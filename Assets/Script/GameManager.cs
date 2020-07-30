using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
	//[SerializeField]
	//private Tilemap _tilemap;

	public static GameManager instance;
	//public static Tilemap tilemap => instance._tilemap;

	private void Awake()
	{
		if(instance){
			Destroy(this);return;
		}
		instance = this;
	}
}
