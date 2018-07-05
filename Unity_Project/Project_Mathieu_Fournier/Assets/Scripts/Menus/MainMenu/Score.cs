using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour 
{
	[SerializeField]
	private Slider m_ScoreScollbarTeam01;
	public int Team01Score
	{
		get {return (int)m_ScoreScollbarTeam01.value; }
	}
	[SerializeField]
	private Slider m_ScoreScollbarTeam02;
	public int Team02Score
	{
		get {return (int)m_ScoreScollbarTeam02.value; }
	}

	public void ChangeTeam01Score(int a_ScoreToAdd)
	{
		m_ScoreScollbarTeam01.value += a_ScoreToAdd;
	}

	public void ChangeTeam02Score(int a_ScoreToAdd)
	{
		m_ScoreScollbarTeam02.value += a_ScoreToAdd;
	}
}
