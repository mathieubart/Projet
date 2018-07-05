using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

[System.Serializable]
public enum EScenes
{
	StartMenu = 1,
	MainMenu = 2,
	Level = 3,
}

[CreateAssetMenu(menuName = "ScriptableObjects/Scenes Data ", fileName = "new Scenes Data", order = 3)]
public class ScenesData : ScriptableObject
{
	[SerializeField]
	private EScenes m_Scenes;
}