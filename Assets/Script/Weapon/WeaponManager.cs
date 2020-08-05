using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Tilemaps;

public class WeaponManager : MonoBehaviour
{
	private WeaponController _weaponController;
	public Weapon[] allWeapons;
	public float[] experienceLevel;

	private float _experience = 0;
	private int _currentWeaponIndex = 0;

	public int currentLevel => _currentWeaponIndex;

	public delegate void ExperienceChange(float newExp);
	public event ExperienceChange OnExperienceChange;
	public delegate void LevelChange(int newLevel);
	public event LevelChange OnLevelUp;


	//accessors
	public float experience => _experience;

	private void Awake()
	{
		_weaponController = GetComponent<WeaponController>();
	}

	private void OnEnable()
	{
		TileManager.OnTileDestroy += OnTileDestroy;
	}

	private void OnDisable()
	{
		TileManager.OnTileDestroy -= OnTileDestroy;
	}

    private void OnTileDestroy(Vector3Int position, float experience)
	{
		Collect(experience);
	}
	private void Collect(float amount)
	{
		_experience += amount;
		if (_currentWeaponIndex < allWeapons.Length - 1)
		{
			if (_experience >= experienceLevel[_currentWeaponIndex + 1])
			{
				_currentWeaponIndex++;
				_weaponController.SwitchWeapon(GetCurrentWeapon());
				OnLevelUp?.Invoke(_currentWeaponIndex);
                //Sound
                AudioManager.instance.EFFECT_Weapon_LevelUp.Post(gameObject);
                AudioManager.weaponLevel = _currentWeaponIndex;
                if (_currentWeaponIndex == 1)
                {
                    AkSoundEngine.SetState("MainMusic", "Level_01");
                }
                else
                    AkSoundEngine.SetState("MainMusic", "Level_0" + (GetCurrentWeapon().bulletLevel + 1).ToString());
                AudioManager.instance.FOLEYS_Weapon_Shoot.Stop(_weaponController.gameObject);
                AkSoundEngine.SetState("WeaponLevel", "Level_0" + _currentWeaponIndex.ToString());
                AkSoundEngine.SetState("DestructionLevel", "Level_" + (GetCurrentWeapon().bulletLevel + 1).ToString());
                if (GetCurrentWeapon().bulletLevel == 2)
                {
                    AudioManager.instance.AMB_Nature.Stop(AudioManager.instance.gameObject, 10, AkCurveInterpolation.AkCurveInterpolation_SCurve);
                    AudioManager.instance.AMB_Destroyed.Post(AudioManager.instance.gameObject);
                }
                if (_weaponController.isShooting)
                    AudioManager.instance.FOLEYS_Weapon_Shoot.Post(_weaponController.gameObject);
                //Sound
            }
		}
		OnExperienceChange?.Invoke(_experience);
	}

	private void OnTileDamage(Tuple<Vector3Int, float> tuple)
	{
		Collect(tuple.Item2);
	}

	public Weapon GetCurrentWeapon()
	{
		return allWeapons[_currentWeaponIndex];
	}

	public float GetNormalizedExperience()
	{
		
		float f = (_experience - experienceLevel[_currentWeaponIndex]) / (experienceLevel[_currentWeaponIndex + 1] - experienceLevel[_currentWeaponIndex]);
		//Debug.Log(_experience + "/" + experienceLevel[_currentWeaponIndex + 1] + " ----> " + f);
		return f;
	}


}
