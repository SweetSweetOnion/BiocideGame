using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlayerAnimation : MonoBehaviour
{
	public PlayerController controller;
	public WeaponManager weaponManager;
	public PlayerHealth health;

	private SpriteRenderer spriteRenderer;
	private Animator animator;

	public RuntimeAnimatorController[] animatorControllers;
	

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();

	}

	private void OnEnable()
	{
		controller.OnJump += OnJump;
		weaponManager.OnLevelUp += OnLevelUp;
		health.OnDamage += OnDamage;
	}

	

	private void OnDisable()
	{
		controller.OnJump -= OnJump;
		weaponManager.OnLevelUp -= OnLevelUp;
		health.OnDamage -= OnDamage;


	}

	private void Update()
	{
		
		spriteRenderer.flipX = controller.isFlip;

		if(controller.isGrounded){
			animator.SetBool("Jumping", false);
			if (controller.velocity.x > 0.1f){
				animator.SetBool("Walking", true);
			}
			else{
				animator.SetBool("Walking", false);
			}
		}		
	}

	private void OnJump()
	{
		animator.SetBool("Jumping", true);
	}

	private void OnLevelUp(int newLevel)
	{
		animator.runtimeAnimatorController = animatorControllers[newLevel];
		transform.DOPunchScale(Vector3.one * 2.2f, 1);
	}

	private void OnDamage(int amount, Vector3 worldOrigin)
	{
		StopAllCoroutines();
		StartCoroutine(BlinkRoutine(5, 0.1f));
	}

	private IEnumerator  BlinkRoutine(int blinkCount, float blinkDuration){
		int t = 0;
		while(t < blinkCount)
		{
			spriteRenderer.enabled = false;
			yield return new WaitForSeconds(blinkDuration);
			spriteRenderer.enabled = true;
			yield return new WaitForSeconds(blinkDuration);
			t++;
		}
	}
}
