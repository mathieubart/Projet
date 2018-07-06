﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRef : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_Runner;
	[SerializeField]
	private GameObject m_Grabber;

	public void SetActiveCharacter(bool a_IsRunnerActive)
	{
		m_Runner.SetActive(a_IsRunnerActive);
		m_Grabber.SetActive(!a_IsRunnerActive);
	}

	public void SwitchActiveCharacter()
	{
		m_Runner.SetActive(!m_Runner.activeSelf);
		m_Grabber.SetActive(!m_Grabber.activeSelf);		
	}
}