using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Prime31;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;

[RequireComponent(typeof(BoxCollider2D), typeof(CharacterController2D))]
public class PlayerController : MonoBehaviour
{
	//inspector
	[Header("Jump")]
	public float maxJumpHeight = 5;
	public float timeToJumpHeight = 1;
	public float minJumpTime = 0.2f;
	public float timeToFallJump = 0.2f;
	public float jumpCooldown = 0.2f;
	public float timeToFallnormal = 0.5f;

	[Header("Move")]
	public float acceleration = 1;
	public float deceleration = 2;
	public float speed = 10;
	public float aerialSpeed = 10;

	[Header("Recoil")]
	public float damageRecoil = 1;
	public float recoilDuration = 0.3f;

	public float landedLockDuration = 1f;
	public float aerialTimeToLock = 1;

	//Accessors
	public bool isGrounded => _controller2D.isGrounded;
	public Vector3 velocity => _controller2D.velocity;
	public bool isFlip => _isFlip;
	public PlayerHealth playerHealth => _playerHealth;

	//privates fields

	private float _gravity = -9.8f;
	private Vector3 _velocity;
	private float _jumpGravity;
	private float _fallGravity;
	private float _normalGravity;
	private float _jumpVelocity;
	private float _jumpTime = 0;
	private float _moveInput = 0;
	private bool _jumpInput;
	private bool _canJump = false;
	private bool _hasJump = false;
	private Vector3 _externalForce;
	private float _lastJump;
	private bool _isFlip = false;
	private bool _lastGrounded = false;
	private float _fallDuration = 0;
	private float _groundedDuration = 0;
	private bool _shouldLock;

	private BoxCollider2D _boxCollider;
	private CharacterController2D _controller2D;
	private PlayerInput _playerInput;
	private PlayerHealth _playerHealth;

	private Vector3Int[] currentTiles = new Vector3Int[2];

	//events
	public delegate void TeleportEvent(Vector2 previous, Vector2 newPos);
	public event TeleportEvent OnTeleport;
	public delegate void FloatEvent(float f);
	public event FloatEvent OnLanded;
	public delegate void JumpEvent();
	public event JumpEvent OnJump;

	private void Awake()
	{
		_boxCollider = GetComponent<BoxCollider2D>();
		_controller2D = GetComponent<CharacterController2D>();
		_playerInput = GetComponent<PlayerInput>();
		_playerHealth = GetComponent<PlayerHealth>();
	}

	private void OnEnable()
	{
		_playerInput.currentActionMap["Jump"].started += JumpStart;
		_playerInput.currentActionMap["Jump"].canceled += JumpEnd;
		_playerInput.currentActionMap["Move"].performed += MovePerformed;
		_playerHealth.OnDamage += OnDamage;
	}

	

	private void OnDisable()
	{
		_playerInput.currentActionMap["Jump"].started -= JumpStart;
		_playerInput.currentActionMap["Jump"].canceled -= JumpEnd;
		_playerInput.currentActionMap["Move"].performed -= MovePerformed;
		_playerHealth.OnDamage += OnDamage;

	}

	private void OnDamage(int amount, Vector3 worldOrigin)
	{
		Vector3 v = transform.position - worldOrigin;
		Vector2 vv = new Vector2(v.x, v.y).normalized;
		AddForce(vv * damageRecoil, recoilDuration);
		//_jumpInput = true;
	}

	private void MovePerformed(InputAction.CallbackContext obj)
	{
		_moveInput = obj.ReadValue<float>();
		if (_moveInput > 0) _isFlip = false;
		if (_moveInput < 0) _isFlip = true;
	}

	private void JumpStart(InputAction.CallbackContext obj)
	{
		_jumpInput = true;
	}

	private void JumpEnd(InputAction.CallbackContext obj)
	{
		_jumpInput = false;
	}

	public void SetPosition(Vector2 newPos)
	{
		Vector2 old = transform.position;
		transform.position = newPos;
		OnTeleport?.Invoke(old, newPos);
	}

	public void AddForce(Vector2 v)
	{
		_externalForce += new Vector3(v.x, v.y, 0);
	}

	public void AddForce(Vector2 v, float duration){
		StartCoroutine(ForceOverTime(v, duration)) ;
	}

	private IEnumerator ForceOverTime(Vector2 v, float duration){
		float t = 0;
		while(t < duration){
			t += Time.deltaTime;
			AddForce(v * Time.deltaTime);
			yield return null;
		}
	}

	private void Update()
	{
		UpdateCurrentTiles();
		UpdateJump();

		float currentInput = _moveInput;

		if(isGrounded && _groundedDuration < landedLockDuration){
			if(_shouldLock)
			currentInput = 0;
		}else{
			_shouldLock = false;
		}

		if (currentInput != 0 )
		{
			if (isGrounded)
			{
				_velocity.x = Mathf.MoveTowards(_velocity.x, speed * _moveInput, acceleration * Time.deltaTime);
			}
			else
			{
				_velocity.x = Mathf.MoveTowards(_velocity.x, aerialSpeed * _moveInput, acceleration * Time.deltaTime);
			}

		}
		else
		{
			_velocity.x = Mathf.MoveTowards(_velocity.x, 0, deceleration * Time.deltaTime);

		}



		if(velocity.y > 0){
			_externalForce.y = 0;
		}
		_controller2D.move(_velocity * Time.deltaTime + _externalForce);

		_externalForce = Vector3.zero;
		if(!isGrounded){
			_fallDuration += Time.deltaTime;
			_groundedDuration = 0;
		}else{
			_groundedDuration += Time.deltaTime;
		}
		if(!_lastGrounded && isGrounded){
			OnLanded?.Invoke(_fallDuration);
			if (_fallDuration > aerialTimeToLock) _shouldLock = true;
			_fallDuration = 0;
		}
		_lastGrounded = isGrounded;
	}

	private void UpdateCurrentTiles(){
		Vector3 bottomLeftPos = transform.position - Vector3.up * _boxCollider.size.y / 2 + Vector3.left * _boxCollider.size.x /2;
		Vector3 bottomRightPos = transform.position - Vector3.up * _boxCollider.size.y / 2 - Vector3.left * _boxCollider.size.x /2;
		currentTiles[0] = TileManager.mainTilemap.WorldToCell(bottomLeftPos - Vector3.up *0.1f);
		currentTiles[1] = TileManager.mainTilemap.WorldToCell(bottomRightPos - Vector3.up * 0.1f);
		if(isGrounded){
			TileManager.OnTilesWalkedOn(currentTiles, this);
		}
	}

	private void UpdateJump()
	{
		_jumpGravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpHeight, 2);
		_fallGravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToFallJump, 2);
		_normalGravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToFallnormal, 2);

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
				_hasJump = true;
				_lastJump = Time.time;
				OnJump?.Invoke();
			}else{
				_hasJump = false;
			}
		}

		if (!_jumpInput && Time.time >= _lastJump + jumpCooldown)
		{
			_canJump = true;
		}

		if (!_jumpInput && _jumpTime > minJumpTime)
		{
			_jumpTime = timeToJumpHeight;
		}

		if(_hasJump){
			_gravity = _jumpTime >= timeToJumpHeight ? _fallGravity : _jumpGravity;
		}else{
			_gravity = _normalGravity;
		}

		_jumpTime += Time.deltaTime;

		_velocity.y += _gravity * Time.deltaTime;
	}

}
