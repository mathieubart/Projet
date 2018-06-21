using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new player grab data", menuName = "ScriptableObjects/Player Grab Data", order = 2)]
public class PlayerGrabData : ScriptableObject
{
	[SerializeField]
	private PlayerID m_ID;
	public PlayerID ID
	{
		get { return m_ID; }
	}

	[SerializeField]
    private float m_Speed;
	public float Speed
	{
		get { return m_Speed; }
	}

	[SerializeField]
    private float m_RotationSpeed;
	public float RotationSpeed 
	{
		get { return m_RotationSpeed; }
	}

	[SerializeField]
    private float m_ThrowForce;
	public float ThrowForce 
	{
		get { return m_ThrowForce; }
	}
}
