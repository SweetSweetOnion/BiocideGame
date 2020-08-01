﻿using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class PlayerHealth : MonoBehaviour
{
	public int startHp = 3;
	public float damageCooldown = 1;

	private int _currentHp;
	private float _lastDamage = -1;

	public delegate void BasicEvent();
	public event BasicEvent OnDead;
	public delegate void Damage(int amount, Vector3 worldOrigin);
	public event Damage OnDamage;
	public int currentHp => _currentHp;

	public void ReceivedDamage(int amount, Vector3 worldOrigin){
		if (Time.time <= _lastDamage + damageCooldown) return;
		_currentHp -= amount;
		_lastDamage = Time.time;
		OnDamage?.Invoke(amount, worldOrigin);
		if(_currentHp <= 0){
			OnDead?.Invoke();
		}
        //SOUND
        AudioManager.instance.FOLEYS_Char_Hit.Post(gameObject);
        //SOUND
	}

	public void ResetHp(){
		_currentHp = startHp;
	}
}
