using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class PlayerHealth : MonoBehaviour
{
	public int startHp = 3;

	private int _currentHp;

	public delegate void BasicEvent();
	public event BasicEvent OnDead;
	public delegate void Damage(int amount, Vector3 worldOrigin);
	public event Damage OnDamage;
	public int currentHp => _currentHp;

	public void ReceivedDamage(int amount, Vector3 worldOrigin){
		_currentHp -= amount;
		OnDamage?.Invoke(amount, worldOrigin);
		if(_currentHp <= 0){
			OnDead?.Invoke();
		}
	}

	public void ResetHp(){
		_currentHp = startHp;
	}
}
