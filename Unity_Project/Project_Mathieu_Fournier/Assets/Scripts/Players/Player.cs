using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	[SerializeField]
	private PlayerID m_ID;
	[SerializeField]
	private GameObject m_Runner;
	[SerializeField]
	private GameObject m_Grabber;
	[SerializeField]
	private GameObject m_PlayerUI;

	private void Awake()
	{
		if(TeamManager.Instance != null)
		{
			TeamManager.Instance.AddPlayer( (int)m_ID, this);
		}
	}

	public void SetActiveCharacter(bool a_IsRunnerActive)
	{
		m_Runner.SetActive(a_IsRunnerActive);
		m_Grabber.SetActive(!a_IsRunnerActive);
		m_PlayerUI.SetActive(a_IsRunnerActive);
	}

	public void SwitchActiveCharacter()
	{
		m_Runner.SetActive(!m_Runner.activeSelf);
		m_Grabber.SetActive(!m_Grabber.activeSelf);		
		m_PlayerUI.SetActive(m_Runner.activeSelf);		
	}

	public PlayerControllerFlee GetPlayerFlee()
	{
		return m_Runner.GetComponent<PlayerControllerFlee>();
	}
}