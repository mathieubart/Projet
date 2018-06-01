using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControllerFlee : MonoBehaviour
{
    public float m_Speed = 10f;
    public float m_RotationSpeed = 10f;
    public Transform m_GroundRaycaster;
    public PlayerFleeUI m_PointText;
    public CinemachineFreeLook m_Cinemachine;
    [SerializeField]
    private List<Transform> m_FrontRaycasters = new List<Transform>();

    [HideInInspector]
    public bool m_IsGrabbed {get; set;}
    [HideInInspector]
    public bool m_IsInAJar {get; set;}
    private int m_Points = 0;
    private float m_RotationStep;
    private Vector3 m_NewDir;
    private Vector3 m_Direction;
    [HideInInspector]
    public Transform m_Jar;
    private Rigidbody m_Rigid;
    private LayerMask m_CollisionIgnoreLayer;


    private void Start()
    {
        m_Direction = Vector3.zero;
        m_Jar = null;
        m_Rigid = GetComponent<Rigidbody>();
        m_PointText.SetText(m_Points);
    }

    private void Update()
    {
        SetDirection();
        Rotate();
        if(Input.GetKeyDown(KeyCode.F) && m_Jar != null)
        {
            if(!m_IsInAJar)
            {
                GetInsideJar();
                m_IsInAJar = true;
            }
            else 
            {
                Debug.Log("FFFF");
                GetOutsideJar();
                m_IsInAJar = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_Direction != Vector3.zero && IsNothingInFrontOfPlayer())
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

    private bool IsNothingInFrontOfPlayer()
    {
        bool isNothingInFrontOfPlayer = true;

        for (int i = 0; i < m_FrontRaycasters.Count; i++)
        {
            Ray frontRay = new Ray(m_FrontRaycasters[i].position, gameObject.transform.forward);
            if(Physics.Raycast(frontRay, 0.75f))
            {
                isNothingInFrontOfPlayer = false;
                continue;
            }
        }
        return isNothingInFrontOfPlayer;
    }

    public void Hide(Transform a_cameraTarget, LayerMask a_CollisionIgnore)
    {
        m_CollisionIgnoreLayer = a_CollisionIgnore;
        GetComponent<Renderer>().enabled = false;
        Physics.IgnoreLayerCollision(gameObject.layer, m_CollisionIgnoreLayer, true);
        SetCameraTarget(a_cameraTarget);
    }

    public void SetCameraTarget(Transform a_cameraTarget)
    { 
        if(a_cameraTarget != null)
        {
            m_Cinemachine.m_Follow = a_cameraTarget;
            m_Cinemachine.m_LookAt = a_cameraTarget;
        }
    }

    public void UnHide()
    {
        GetComponent<Renderer>().enabled = true;
        Physics.IgnoreLayerCollision(gameObject.layer, m_CollisionIgnoreLayer, false);

        m_Cinemachine.m_Follow = gameObject.transform;
        m_Cinemachine.m_LookAt = gameObject.transform;     
    }

    //Get Input And Set Direction
    public void SetDirection()
    {
        m_Direction = Vector3.zero;

        if(!m_IsGrabbed && IsGrounded() && !m_IsInAJar)
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
            isGrounded = Physics.Raycast(m_GroundRaycaster.position, -transform.up, 0.52f);
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

    private void GetInsideJar()
    {
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("PlayerGrab"), true);
        Hide(m_Jar, m_Jar.gameObject.layer);
    }

    private void GetOutsideJar()
    {
        transform.position = m_Jar.position + Vector3.up;
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("PlayerGrab"), false);
        UnHide();
    }

}