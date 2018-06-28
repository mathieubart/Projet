using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new player grab data", menuName = "ScriptableObjects/Player Grab Data", order = 3)]
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

	[Tooltip("Angle in degrees between the forward and the wanted throw angle. Starting at the player forward going up.")]
	[SerializeField]
    private float m_ThrowAngle;
	public float ThrowAngle 
	{
		get { return m_ThrowAngle; }
	}

	[SerializeField]
    private float m_ThrowForce;
	public float ThrowForce 
	{
		get { return m_ThrowForce; }
	}
}
