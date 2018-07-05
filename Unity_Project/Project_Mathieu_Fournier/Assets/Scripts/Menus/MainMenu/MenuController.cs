using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour 
{
	[SerializeField]
	private TextMeshProUGUI m_PressStartText;

	private void Awake()
	{
		m_PressStartText.enabled = false;
	}

	public void ShowText()
	{
		m_PressStartText.enabled = true;
	}
}
