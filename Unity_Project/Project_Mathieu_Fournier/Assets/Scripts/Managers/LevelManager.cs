using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour 
{
	private static LevelManager m_Instance;
	public static LevelManager Instance
	{
		get { return m_Instance; }
	}

	private void Awake()
	{
		if(LevelManager.Instance == null)
		{
			m_Instance = this;
		}		
		else
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			ChangeScene(EScenes.MainMenu);
		}
	}

	public void ChangeScene(EScenes a_Scene)
	{
		SceneManager.LoadScene((int)a_Scene);
	}
}
