using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Truck : MonoBehaviour 
{
	[SerializeField]
	[Range(0, 1)]
	private int m_TeamAssigned;

	private void OnTriggerEnter(Collider aCol)
	{	
		if(aCol.name == "CharacterFlee" )
		{
			int points = aCol.GetComponent<PlayerControllerFlee>().GetPoints();
			TeamManager.Instance.ModifyLevelScore(m_TeamAssigned, points);
			aCol.GetComponent<PlayerControllerFlee>().ResetBag();
		}
		else if(aCol.transform.tag == "Jar" && aCol.GetComponent<Jar>().m_IsHiddingThePlayer)
		{
			int points = aCol.GetComponent<Jar>().m_PlayerHidden.GetPoints();
			TeamManager.Instance.ModifyLevelScore(m_TeamAssigned, points);			
		}
	}
}
