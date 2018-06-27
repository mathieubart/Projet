using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour 
{
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
}
