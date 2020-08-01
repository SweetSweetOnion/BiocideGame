using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class UILevelUp : MonoBehaviour
{
	public WeaponManager manager;
	public Text text;

	private Animator anim;

	private void Awake()
	{
		anim = GetComponent<Animator>();
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
		text.text = manager.GetCurrentWeapon().levelUpText;
		anim.ResetTrigger("LevelUp");
		anim.SetTrigger("LevelUp");
	}

}
