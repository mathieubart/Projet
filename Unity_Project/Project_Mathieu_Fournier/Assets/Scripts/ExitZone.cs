using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZone : MonoBehaviour 
{
	private void Start () 
	{
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("ExitZoneCollider"), LayerMask.NameToLayer("PlayerFlee"), true);
	}

	private void OnTriggerEnter(Collider aCol)
	{
		Debug.Log(aCol);
	}
}
