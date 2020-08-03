using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "new weapon", menuName = "Biocide/weapons", order = 0)]
public class Weapon : ScriptableObject
{
	[Header("Bullet")]
	public Bullet bulletPrefab;
	public float maxBulletSpeed = 10;
	public float minBulletSpeed = 10;
	public float bulletDamage = 10;
	public float bulletGravity = -9;
	public float bulletLifetime = 10;
	public int bulletLevel = 0;
	[Header("Spawn")]
	public Vector2 maxSpawnCount = new Vector2(1, 2);
	public Vector2 minSpawnCount = new Vector2(1, 2);
	public float maxSpawnCooldown = 0.1f;
	public float minSpawnCooldown = 1;
	public float directionSpread = 30;
	public float directionOffsetY = 2;
	public Vector2 spawnOffset;
	public Vector2 recoil;
	public float recoilDuration;
	[Header("Pressure")]
	public float pressureAddPerUse = -10f;
	public float pressureAddEverySecond = -1f;
	public float pressureAddPerReload = 2f;
	public AnimationCurve pressureCurve;
	[Space]
	public string levelUpText;



	public void SpawnBullet(Vector3 position, Vector2 direction, Vector3 playerVel, float pressure)
	{
		float p = GetPressure(pressure);
		Vector2 spawnCount = Vector2.Lerp(minSpawnCount, maxSpawnCount, p );
		int random = (int) Random.Range(spawnCount.x, spawnCount.y);
		float bulletSpeed = Mathf.Lerp(minBulletSpeed, maxBulletSpeed, p);
		for(int i = 0; i < random; i++){
			Vector2 baseDirection = direction + Vector2.up * directionOffsetY;
			
			Vector2 finalDir = Vector2.ClampMagnitude(Rotate(baseDirection,Random.Range(-directionSpread,directionSpread) * Mathf.Deg2Rad) , 1);

			Bullet bullet = Instantiate(bulletPrefab, position + new Vector3(spawnOffset.x * Mathf.Sign(direction.x), spawnOffset.y, 0) , Quaternion.identity);
			bullet.speed = bulletSpeed + Mathf.Abs(playerVel.x/10);
			bullet.direction = finalDir;
			bullet.gravity = bulletGravity;
			bullet.lifetime = bulletLifetime;
			bullet.weaponLevel = bulletLevel;
			bullet.damage = bulletDamage;
		}
	}

	public float GetPressure(float pressure){
		float p = pressure / 100f;
		return pressureCurve.Evaluate(p);
	}

	public float GetSpawnCooldown(float pressure){
		return Mathf.Lerp(minSpawnCooldown, maxSpawnCooldown, GetPressure(pressure));
	}

	public static Vector2 Rotate(Vector2 v, float angleInRadians)
	{
		return new Vector2(v.x * Mathf.Cos(angleInRadians) - v.y * Mathf.Sin(angleInRadians),
						  v.x * Mathf.Sin(angleInRadians) + v.y * Mathf.Cos(angleInRadians));
	}

}
