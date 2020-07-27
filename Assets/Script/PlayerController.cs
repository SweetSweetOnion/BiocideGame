using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Prime31;

[RequireComponent(typeof(BoxCollider2D), typeof(CharacterController2D))]
public class PlayerController : MonoBehaviour
{
	//inspector
	[Header("Jump")]
	public float maxJumpHeight = 5;
	public float timeToJumpHeight = 1;
	public float minJumpTime = 0.2f;
	public float timeToFall = 0.2f;
	public float jumpCooldown = 0.2f;

	[Header("Move")]
	public float acceleration = 1;
	public float deceleration = 2;
	public float speed = 10;
	public float aerialSpeed = 10;

	//Accessors
	public bool isGrounded => _controller2D.isGrounded;
	public Vector3 velocity => _controller2D.velocity;
	public bool isFlip => _isFlip;

	private float _gravity = -9.8f;
	private Vector3 _velocity;
	private float _jumpGravity;
	private float _fallGravity;
	private float _jumpVelocity;
	private float _jumpTime = 0;
	private float _moveInput = 0;
	private bool _jumpInput;
	private bool _canJump = false;
	private Vector3 _externalForce;
	private bool _isFlip = false;

	private BoxCollider2D boxCollider;
	private CharacterController2D _controller2D;

	//events
	public delegate void TeleportEvent(Vector2 previous, Vector2 newPos);
	public event TeleportEvent OnTeleport;

	private void Awake()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		_controller2D = GetComponent<CharacterController2D>();
	}

	public void SetPosition(Vector2 newPos)
	{
		Vector2 old = transform.position;
		transform.position = newPos;
		OnTeleport?.Invoke(old, newPos);
	}

	public void AddForce(Vector2 v){
		_externalForce += new Vector3(v.x,v.y,0);
	}

	private void Update()
	{
		UpdateInput();
		UpdateJump();

		if (_moveInput != 0)
		{
			if(isGrounded){
				_velocity.x = Mathf.MoveTowards(_velocity.x, speed * _moveInput, acceleration * Time.deltaTime);
			}else{
				_velocity.x = Mathf.MoveTowards(_velocity.x, aerialSpeed * _moveInput, acceleration * Time.deltaTime);
			}
			
		}
		else
		{
			_velocity.x = Mathf.MoveTowards(_velocity.x, 0, deceleration * Time.deltaTime);
			
		}

	
	

		_controller2D.move(_velocity * Time.deltaTime + _externalForce);

		_externalForce = Vector3.zero;
	}

	private void UpdateInput()
	{
		if (Input.GetButtonDown("Jump"))
		{
			_jumpInput = true;
		}
		if (Input.GetButtonUp("Jump"))
		{
			_jumpInput = false;
		}

		_moveInput = Input.GetAxis("Horizontal");
		if(_moveInput > 0){
			_isFlip = false;
		}
		if (_moveInput < 0)
		{
			_isFlip = true;
		}
	}

	private void UpdateJump()
	{
		_jumpGravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpHeight, 2);
		_fallGravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToFall, 2);
		_jumpVelocity = Mathf.Abs(_jumpGravity) * timeToJumpHeight;


		if (_controller2D.isGrounded)
		{
			_jumpTime = timeToJumpHeight;
			_velocity.y = 0;
			if (_jumpInput && _canJump)
			{
				_velocity.y = _jumpVelocity;
				_jumpTime = 0;
				_canJump = false;
			}
		}

		if (!_jumpInput)
		{
			_canJump = true;
		}

		if (!_jumpInput && _jumpTime > minJumpTime)
		{
			_jumpTime = timeToJumpHeight;
		}

		_gravity = _jumpTime >= timeToJumpHeight ? _fallGravity : _jumpGravity;

		_jumpTime += Time.deltaTime;

		_velocity.y += _gravity * Time.deltaTime;
	}

}
