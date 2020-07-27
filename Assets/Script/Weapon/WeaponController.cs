using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{

	private PlayerController _controller;
	public Weapon weapon;
	private float _pressure = 100;

	private float lastSpawnTime = 0;

	private void Awake()
	{
		_controller = GetComponent<PlayerController>();
	}

	private void Update()
	{
		if (!weapon) return;

		if(Input.GetAxis("RightTrigger") > 0.2f){

			if(Time.time > lastSpawnTime + weapon.GetSpawnCooldown(_pressure)){
				Vector2 recoil = weapon.recoil;
				Vector2 shootDirection = Vector2.right;
				if(_controller.isFlip ){
					recoil.x *= -1;
					shootDirection.x *= -1;
				}
				_controller.AddForce(recoil * weapon.GetPressure(_pressure));
				weapon.SpawnBullet(transform.position, shootDirection, _controller.velocity,_pressure) ;
				lastSpawnTime = Time.time;
				_pressure += weapon.pressureAddPerUse;
			}
		}

		if(Input.GetButtonUp("Reload")){
			_pressure += weapon.pressureAddPerReload;
		}
		float p = _pressure/100f;
		Debug.Log(p + "  " + p * p);
		_pressure += weapon.pressureAddEverySecond * Time.deltaTime;
		_pressure = Mathf.Clamp(_pressure, 0, 100);
	}
}
