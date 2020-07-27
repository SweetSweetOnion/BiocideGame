using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{

	public PlayerController player;
	private Camera cam;
	public Vector2 speed = new Vector2(100,5);

	private Vector3 targetPos;

	private void OnEnable()
	{
		player.OnTeleport += Player_onTeleport;
	}

	private void OnDisable()
	{
		player.OnTeleport -= Player_onTeleport;
	}

	private void Player_onTeleport(Vector2 old, Vector2 newPos)
	{
		Vector2 pos = targetPos;
		Vector2 dir = pos - old;
		Vector2 dir2 = transform.position - targetPos;
		targetPos = newPos + dir;
		transform.position = new Vector3(targetPos.x + dir2.x, targetPos.y + dir2.y,transform.position.z);
		
		

	}

	// Use this for initialization
	void Start()
	{
		SetCamToTarget();
	}

	// Update is called once per frame
	void LateUpdate()
	{
		targetPos.z = transform.position.z;

		if (player.transform.position.x > targetPos.x) targetPos.x = player.transform.position.x;

		if(player.isGrounded){
			targetPos.y = player.transform.position.y;
		}

		Vector3 v = transform.position;
		v.x = Mathf.MoveTowards(v.x, targetPos.x, Time.deltaTime * speed.x);
		v.y = Mathf.MoveTowards(v.y, targetPos.y, Time.deltaTime * speed.y);
		transform.position = v;
		//transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed)
		}

	private void SetCamToTarget(){
		targetPos = player.transform.position;
	}
}
