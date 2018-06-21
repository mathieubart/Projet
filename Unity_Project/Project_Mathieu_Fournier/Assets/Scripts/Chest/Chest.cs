using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_Saxophone;

	//[SerializeField] Serialize when the boot will be created
	private GameObject m_Boot;

	private void OnTriggerStay(Collider a_Col)
	{
		if(a_Col.GetComponent<PlayerControllerFlee>().IsASlotEmpty())
		{
			int randomPowerUp = Random.Range(0, 2);

			if(randomPowerUp == 0)
			{
				a_Col.GetComponent<PlayerControllerFlee>().AddPowerUp(m_Saxophone);	
			}
			else
			{
				//For when the Boot will exist.
				//a_Col.GetComponent<PlayerControllerFlee>().AddPowerUp(m_Boot);			
				a_Col.GetComponent<PlayerControllerFlee>().AddPowerUp(m_Saxophone);		
			}
			Destroy(this);
		}
	}
}