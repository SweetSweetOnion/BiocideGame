using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class OiseauManager : MonoBehaviour
{
	
	public WeaponManager manager;
	public int killPerLevel = 5;


	public static List<Bird> birds;

	private void Awake()
	{
		birds = FindObjectsOfType<Bird>().ToList() ;
	}

	private void OnEnable()
	{
		manager.OnLevelUp += OnLevelUp;
	}



	private void OnDisable()
	{
		manager.OnLevelUp -= OnLevelUp;
	}

	private void OnLevelUp(int newLevel)
	{
		int k = 0;
		for(int i = 0; i< birds.Count; i++){
			if(k<killPerLevel){
				birds[i].Kill();
				i--;
			}
			k++;

		}
	}
}
