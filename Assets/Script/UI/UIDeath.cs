using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class UIDeath : MonoBehaviour
{
	public PlayerHealth pHealth;
	public Image background;
	public Text endText;

	private void OnEnable()
	{
		endText.enabled = false;
		background.enabled = false;
		pHealth.OnDead += OnDead;
		GameManager.OnRespawn += OnRespawn;
	}

	

	private void OnDisable()
	{
		pHealth.OnDead -= OnDead;
		GameManager.OnRespawn -= OnRespawn;
	}

	private void OnDead()
	{
		endText.enabled = true;
		background.enabled = true;
		Color c = Color.black;
		c.a = 0;
		background.color = c;

		background.DOFade(1, 1);
	}

	private void OnRespawn()
	{
		endText.enabled = false;
		background.enabled = false;
	}
}
