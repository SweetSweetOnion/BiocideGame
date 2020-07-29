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

	public delegate void ExperienceChange(float newExp);
	public event ExperienceChange OnExperienceChange;


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

	private void Update()
	{
		Debug.Log(experience + "  " + GetNormalizedExperience());
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
			}
		}
		OnExperienceChange(_experience);
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
		return (_experience - experienceLevel[_currentWeaponIndex]) / experienceLevel[_currentWeaponIndex + 1];
	}


}
