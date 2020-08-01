using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{

	private PlayerController _controller;
	[SerializeField]
	private Weapon _weapon;
	private float _pressure = 100;

	private float lastSpawnTime = 0;

	private PlayerInput _playerInput;

	private bool _useWeapon = false;

	public float pressure => _pressure;
	public Weapon currentWeapon => _weapon;


	private void Awake()
	{
		_controller = GetComponent<PlayerController>();
		_playerInput = GetComponent<PlayerInput>();
	}

	private void OnEnable()
	{
		_playerInput.currentActionMap["Fire"].started += FireStart;
		_playerInput.currentActionMap["Fire"].canceled += FireEnd;
		_playerInput.currentActionMap["Reload"].performed += ReloadPerformed;

	}

	private void OnDisable()
	{
		_playerInput.currentActionMap["Fire"].started -= FireStart;
		_playerInput.currentActionMap["Fire"].canceled -= FireEnd;
		_playerInput.currentActionMap["Reload"].performed -= ReloadPerformed;
	}

	private void FireStart(InputAction.CallbackContext obj)
	{
		_useWeapon = true;
	}

	private void FireEnd(InputAction.CallbackContext obj)
	{
		_useWeapon = false;
	}

	private void ReloadPerformed(InputAction.CallbackContext obj)
	{
		_pressure += _weapon.pressureAddPerReload;
	}

	private void Update()
	{
		if (!_weapon) return;

		if(_useWeapon)
		{

			if(Time.time > lastSpawnTime + _weapon.GetSpawnCooldown(_pressure)){
				//shoot

				Vector2 recoil = _weapon.recoil;
				Vector2 shootDirection = Vector2.right;
				if(_controller.isFlip ){
					recoil.x *= -1;
					shootDirection.x *= -1;
				}
				_controller.AddForce(recoil * _weapon.GetPressure(_pressure));
				_weapon.SpawnBullet(transform.position, shootDirection, _controller.velocity,_pressure) ;
				lastSpawnTime = Time.time;
				_pressure += _weapon.pressureAddPerUse;
			}
		}
		float p = _pressure/100f;
		_pressure += _weapon.pressureAddEverySecond * Time.deltaTime;
		_pressure = Mathf.Clamp(_pressure, 0, 100);
	}

	public void SwitchWeapon(Weapon nextWeapon ){
		_weapon = nextWeapon;
		_pressure = 100;
	}
}
