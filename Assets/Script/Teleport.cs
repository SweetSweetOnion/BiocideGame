using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour
{
	public Transform targetPos;
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		var player = collision.transform.GetComponent<PlayerController>();
		if(player){
			
			TeleportSubscriber.Teleport(transform.position, targetPos.position);
		}
	
	}

	private void OnDrawGizmos()
	{
		if(targetPos)
		Debug.DrawLine(transform.position, targetPos.position);
	}
}
