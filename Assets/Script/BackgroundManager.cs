using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class BackgroundManager : MonoBehaviour
{
	public SpriteRenderer[] background;
//	public Color foreColorStart;
//	public Color foreColorEnd;
	public Color[] backColorStart;
	public Color[] backColorEnd;

	
	public float minX = 0;
	public float maxX = 100;

	public PlayerController controller;

	


	[Range(0,1),SerializeField]
	private float f = 0;


	public Tilemap[] tilemapColor;
	public Color[] tilemapStart;
	public Color[] tilemapEnd;

	public Material materialColor;
	public Color matStart;
	public Color matEnd;

	public float offset = 0;
	public Transform sunMoonRotation;

	private float currentValue = 0;
	private float sunMoonPos;
	public AnimationCurve transition;

	private void Start()
	{
		sunMoonPos = controller.transform.position.x;
	}


	private void Update()
	{
		float n = Mathf.Clamp01(Mathf.InverseLerp(minX, maxX, controller.transform.position.x));
		if (currentValue - n >0.3f )
		{
			currentValue = 0;
			sunMoonPos = controller.transform.position.x;
		}
		currentValue = Mathf.MoveTowards(currentValue, Mathf.Max(currentValue, n), Time.deltaTime * 5);


		sunMoonPos = Mathf.MoveTowards(sunMoonPos, Mathf.Max(sunMoonPos, controller.transform.position.x), Time.deltaTime * 15);

		float x =  Mathf.Abs(Mathf.Sin(Mathf.Clamp01(transition.Evaluate(currentValue)) * Mathf.PI + offset));

		sunMoonRotation.rotation = Quaternion.Euler(0, 0, -currentValue * 360);
		sunMoonRotation.transform.position = new Vector3(sunMoonPos, sunMoonRotation.position.y, sunMoonRotation.position.z);

		for(int i = 0; i< background.Length; i++){
			background[i].color = Color.Lerp(backColorStart[i], backColorEnd[i], x);

		}

		for (int i = 0; i < tilemapColor.Length; i++)
		{
			tilemapColor[i].color = Color.Lerp(tilemapStart[i], tilemapEnd[i], x);

		}

		materialColor.color = Color.Lerp(matStart, matEnd, x);

	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying) return;
		for (int i = 0; i < background.Length; i++)
		{
			background[i].color = Color.Lerp(backColorStart[i], backColorEnd[i], f);

		}
		for (int i = 0; i < tilemapColor.Length; i++)
		{
			tilemapColor[i].color = Color.Lerp(tilemapStart[i], tilemapEnd[i], f);

		}
		materialColor.color = Color.Lerp(matStart, matEnd, f);
		sunMoonRotation.rotation = Quaternion.Euler(0, 0, -f * 180);


	}
}
