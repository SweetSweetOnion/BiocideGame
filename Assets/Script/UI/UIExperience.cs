using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIExperience : MonoBehaviour
{
	public Slider slider;
	public WeaponManager weaponManager;

	private void OnEnable()
	{
		weaponManager.OnExperienceChange += OnExpChange;
	}

	private void OnDisable()
	{
		weaponManager.OnExperienceChange -= OnExpChange;
	}

	private void OnExpChange(float newExp)
	{
		slider.value = weaponManager.GetNormalizedExperience();
	}
}
