using UnityEngine;
using System.Collections;

public class Decoration : MonoBehaviour
{
	public Vector3 bottomPoint;
	public bool doDamage;
	private Vector3Int pos;

	public Sprite[] spriteHP;
	private SpriteRenderer spriteRend;

	private void Awake()
	{
		spriteRend = GetComponent<SpriteRenderer>();
	}
	private void OnEnable()
	{

		TileManager.OnTileDamage += OnTileDamage ;
		TileManager.OnTileToxicDamage += OnTileDamage;

		TileManager.OnTileDestroy += OnTileDestroy;
	}

	

	private void OnDisable()
	{
		TileManager.OnTileDamage -= OnTileDamage;
		TileManager.OnTileToxicDamage -= OnTileDamage;
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

	private void OnTileDamage(Vector3Int position, EnvironementTile envTile)
	{
		if (pos == position)
		{
			if(spriteHP.Length > 0)
				spriteRend.sprite = GetSpriteHp(envTile.GetNormHp());

			StartCoroutine(FlashRoutine());
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (!doDamage) return;	
		PlayerHealth hp = other.GetComponent<PlayerHealth>();
		if(hp){
			hp.ReceivedDamage(1, transform.position + bottomPoint);
		}
	}

	private Sprite GetSpriteHp(float normHp){
		float rev = 1 - normHp;
		int index = Mathf.Clamp(Mathf.FloorToInt(rev * (spriteHP.Length)),0,spriteHP.Length-1);
		return spriteHP[index];
	}

	private IEnumerator FlashRoutine(){
		spriteRend.material.SetColor("_AddColor", new Color(1, 1, 1, 0));
		yield return new WaitForSeconds(0.02f);
		spriteRend.material.SetColor("_AddColor", new Color(0, 0, 0, 0));

	}
}
