using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour 
{
	[SerializeField]
	private float m_TransitionTime;
	[SerializeField]
	private Image m_SceneTransitionImage;

	[SerializeField]
	private float m_LoadingSceneTime;

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

	private void Start()
	{
		ChangeScene(EScenes.StartMenu);
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
		StartCoroutine(FadeInScenes(a_Scene));
	}

	private void OnLoadingDone(Scene a_Scene, LoadSceneMode i_Mode)
    {
        //On enleve la fct de la liste de fct appelees par l'event OnLoadingDone de Unity.
        SceneManager.sceneLoaded -= OnLoadingDone;

		StartCoroutine(FadeOutScenes());

        switch (a_Scene.buildIndex)
        {
            case (int)EScenes.StartMenu:
                {
                    //AudioManager.Instance.SwitchMusic("XXYYXX - Rad Racer", 0.1f);
                    break;
                }            
			case (int)EScenes.MainMenu:
                {
                    //AudioManager.Instance.SwitchMusic("XXYYXX - Rad Racer", 0.1f);
                    break;
                }
			case (int)EScenes.Levels:
                {
                    //AudioManager.Instance.SwitchMusic("XXYYXX - Rad Racer", 0.1f);
                    break;
                }            
        }
    }

	private IEnumerator FadeInScenes(EScenes a_Scene)
	{
		float opacity = 0f;
		float opacityValue = 0f;
		Color color = m_SceneTransitionImage.color;

		while(opacityValue < 1)
		{
			opacity = Mathf.Lerp(0f, 1f, opacityValue);
			m_SceneTransitionImage.color = new Vector4(color.r, color.g, color.b, opacityValue);

			opacityValue +=  Time.deltaTime / m_TransitionTime;
		}

		SceneManager.LoadScene((int)a_Scene);
		SceneManager.sceneLoaded += OnLoadingDone;

		yield return null;
	}

	private IEnumerator FadeOutScenes()
	{
		float opacity = 0f;
		float opacityValue = 0f;
		Color color = m_SceneTransitionImage.color;

		while(opacityValue < 1)
		{
			opacity = Mathf.Lerp(1f, 0f, opacityValue);
			m_SceneTransitionImage.color = new Vector4(color.r, color.g, color.b, opacityValue);

			opacityValue +=  Time.deltaTime / m_TransitionTime;
		}
		yield return null;
	}
}
