using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum PlayerID
{
	PlayerOne = 1,
	PlayerTwo = 2,
	PlayerThree = 3,
	PlayerFour = 4,
}

[CreateAssetMenu(fileName = "new player flee data", menuName = "ScriptableObjects/Player Flee Data", order = 1)]
public class PlayerFleeData : ScriptableObject
{
	[SerializeField]
	private PlayerID m_ID;
	public PlayerID ID
	{
		get { return m_ID; }
	}

	[SerializeField]
    private float m_Speed = 10f;
	public float Speed
	{ 
		get { return m_Speed; }
	}

    [SerializeField]
    private float m_RotationSpeed = 10f; 
	public float RotationSpeed
	{ 
		get { return m_RotationSpeed; }
	}
}