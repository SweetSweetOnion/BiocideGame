using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
	public PlayerController controller;
	public WeaponManager weaponManager;
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
	}



	private void OnDisable()
	{
		controller.OnJump -= OnJump;
		weaponManager.OnLevelUp -= OnLevelUp;

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
	}
}
