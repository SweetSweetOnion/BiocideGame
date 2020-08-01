using UnityEngine;
using System.Collections;

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


	
	private void Update()
	{
		float x =  Mathf.Sin(Mathf.InverseLerp(minX,maxX,controller.transform.position.x) * Mathf.PI);

		for(int i = 0; i< background.Length; i++){
			background[i].color = Color.Lerp(backColorStart[i], backColorEnd[i], x);

		}

	}

	private void OnDrawGizmos()
	{
		if (Application.isPlaying) return;
		for (int i = 0; i < background.Length; i++)
		{
			background[i].color = Color.Lerp(backColorStart[i], backColorEnd[i], f);

		}
	}
}
