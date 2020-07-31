using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
	public PlayerController controller;
	private SpriteRenderer spriteRenderer;
	private Animator animator;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();

	}

	private void OnEnable()
	{
		controller.OnJump += OnJump;
	}

	

	private void OnDisable()
	{
		controller.OnJump -= OnJump;

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
}
