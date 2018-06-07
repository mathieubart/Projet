using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluteEffect : MonoBehaviour 
{
	private float m_EffectDuration = 3f;
	List<PlayerControllerGrab> m_Grabbers = new List<PlayerControllerGrab>();
	List<float> m_BaseSpeed = new List<float>();



	void Start () 
	{
		SetCharactersSpeedToZero();
		StartCoroutine("FluteEffectTimer");
	}



	private IEnumerator FluteEffectTimer()
	{
		yield return new WaitForSeconds(m_EffectDuration);
		ResetCharactersSpeed();
		yield return new WaitForSeconds(0.1f);
		Destroy(this);	
	}

	private void SetCharactersSpeedToZero()
	{
		RaycastHit[] spherecastHifos;

		spherecastHifos = Physics.SphereCastAll(transform.position, 5f, transform.position, 0f, LayerMask.GetMask("PlayerGrab"));

		if(spherecastHifos.Length != 0)
		{
			m_Grabbers.Clear();
			m_BaseSpeed.Clear();
			for (int i = 0; i < spherecastHifos.Length; i++)
			{
				if(!m_Grabbers.Contains(spherecastHifos[i].collider.GetComponent<PlayerControllerGrab>()))
				{
					m_Grabbers.Add(spherecastHifos[i].collider.GetComponent<PlayerControllerGrab>());
					m_BaseSpeed.Add(m_Grabbers[i].m_Speed);
					m_Grabbers[i].SetSpeed(0f);
				}
			}
		}
	}

	private void ResetCharactersSpeed()
	{
		if(m_Grabbers != null)
		{
			for (int i = 0; i < m_Grabbers.Count; i++)
			{
				m_Grabbers[i].SetSpeed(m_BaseSpeed[i]);
			}
		}
	}
	



}
