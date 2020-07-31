using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class BackpackPressure : MonoBehaviour
{
	public WeaponController weaponController;
	public PlayerController playerController;
	private new Light light;

	private Vector3 localPos;
	private float maxLight;
	private void Awake()
	{
		light = GetComponent<Light>();
		localPos = transform.localPosition;
		maxLight = light.intensity;
	}

	private void Update()
	{
		if(playerController.isFlip){
			transform.localPosition = new Vector3(-localPos.x, localPos.y, localPos.z);
		}else{
			transform.localPosition = localPos;
		}

		light.intensity = Mathf.Lerp(0, maxLight, weaponController.currentWeapon.GetPressure(weaponController.pressure) * weaponController.currentWeapon.GetPressure(weaponController.pressure));
	}


}
