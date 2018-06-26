using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jar : MonoBehaviour 
{
	[HideInInspector]
	public bool m_IsHiddingThePlayer = false;
	private bool test = false;
	

	private void FixedUpdate()
	{
		if(test)
		{
			Debug.Log(GetComponent<Rigidbody>().velocity);
		}
	}

	public void OnHold(Transform a_Parent)
	{
        //GetComponent<Rigidbody>().isKinematic = true;
        gameObject.layer = LayerMask.NameToLayer("HeldJar");		
		gameObject.AddComponent<FollowGrabber>();
		GetComponent<FollowGrabber>().SetParent(a_Parent);
	}

	public void OnRelease()
	{
		Debug.Log("Release");
		test = true;
		//GetComponent<Rigidbody>().isKinematic = false;
        gameObject.layer = LayerMask.NameToLayer("Jar");
		Destroy(gameObject.GetComponent<FollowGrabber>());
	}
}
