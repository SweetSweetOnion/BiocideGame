using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class VFXManager : MonoBehaviour
{
	public Animator impactVFX;
	public PlayerHealth ph;

	public ParticleSystem part;

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
		ph.OnDead += OnDead;
	}

	

	private void OnDisable()
	{
		Bullet.OnBulletHit -= OnBulletHit;
		ph.OnDead -= OnDead;

	}

	private void SpawnImpact(Vector3 pos, float scaleMult = 1){
		if (currentImpact >= impacts.Length) currentImpact = 0;
		impacts[currentImpact].transform.position = pos;
		impacts[currentImpact].transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-360, 360));
		impacts[currentImpact].transform.localScale = Vector3.one * Random.Range(0.7f, 1.1f) * scaleMult;
		impacts[currentImpact].ResetTrigger("Trigger");
		impacts[currentImpact].SetTrigger("Trigger");
		currentImpact++;
		part.transform.position = pos;
		part.Play();
	}
	private void OnDead()
	{
		SpawnImpact(ph.transform.position,2);
	}


	private void OnBulletHit(Vector3 pos)
	{
		SpawnImpact(pos);
	}
}
