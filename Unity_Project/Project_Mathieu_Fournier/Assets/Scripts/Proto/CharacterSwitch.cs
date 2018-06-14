using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSwitch : MonoBehaviour 
{
	[SerializeField]
	private GameObject[] m_ProtoGrabbersObjects;

	[SerializeField]
	private GameObject[] m_CharacterFleeObjects;

	private bool m_SwitchToggle;

	private Button m_Button; 

	private void Awake()
	{
		m_Button = GetComponent<Button>();
		m_Button.onClick.AddListener(ButtonClic);
	}

	private void ButtonClic()
	{
		m_SwitchToggle = !m_SwitchToggle;

		for (int i = 0; i < m_ProtoGrabbersObjects.Length; i++)
		{
			m_ProtoGrabbersObjects[i].SetActive(m_SwitchToggle);
		}
		
		for (int i = 0; i < m_CharacterFleeObjects.Length; i++)
		{		
			m_CharacterFleeObjects[i].SetActive(!m_SwitchToggle);
		}
	}
}
