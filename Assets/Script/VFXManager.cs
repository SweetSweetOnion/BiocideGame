using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class VFXManager : MonoBehaviour
{
	public Animator impactVFX;

	private Animator[] impacts = new Animator[20];
	private int currentImpact = 0;

	private void Awake()
	{
		for(int i = 0; i< impacts.Length; i++){
			impacts[i] = Instantiate(impactVFX, Vector3.up * 1000, Quaternion.identity);
		}
	}

	private void OnEnable()
	{
		Bullet.OnBulletHit += OnBulletHit;
	}

	private void OnDisable()
	{
		Bullet.OnBulletHit -= OnBulletHit;

	}



	private void OnBulletHit(Vector3 pos)
	{
		if (currentImpact >= impacts.Length) currentImpact = 0;
		 impacts[currentImpact].transform.position = pos;
		impacts[currentImpact].ResetTrigger("Trigger");
		impacts[currentImpact].SetTrigger("Trigger");
		currentImpact++;
	}
}
