using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerFlee : MonoBehaviour
{
    public float m_Speed = 10f;
    public float m_RotationSpeed = 10f;
    public Transform m_Raycaster;
    public PlayerFleeUI m_PointText;

    private bool m_IsGrabbed;
    private int m_Points = 0;
    private float m_RotationStep;
    private Vector3 m_NewDir;
    private Vector3 m_Direction;
    private Rigidbody m_Rigid;


    private void Start()
    {
        m_Direction = Vector3.zero;
        m_Rigid = GetComponent<Rigidbody>();
        m_PointText.SetText(m_Points);
    }

    private void Update()
    {
        SetDirection();
        Rotate();
    }

    private void FixedUpdate()
    {
        if (m_Direction != Vector3.zero)
        {
            Move();
        }
    }

    private void OnTriggerEnter(Collider aCol)
    {
        if(aCol.tag == "Token")
        {
            aCol.gameObject.SetActive(false);
            m_Points++;
            m_PointText.SetText(m_Points);
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

    public void Hide()
    {
        GetComponent<Renderer>().enabled = false;
        m_IsGrabbed = true;
    }

    public void UnHide()
    {
        GetComponent<Renderer>().enabled = true;
        m_IsGrabbed = false;
    }

    //Get Input And Set Direction
    public void SetDirection()
    {
        m_Direction = Vector3.zero;

        if(!m_IsGrabbed && IsGrounded())
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
        }
    }

    private bool IsGrounded()
    {
        bool isGrounded = false;
        if(!isGrounded)
        {
            isGrounded = Physics.Raycast(m_Raycaster.position, -transform.up, 0.52f);
        }
        return isGrounded;
    }

    private void Rotate()
    {
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