using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

public class UIHP : MonoBehaviour
{
	public PlayerHealth health;
	public GameObject prefab;

	private List<GameObject> hps = new List<GameObject>();

	private void OnEnable()
	{
		health.OnDamage += OnDamage;
		health.OnReset += OnReset;
	}

	

	private void OnDisable()
	{
		health.OnDamage -= OnDamage;
		health.OnReset -= OnReset;


	}

	private void OnDamage(int amount, Vector3 worldOrigin)
	{
		RemoveHp();
	}

	private void AddHp(){
		hps.Add( Instantiate(prefab, transform));
	}

	private void RemoveHp(){
		if (hps.Count > 0)
		{
			StartCoroutine(RemoveRoutine(hps[hps.Count - 1]));
			hps.RemoveAt(hps.Count - 1);
			
		}
	}

	private IEnumerator RemoveRoutine(GameObject g){
		var t = g.GetComponent<Text>();
		t.color = Color.white;
		t.fontStyle = FontStyle.Bold;
		t.fontSize = 60;
		yield return new WaitForSeconds(0.1f);
		Destroy(g);
	}

	private void OnReset()
	{
		for(int i = 0; i< hps.Count; i++){
			RemoveHp();
		}

		for(int i =0; i< health.startHp; i++){
			AddHp();
		}
	}
}
