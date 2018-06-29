using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Team
{
	public int GameScore;
	public int LevelScore;
	public GameObject Player01;
	public GameObject Player02;
}

public class TeamManager : MonoBehaviour 
{
	private Team Team01;
	private Team Team02;

	private static TeamManager m_Instance;
	public TeamManager Instance
	{
		get { return m_Instance; }
	}

	private void Awake()
	{
		if(m_Instance != null)
		{
			Destroy(this);
		}
		else
		{
			m_Instance = this;
		}
		DontDestroyOnLoad(gameObject);
	}

	public void AssignPlayer()
	{
		if(Team01.Player01 == null)
		{

			//Set The Player Control to The First Camera Quadrant.    Top-Left
		}
		else if(Team02.Player01 == null)
		{
			//Set The Player Control to The Second Camera Quadrant.   Top-Right
		}
		else if(Team01.Player02 == null)
		{
			//Set The Player Control to The Third Camera Quadrant.    Bottom-Left
		}
		else if(Team02.Player02 == null)
		{
			//Set The Player Control to The Fourth Camera Quadrant.   Bottom-Right
		}
	}

	//Assign randomly the characters to the teams players.
	public void SetRandomCharacters()
	{
		
	}

	public void SwitchCharacters()
	{
		//Activate/Desactivate The Characters to Change The Current Players Characters.  Player01 /02 /03 /04 : Flee <--> Grabber.
	}
}