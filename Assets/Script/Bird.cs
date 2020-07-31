using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour
{
	public float minY, maxY;
	public float minSpeed, maxSpeed;
	//public Vector2 randomDir;
	public Vector2 perlinSpeed;

	private Vector3 direction;
	private float speed;

	private SpriteRenderer sprite;
	private Animator anim;
	private Vector2 perlinPos;

	private void Awake()
	{
		sprite = GetComponent<SpriteRenderer>();
		speed = Random.Range(minSpeed, maxSpeed);
		anim = GetComponent<Animator>();
		perlinPos = new Vector2(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f));
		anim.Play("Fly", 0, Random.Range(0f, 1f));
	}


	private void Update()
	{
		Vector3 pos = transform.position;
		direction.x += Mathf.PerlinNoise(perlinPos.x,perlinPos.y) *2 -1 ;
		direction.y += Mathf.PerlinNoise(perlinPos.x +100, perlinPos.y +200) * 2 - 1;
		direction = direction.normalized;
		pos += direction * speed * Time.deltaTime;
		pos.y = Mathf.Clamp(pos.y, minY, maxY);
			
		if(Mathf.Abs(minY - pos.y) < 3 || Mathf.Abs(maxY - pos.y) < 3 ){
			if(Random.value > 0.9f){
				direction.y = Random.Range(-1,1);
			}
		}

		transform.position = pos;
		perlinPos.x += perlinSpeed.x * Time.deltaTime;
		perlinPos.y += perlinSpeed.y * Time.deltaTime;


		sprite.flipX = direction.x < 0;

		

	}
}
