using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerFleeUI : MonoBehaviour 
{
	private TextMeshProUGUI m_PointTextMesh;
	
	[SerializeField]
	private GameObject m_UISax01;
	[SerializeField]
	private GameObject m_UISax02;
	[SerializeField]
	private GameObject m_UIBoot01;
	[SerializeField]
	private GameObject m_UIBoot02;
	
	private void Awake()
	{
		m_PointTextMesh = GetComponent<TextMeshProUGUI>();
	}

	public void SetText(int aNumber)
	{
		m_PointTextMesh.text = aNumber.ToString();
	}

	public void ShowPowerUp01(string aPowerUp)
	{
		if(aPowerUp == "Saxophone")
		{
			m_UISax01.SetActive(true);
		}
		else if(aPowerUp == "Boot")
		{
			m_UIBoot01.SetActive(true);
		}
	}

	public void ShowPowerUp02(string aPowerUp)
	{
		if(aPowerUp == "Saxophone")
		{
			m_UISax02.SetActive(true);
		}
		else if(aPowerUp == "Boot")
		{
			m_UIBoot02.SetActive(true);
		}
	}

	public void HidePowerUp(int aSlot)
	{
		if(aSlot == 0)
		{
			m_UISax01.SetActive(false);
			//m_UIBoot01.SetActive(false);			
		}
		else if(aSlot == 1)
		{
			m_UISax02.SetActive(false);
			//m_UIBoot02.SetActive(false);	
		}
	}
}
