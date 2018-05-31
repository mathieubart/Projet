using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerFleeUI : MonoBehaviour 
{
	private TextMeshProUGUI m_PointTextMesh;
	
	private void Awake()
	{
		m_PointTextMesh = GetComponent<TextMeshProUGUI>();
	}

	public void SetText(int aNumber)
	{
		m_PointTextMesh.text = aNumber.ToString();
	}
}
