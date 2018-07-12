using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Team
{
	public float GameScore; 
	public int LevelScore;
	public Player Player01;
	public Player Player02;
}

public class TeamManager : MonoBehaviour 
{
	private List<Team> m_Teams = new List<Team>();

	private static TeamManager m_Instance;
	public static TeamManager Instance
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

	public void AddPlayer(int a_ID, Player a_Player)
	{
		switch (a_ID)
		{
			case 1:
			{
				m_Teams[0].Player01 = a_Player;
				break;
			}			
			case 2:
			{
				m_Teams[1].Player01 = a_Player;
				break;
			}
			case 3:
			{
				m_Teams[0].Player02 = a_Player;
				break;
			}			
			case 4:
			{
				m_Teams[1].Player02 = a_Player;
				break;
			}
		}
	}


	//Assign a random character to the teams players.
	public void SetRandomCharacters()
	{
		bool runnerIsActive = Random.Range(0, 2) == 0;
		for(int i = 0; i < m_Teams.Count; i++)
		{
			m_Teams[i].Player01.gameObject.SetActive(true);
			m_Teams[i].Player02.gameObject.SetActive(true);	
			m_Teams[i].Player01.SetActiveCharacter(runnerIsActive);
			m_Teams[i].Player02.SetActiveCharacter(!runnerIsActive);

			runnerIsActive = !runnerIsActive;
		}
	}

	//Activate/Desactivate The Characters to Change The Current Players Characters.  Player01 /02 /03 /04 : Flee <--> Grabber.
	public void SwitchCharacters()
	{
		for(int i = 0; i < m_Teams.Count; i++)
		{
			m_Teams[i].Player01.gameObject.SetActive(true);
			m_Teams[i].Player02.gameObject.SetActive(true);	
			m_Teams[i].Player01.SwitchActiveCharacter();
			m_Teams[i].Player02.SwitchActiveCharacter();			
		}
	}

	//Set Unactive All characters in the Scene
	public void DesactivateCharacters()
	{
		for(int i = 0; i < m_Teams.Count; i++)
		{
			m_Teams[i].Player01.gameObject.SetActive(false);
			m_Teams[i].Player02.gameObject.SetActive(false);		
		}
	}

	public float GetGameScore(int a_Team) 
	{
		return m_Teams[a_Team].GameScore;
	}

	public void ModifyGameScore(int a_Team, float a_Addition)
	{
		m_Teams[a_Team].GameScore += a_Addition;
	}

	public int GetLevelScore(int a_Team) 
	{		
		return m_Teams[a_Team].LevelScore;
	}

	public void ModifyLevelScore(int a_Team, int a_Addition)
	{
		m_Teams[a_Team].LevelScore += a_Addition;	
	}

	public void ResetLevelScores()
	{
		for(int i = 0; i < m_Teams.Count; i++)
		{
			m_Teams[i].LevelScore = 0;	
		}
	}

	public void AddTeam()
	{
		m_Teams.Add(new Team());
	}

	public PlayerControllerFlee GetPlayerFlee(int a_ID)
	{
		switch (a_ID)
		{
			case 1:
			{
				return m_Teams[0].Player01.GetPlayerFlee();
			}			
			case 2:
			{
				return m_Teams[1].Player01.GetPlayerFlee();
			}
			case 3:
			{
				return m_Teams[0].Player02.GetPlayerFlee();
			}			
			case 4:
			{
				return m_Teams[1].Player02.GetPlayerFlee();
			}
			default:
			{
				return null;				
			}
		}
	}

	public void AssignPlayer() //Mathieu Fournier: Used to set Dynamically the players in one of the Screen Quadrant.
	{
		if(m_Teams[0].Player01 == null)
		{
			//Set The Player Control to The First Camera Quadrant.    Top-Left
		}
		else if(m_Teams[1].Player01 == null)
		{
			//Set The Player Control to The Second Camera Quadrant.   Top-Right
		}
		else if(m_Teams[0].Player02 == null)
		{
			//Set The Player Control to The Third Camera Quadrant.    Bottom-Left
		}
		else if(m_Teams[1].Player02 == null)
		{
			//Set The Player Control to The Fourth Camera Quadrant.   Bottom-Right
		}
	}
}