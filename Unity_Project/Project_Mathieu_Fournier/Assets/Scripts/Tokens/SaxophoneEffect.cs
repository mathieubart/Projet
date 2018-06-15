using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaxophoneEffect : BaseEffect 
{
	[HideInInspector]
	public float m_EffectDuration = 3f;
	private List<PlayerControllerGrab> m_Grabbers = new List<PlayerControllerGrab>();
	private List<float> m_BaseSpeed = new List<float>();

	//PROTO_ONLY
	private GameObject m_MusicImage;
	private void Awake()
	{
		m_MusicImage = GetComponent<PlayerControllerFlee>().m_MusicImage;
	}

	public override void PlayEffect()
	{
		SetCharactersSpeedToZero();
		m_MusicImage.SetActive(true);
		StartCoroutine("EffectTimer");
	}

	private IEnumerator EffectTimer()
	{
		yield return new WaitForSeconds(m_EffectDuration);
		ResetCharactersSpeed();
		Debug.Log(m_EffectDuration);
		m_MusicImage.SetActive(false);
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
					if(m_Grabbers[i].m_Speed != 0)
					{
						m_BaseSpeed.Add(m_Grabbers[i].m_Speed);
						m_Grabbers[i].SetSpeed(0f);
					}
					else
					{
						GetComponent<SaxophoneEffect>().m_EffectDuration *= 2;
						Destroy(this);
					}
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
