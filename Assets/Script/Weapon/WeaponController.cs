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

        //SOUND
        AkSoundEngine.SetState("WeaponLevel", "Level_00");
        //SOUND
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

        //SOUND
        AudioManager.instance.FOLEYS_Weapon_Trigger.Post(gameObject);
        if (AudioManager.weaponLevel == 0 || AudioManager.weaponLevel == 1 || AudioManager.weaponLevel == 4)
        {
            AudioManager.instance.FOLEYS_Weapon_Shoot.Post(gameObject);
        }
        //SOUND
    }

    private void FireEnd(InputAction.CallbackContext obj)
	{
		_useWeapon = false;

        //SOUND
        if (AudioManager.weaponLevel == 0 || AudioManager.weaponLevel == 1 || AudioManager.weaponLevel == 4)
        {
            AudioManager.instance.FOLEYS_Weapon_Shoot.Stop(gameObject);
        }
        //SOUND
    }

    private void ReloadPerformed(InputAction.CallbackContext obj)
	{
		_pressure += _weapon.pressureAddPerReload;

        //SOUND
        AudioManager.instance.FOLEYS_Weapon_Reload.Post(gameObject);
        //SOUND
    }

	private void Update()
	{
        //SOUND
        AkSoundEngine.SetRTPCValue("WeaponCharge", _pressure);
        //SOUND

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

                //SOUND
                if (AudioManager.weaponLevel == 2 || AudioManager.weaponLevel == 3)
                {
                    AudioManager.instance.FOLEYS_Weapon_Shoot.Post(gameObject);
                }
                //SOUND
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
