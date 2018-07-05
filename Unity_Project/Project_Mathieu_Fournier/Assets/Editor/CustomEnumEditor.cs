using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomEnumEditor : EditorWindow
{
	[SerializeField]
	private ScriptableObject m_ScriptableObj;

	//Create A Menu Button (Where The File, Edit, Windows Buttons Are)
	//Call The Fucntion Below On Clic
	[MenuItem("Tools/Custom Enum Script Creator")]
	private static void Init()
	{
		//Create A Tool Tab/Window (Like Inspector, Console, Scene, Game, ...)
		EditorWindowExample example = GetWindow<EditorWindowExample>();
		example.Show();
	}

	//Used To Populate The Tab.
	private void OnGUI()
	{
		EditorGUI.BeginChangeCheck();
		m_Target = (GameObject)EditorGUILayout.ObjectField("Target", m_Target, typeof(GameObject), true);

		

		if(EditorGUI.EndChangeCheck() || (m_Target != null && m_Components == null))
		{   
			if(m_Target != null)
			{
				Transform[] allTransforms = m_Target.GetComponentsInChildren<Transform>(true);
				m_Components = new Dictionary<Transform, Component[]>();

				for(int i = 0; i < allTransforms.Length; i++)
				{
					Component[] components = allTransforms[i].GetComponents<Component>();
					m_Components.Add(allTransforms[i], components);
				}
				m_Foldouts = new bool[allTransforms.Length];
		}	}	

		EditorGUI.BeginDisabledGroup(m_Target == null);

		m_Filter = EditorGUILayout.TextField("Filter", m_Filter);
		
		EditorGUILayout.BeginHorizontal();

		GUI.color = Color.cyan;
		if(GUILayout.Button("Expand All"))
		{
			for (int i = 0; i < m_Foldouts.Length; i++)
			{
				m_Foldouts[i] = true;			
		}	}

		GUI.color = Color.magenta;
		if(GUILayout.Button("Collapse All"))
		{
			for (int i = 0; i < m_Foldouts.Length; i++)
			{
				m_Foldouts[i] = false;			
		}	}

		GUI.color = Color.white;		
		EditorGUILayout.EndHorizontal();

		EditorGUI.EndDisabledGroup();
	}
}
