using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartMenu : MonoBehaviour 
{
	private bool m_PlayerOneReady;
	private bool m_PlayerTwoReady;
	private bool m_PlayerThreeReady;
	private bool m_PlayerFourReady;

	[SerializeField]
	private List<Text> m_StartTexts = new List<Text>();
	[SerializeField]
	private List<Image> m_StartImage = new List<Image>();
	

	private void Awake()
	{
		for (int i = 1; i < m_StartTexts.Count; i++)
		{
			m_StartTexts[i].GetComponent<Animator>().enabled = false;
		}

	}

	void Update () 
	{
		if(!m_PlayerFourReady)
		{
			if(Input.GetButtonDown("Action_PlayerOne"))
			{
				Debug.Log("ONE");
				m_PlayerOneReady = true;
				m_StartTexts[0].text = "Ready!";
			}
			
			if(m_PlayerOneReady && Input.GetButtonDown("Action_PlayerTwo"))
			{
				Debug.Log("Two");
				m_PlayerTwoReady = true;
				m_StartTexts[1].text = "Ready!";				
			}
			
			if(m_PlayerTwoReady && Input.GetButtonDown("Action_PlayerThree"))
			{
				m_PlayerThreeReady = true;
				m_StartTexts[2].text = "Ready!";
			}
			
			if(m_PlayerThreeReady && Input.GetButtonDown("Action_PlayerFour"))
			{
				m_PlayerFourReady = true;
				m_StartTexts[3].text = "Ready!";
			}

			if(m_PlayerFourReady)
			{
				//Unload the Start Menu.
				//Load the Main Menu.
			}
		}
	}
}
