using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitZone : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_GameOverImage;

	private void Start () 
	{
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("ExitZoneCollider"), LayerMask.NameToLayer("PlayerFlee"), true);
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("ExitZoneCollider"), LayerMask.NameToLayer("PlayerFlee"), true);
	}

	private void OnTriggerEnter(Collider aCol)
	{
		m_GameOverImage.SetActive(true);
	}
}
