using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class PlayerHealth : MonoBehaviour
{
	public int startHp = 3;
	public float damageCooldown = 1;
	public bool _isInvincible = false;

	private int _currentHp;
	private float _lastDamage = -1;
	private bool _isDead;

	public delegate void BasicEvent();
	public event BasicEvent OnDead;
	public delegate void Damage(int amount, Vector3 worldOrigin);
	public event Damage OnDamage;
	public event BasicEvent OnReset;
	public int currentHp => _currentHp;
	public bool isDead => _isDead;
	private void Start()
	{
		ResetHp();
	}

	public void ReceivedDamage(int amount, Vector3 worldOrigin){
		if (_isInvincible) return;
		if (Time.time <= _lastDamage + damageCooldown) return;
		if (_isDead) return;
		_currentHp -= amount;
		_lastDamage = Time.time;
		OnDamage?.Invoke(amount, worldOrigin);
		if(_currentHp <= 0){
			_isDead = true;
            AudioManager.instance.FOLEYS_Char_Death.Post(gameObject);
            OnDead?.Invoke();
		}
        //SOUND
        AudioManager.instance.FOLEYS_Char_Hit.Post(gameObject);
        //SOUND
	}

	public void ResetHp(){
		_currentHp = startHp;
		_isDead = false;
		OnReset?.Invoke();
	}
}
