using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	public float m_Speed = 10f;
	public float m_RotationSpeed = 10f;

	private bool m_InputIsDown;
	private Vector3 m_Direction;
	private Rigidbody m_Rigid;

	private void Start () 
	{
		m_Direction = Vector3.zero;
		m_Rigid = GetComponent<Rigidbody>();
	}
	

	private void Update () 
	{
		m_Direction = Vector3.zero;
		m_InputIsDown = false;

		if(Input.GetKey(KeyCode.W))
		{
			m_Direction += Vector3.forward;
			m_InputIsDown = true;
		}
		if(Input.GetKey(KeyCode.S))
		{
			m_Direction -= Vector3.forward;
			m_InputIsDown = true;
		}
		if(Input.GetKey(KeyCode.A))
		{
			m_Direction -= Vector3.right;
			m_InputIsDown = true;
		}
		if(Input.GetKey(KeyCode.D))
		{
			m_Direction += Vector3.right;
			m_InputIsDown = true;
		}	

		if(m_InputIsDown)
		{
			/***ROTATION***
            m_RotationStep = m_RotationSpeed * Time.deltaTime;
            m_NewDir = Vector3.RotateTowards(transform.forward, m_Direction, m_RotationStep, 0.0f);

            if (m_Direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(m_NewDir, transform.up);
            }
            else
            {
                m_Rigid.angularVelocity = Vector3.zero;
            }
			*/
		}
	}

	private void FixedUpdate()
	{
		if(m_InputIsDown)
		{
		Move();
		}
	}

	public void Move()
	{
		float velocityY = m_Rigid.velocity.y;
		Vector3 forwardXZ = transform.forward;
		forwardXZ.y = velocityY;
		m_Rigid.velocity = forwardXZ;
	}
}
