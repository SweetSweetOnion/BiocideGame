using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
	public PlayerController controller;
	public PlayerHealth playerHealth;

	private Vector3 startPosition;
	//[SerializeField]
	//private Tilemap _tilemap;

	public static GameManager instance;
	//public static Tilemap tilemap => instance._tilemap;

	public delegate void BasicEvent();
	public static event BasicEvent OnRespawn;

	private void Awake()
	{
		if(instance){
			Destroy(this);return;
		}
		instance = this;
	}

	private void Start()
	{
		startPosition = controller.transform.position;
	}

	private void OnEnable()
	{
		playerHealth.OnDead += PlayerHealth_OnDead;
	}


	private void OnDisable()
	{
		playerHealth.OnDead -= PlayerHealth_OnDead;

	}


	private void PlayerHealth_OnDead()
	{
		StartCoroutine(EndGameRoutine());
	}
	public void DoReset()
	{
		controller.transform.position = startPosition;
		controller.playerHealth.ResetHp();
		OnRespawn?.Invoke();
	}
	private IEnumerator EndGameRoutine(){
		yield return new WaitForSeconds(3);
		controller.transform.position = startPosition;
		controller.playerHealth.ResetHp();
		OnRespawn?.Invoke();
	}
}
