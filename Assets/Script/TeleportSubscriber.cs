using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeleportSubscriber : MonoBehaviour
{
	public static List<TeleportSubscriber> tpList = new List<TeleportSubscriber>();

	public static void Teleport(Vector3 from, Vector3 to){
		foreach(TeleportSubscriber tp in tpList){
			Vector3 dir = tp.transform.position - from;

			var player = tp.GetComponent<PlayerController>();
			if(player){
				player.SetPosition(to + dir);
			}else{
				tp.transform.position = to + dir;
			}
			
		}
	}

	private void OnEnable()
	{
		tpList.Add(this);
	}

	private void OnDisable()
	{
		tpList.Remove(this);
	}
}
