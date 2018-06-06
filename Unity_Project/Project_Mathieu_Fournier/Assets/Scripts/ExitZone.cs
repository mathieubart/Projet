using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitZone : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_GameOverImage;

	private void OnTriggerEnter(Collider aCol)
	{
		if(aCol.name == "CharacterFlee" || (aCol.transform.tag == "Jar" && aCol.GetComponent<Jar>().m_IsHiddingThePlayer))
		{
			m_GameOverImage.SetActive(true);
		}
	}
}
