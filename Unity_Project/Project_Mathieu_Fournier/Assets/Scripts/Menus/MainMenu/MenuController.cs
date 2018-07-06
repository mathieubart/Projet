using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuController : MonoBehaviour 
{
	[SerializeField]
	private float m_WinningGameScore = 50f;

	[SerializeField]
	private TextMeshProUGUI m_PressStartText;

	private bool m_IsDistributingPoints = false;
	[SerializeField]
	private float m_DistributionTime = 5f;
	[SerializeField]
	private ParticleSystem m_ParticleST01; //Particle System Team 01.
	[SerializeField]
	private ParticleSystem m_ParticleST02; //Particle System Team 02.

	[SerializeField]
	private Image m_WinTeam01;
	[SerializeField]
	private Image m_WinTeam02;
	[SerializeField]
	private Image m_LooseTeam01;
	[SerializeField]
	private Image m_LooseTeam02;


	private List<int> m_LevelScores = new List<int>();


	private void Awake()
	{
		m_PressStartText.enabled = false;
		m_ParticleST01.Stop();
		m_ParticleST02.Stop();
	}

	private void Start()
	{
		GetTeamsPoint();

		if(m_LevelScores[0] == 0 && m_LevelScores[1] == 0)
		{
			ShowText();
		}
		else
		{
			TeamManager.Instance.ResetLevelScores();
			StartCoroutine(DistributePoints());
		}	
	}

	private void Update()
	{
		if(m_PressStartText.enabled && Input.GetButtonDown("Action_PlayerOne"))
		{
			LevelManager.Instance.ChangeScene(EScenes.Levels);
		}

		if(TeamManager.Instance.GetGameScore(0) >= m_WinningGameScore)
		{
			StopCoroutine(DistributePoints());


		}	
		else if(TeamManager.Instance.GetGameScore(1) >= m_WinningGameScore)
		{
			StopCoroutine(DistributePoints());


		}
	}

	private void ShowText()
	{
		m_PressStartText.enabled = true;
	}

	private void GetTeamsPoint()
	{
		m_LevelScores.Add(TeamManager.Instance.GetLevelScore(0));
		m_LevelScores.Add(TeamManager.Instance.GetLevelScore(1));
	}

	private IEnumerator DistributePoints()
	{
		//Value used to Lerp and Stop the SFX of the Teams at the End of the Distribution Time.
		float team01Value = m_LevelScores[0]; 
		float team02Value = m_LevelScores[1]; 

		//Change The Teams Value To a 0 -> 1 base. 1 = the highest level score.
		if(team01Value < team02Value)
		{
			team01Value = (1f / (team02Value / team01Value));
			team02Value = 0f; 
		}
		else if(team02Value > team01Value)
		{
			team02Value = (1f / (team01Value / team02Value));
			team01Value = 0f;
		}
		else
		{
			team01Value = 0f;
			team02Value = 0f;			
		}

		yield return new WaitForSeconds(1f); //Delay Before The Distribution Start.

		m_ParticleST01.Play();
		m_ParticleST02.Play();

		while(team01Value <= m_DistributionTime || team02Value <= m_DistributionTime)
		{
			if(team01Value <= 1f)
			{
				TeamManager.Instance.ModifyGameScore(0, Time.deltaTime * m_LevelScores[0]);
				team01Value += Time.deltaTime / m_DistributionTime;
			}
			else
			{
				m_ParticleST01.Stop();
			}

			if(team02Value <= 1f)
			{
				TeamManager.Instance.ModifyGameScore(0, Time.deltaTime * m_LevelScores[1]);				
				team02Value += Time.deltaTime / m_DistributionTime;
			}
			else
			{
				m_ParticleST02.Stop();
			}
		}

		ShowText();

		yield return null;
	}
}
