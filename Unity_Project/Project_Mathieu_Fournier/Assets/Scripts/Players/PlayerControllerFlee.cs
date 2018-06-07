using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControllerFlee : MonoBehaviour
{
    [SerializeField]
    private float m_Speed = 10f;
    [SerializeField]
    private float m_RotationSpeed = 10f;
    [SerializeField]
    private Transform m_GroundRaycaster;
    [SerializeField]
    private PlayerFleeUI m_PointText;
    [SerializeField]
    private CinemachineFreeLook m_Cinemachine;
    [SerializeField]
    private List<Transform> m_FrontRaycasters = new List<Transform>();

    [HideInInspector]
    public bool m_HisHeld {get; set;}
    [HideInInspector]
    public bool m_IsInAJar {get; set;}
    private int m_Points = 0;
    private float m_RotationStep;
    private Vector3 m_NewDir;
    private Vector3 m_Direction;
    private Vector3 m_Offset = new Vector3(0f, 0f, 0f);
    [HideInInspector]
    public Transform m_Jar;
    private Transform m_Parent;
    private Rigidbody m_Rigid;



    private void Start()
    {
        m_Direction = Vector3.zero;
        m_Jar = null;
        m_Parent = null;
        m_Rigid = GetComponent<Rigidbody>();
        m_PointText.SetText(m_Points);
    }

    private void Update()
    {
        if(m_Parent != null)
        {
            transform.position = m_Parent.transform.position + m_Offset;
            transform.rotation = m_Parent.rotation;
        }
        else
        {
            SetDirection();
            Rotate();
        }

        if(Input.GetKeyDown(KeyCode.F) && m_Jar != null)
        {
            if(!m_IsInAJar)
            {
               // GetInsideJar();
                OnHold(m_Jar);
                m_IsInAJar = true;
            }
            else 
            {
               // GetOutsideJar();
                OnRelease();
                m_IsInAJar = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_Direction != Vector3.zero && !RaycastPlayerForward())
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
        else if(aCol.tag == "Jar")
        {
            m_Jar = aCol.GetComponent<Transform>();
        }
        else if(aCol.tag == "Saxophone")
        {
            gameObject.AddComponent<FluteEffect>();
            aCol.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider aCol)
    {
        if(aCol.tag == "Jar")
        {
            m_Jar = null;
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

    //Get Input And Set Direction
    public void SetDirection()
    {
        m_Direction = Vector3.zero;

        if(!m_HisHeld && IsGrounded() && !m_IsInAJar)
        {
            if (Input.GetKey(KeyCode.W))
            {
                m_Direction += transform.forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                m_Direction -= transform.right;
            }
            if (Input.GetKey(KeyCode.D))
            {
                m_Direction += transform.right;
            }
        }
    }

    private bool IsGrounded()
    {
        bool isGrounded = false;
        if(!isGrounded)
        {
            isGrounded = Physics.Raycast(m_GroundRaycaster.position + new Vector3(0f, 0.2f, 0f), -transform.up, 0.53f);
        }
        return isGrounded;
    }

    private bool RaycastPlayerForward()
    {
        bool raycastPlayerForward = false;

        for (int i = 0; i < m_FrontRaycasters.Count; i++)
        {
            Ray frontRay = new Ray(m_FrontRaycasters[i].position, gameObject.transform.forward);
            if(Physics.Raycast(frontRay, 0.75f))
            {
                raycastPlayerForward = true;
                continue;
            }
        }
        return raycastPlayerForward;
    }

    public void OnHold(Transform a_Parent)
    {
        m_HisHeld = true;
        m_Rigid.isKinematic = true;
        m_Parent = a_Parent;

        gameObject.layer = LayerMask.NameToLayer("HeldPlayer");

        if(a_Parent.name == "CharacterGrab")
        {
            m_Offset = new Vector3(0f, 1.75f, 0f);
        }
        else if(a_Parent.tag == "Jar")
        {
            m_Offset = new Vector3(0f, 0f, 0f);   
            GetComponent<Renderer>().enabled = false;  
            a_Parent.GetComponent<Jar>().m_IsHiddingThePlayer = true;       
        }
    }

    public void OnRelease()
    {
        if(m_Parent.tag == "Jar")
        {
            transform.position = m_Parent.position + new Vector3(0f, 1f, 0f);
            m_Parent.GetComponent<Jar>().m_IsHiddingThePlayer = false;
        }

        GetComponent<Renderer>().enabled = true;

        m_HisHeld = false;
        m_Rigid.isKinematic = false;
        m_Parent = null;

        gameObject.layer = LayerMask.NameToLayer("PlayerFlee");
    }
}