using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{

	private PlayerController _controller;
	[SerializeField]
	private Weapon _weapon;
	private float _pressure = 100;

	private float lastSpawnTime = 0;

	private void Awake()
	{
		_controller = GetComponent<PlayerController>();
	}

	private void Update()
	{
		if (!_weapon) return;

		if(Input.GetAxis("RightTrigger") > 0.2f){

			if(Time.time > lastSpawnTime + _weapon.GetSpawnCooldown(_pressure)){
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

		if(Input.GetButtonUp("Reload")){
			_pressure += _weapon.pressureAddPerReload;
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
