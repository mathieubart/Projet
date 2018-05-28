﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerGrab : MonoBehaviour
{
    public float m_Speed = 10f;
    public float m_RotationSpeed = 10f;
	public float m_ThrowForce = 250f;

	[SerializeField]
	private GameObject m_FalseGrabbedCharacter;
	[SerializeField]
	private GameObject m_FalseGrabbedJar;

	private bool m_HoldSomething = false;
    private float m_RotationStep;
    private Vector3 m_NewDir;
    private Vector3 m_Direction;
	private Vector3 m_GrabOffset = new Vector3(0f, 1.8f, 0f);
    private Rigidbody m_Rigid;
	private GameObject m_GrabAbleObject;



    private void Start()
    {
        m_Direction = Vector3.zero;
        m_Rigid = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        m_Direction = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_Direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            m_Direction -= Vector3.forward;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_Direction -= Vector3.right;
        }
        if (Input.GetKey(KeyCode.RightArrow))
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

		if(m_GrabAbleObject != null)
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				if(m_HoldSomething)
				{
					if(m_GrabAbleObject.name == "CharacterFlee")
					{
						m_GrabAbleObject.GetComponent<PlayerControllerFlee>().IsGrabbed = false;
						m_GrabAbleObject.SetActive(true);
						m_FalseGrabbedCharacter.SetActive(false);
					}
					m_GrabAbleObject.transform.position = transform.position + m_GrabOffset;
					m_GrabAbleObject.GetComponent<Rigidbody>().AddForce((transform.forward + transform.up) * m_ThrowForce);
					m_HoldSomething = false;
				}
				else
				{
					if(m_GrabAbleObject.name == "CharacterFlee")
					{
						m_GrabAbleObject.GetComponent<PlayerControllerFlee>().IsGrabbed = true;
						m_GrabAbleObject.SetActive(false);
						m_FalseGrabbedCharacter.SetActive(true);
					}

					m_HoldSomething = true;
				}
			}
		}
    }

    private void FixedUpdate()
    {
		if(m_Direction != Vector3.zero)
		{
        	Move();
		}
    }

	private void OnTriggerEnter(Collider aCol)
	{
		if(aCol.name == "CharacterFlee")
		{
			m_GrabAbleObject = aCol.gameObject;
		}
		else if(aCol.tag == "GrabAble" && m_GrabAbleObject.name != "CharacterFlee")
		{
			m_GrabAbleObject = aCol.gameObject;
		}
	}

	private void OnTriggerExit(Collider aCol)
	{
		if(aCol.name == "CharacterFlee")
		{
			m_GrabAbleObject = null;
		}
		else if(aCol.tag == "GrabAble" && m_GrabAbleObject.name != "CharacterFlee")
		{
			m_GrabAbleObject = null;
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