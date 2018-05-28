﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerFlee : MonoBehaviour
{
    public float m_Speed = 10f;
    public float m_RotationSpeed = 10f;

    private bool m_IsGrabbed;

	public bool IsGrabbed
	{ 
		get{return m_IsGrabbed;} 
		set{m_IsGrabbed = value;}
	}
	
    private float m_RotationStep;
    private Vector3 m_NewDir;
    private Vector3 m_Direction;
    private Rigidbody m_Rigid;

    private void Start()
    {
        m_Direction = Vector3.zero;
        m_Rigid = GetComponent<Rigidbody>();
    }


    private void Update()
    {
		m_Direction = Vector3.zero;

        if (!m_IsGrabbed)
        {
            if (Input.GetKey(KeyCode.W))
            {
                m_Direction += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                m_Direction -= Vector3.forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                m_Direction -= Vector3.right;
            }
            if (Input.GetKey(KeyCode.D))
            {
                m_Direction += Vector3.right;
            }

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
        }
    }

    private void FixedUpdate()
    {
        if (m_Direction != Vector3.zero)
        {
            Move();
        }
    }

    public void Move()
    {
        float velocityY = m_Rigid.velocity.y;
        Vector3 forwardXZ = Vector3.zero;

        forwardXZ = transform.forward * m_Speed;
        forwardXZ.y = velocityY;
        m_Rigid.velocity = forwardXZ;
    }
}