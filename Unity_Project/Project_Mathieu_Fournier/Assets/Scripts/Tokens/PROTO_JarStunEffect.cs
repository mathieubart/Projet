using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PROTO_JarStunEffect : MonoBehaviour 
{
	private float m_EffectDuration = 3f;
	private float m_BaseSpeed;
	private Proto_PlayerControllerGrab m_PlayerGrab;

	private void Awake()
	{
		if(gameObject.GetComponent<PROTO_JarStunEffect>() != null)
		{
			if(gameObject.GetComponent<PROTO_JarStunEffect>() != this)
			{
				Destroy(this);
			}
		}
	} 

	private void Start()
	{
		m_PlayerGrab = gameObject.GetComponent<Proto_PlayerControllerGrab>();

		if(m_PlayerGrab != null)
		{
			m_BaseSpeed = m_PlayerGrab.m_Speed;
			m_PlayerGrab.SetSpeed(0f); 
			StartCoroutine("EffectTimer");
		}
		else
		{
			Destroy(this);
		}
	}

	private IEnumerator EffectTimer()
	{
		yield return new WaitForSeconds(m_EffectDuration);
		m_PlayerGrab.SetSpeed(m_BaseSpeed);
		yield return new WaitForSeconds(0.1f);
		Destroy(this);	
	}
}
