using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerGrab : MonoBehaviour
{
    public float m_Speed = 10f;
    public float m_RotationSpeed = 10f;
    public float m_ThrowForce = 250f;
    [SerializeField]
    private List<Transform> m_FrontRaycasters = new List<Transform>();
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
    private GameObject m_PlayerFlee;

    private void Start()
    {
        m_Direction = Vector3.zero;
        m_Rigid = GetComponent<Rigidbody>();
        m_PlayerFlee = GameObject.Find("CharacterFlee");
    }

    private void Update()
    {
        SetDirection();

        Rotate();

        if (Input.GetKeyDown(KeyCode.Space))
        {
			Action();
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
        if (aCol.name == "CharacterFlee")
        {
            m_GrabAbleObject = aCol.gameObject;
        }
        else if (aCol.name != "CharacterFlee" && aCol.tag == "Jar")
        {
            m_GrabAbleObject = aCol.gameObject;
        }
    }

    private void OnTriggerExit(Collider aCol)
    {
        if (!m_HoldSomething)
        {
            if (m_GrabAbleObject != null && m_GrabAbleObject.name != "CharacterFlee" && aCol.tag == "Jar")
            {
                m_GrabAbleObject = null;
            }
            else if (aCol.name == "CharacterFlee")
            {
                m_GrabAbleObject = null;
            }
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

    //GetAxis And Set The direction
    public void SetDirection()
    {
        m_Direction = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_Direction += transform.forward;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_Direction -= transform.right;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_Direction += transform.right;
        }
    }

    public void Rotate()
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

    public void Action()
    {
        if (m_GrabAbleObject != null)
        {
            if (m_HoldSomething)
            {
                Throw();
            }
            else
            {
                Grab();
            }
        }
    }

    public void Grab()
    {
        if (m_GrabAbleObject.name == "CharacterFlee")
        {
            m_GrabAbleObject.GetComponent<PlayerControllerFlee>().Hide(gameObject.transform, gameObject.layer);
            m_GrabAbleObject.GetComponent<PlayerControllerFlee>().m_IsGrabbed = true;
            m_FalseGrabbedCharacter.SetActive(true);
        }
        else if (m_GrabAbleObject.tag == "Jar")
        {
            m_GrabAbleObject.GetComponent<Renderer>().enabled = false;
            if(m_PlayerFlee.GetComponent<PlayerControllerFlee>().m_IsInAJar)
            {
                m_PlayerFlee.GetComponent<PlayerControllerFlee>().SetCameraTarget(gameObject.transform);
                m_PlayerFlee.GetComponent<PlayerControllerFlee>().m_Jar = m_FalseGrabbedJar.transform;
            }
            m_FalseGrabbedJar.SetActive(true);
        }

        m_HoldSomething = true;
    }

    public void Throw()
    {
		float actualSpeed = Vector3.Magnitude(m_Rigid.velocity);

        if (m_GrabAbleObject.name == "CharacterFlee")
        {
            m_GrabAbleObject.GetComponent<PlayerControllerFlee>().UnHide();
            m_GrabAbleObject.GetComponent<PlayerControllerFlee>().m_IsGrabbed = false;
            m_FalseGrabbedCharacter.SetActive(false);
        }
        else if (m_GrabAbleObject.tag == "Jar")
        {
            m_GrabAbleObject.GetComponent<Renderer>().enabled = true;
            if(m_PlayerFlee.GetComponent<PlayerControllerFlee>().m_IsInAJar)
            {
                m_PlayerFlee.GetComponent<PlayerControllerFlee>().SetCameraTarget(m_GrabAbleObject.transform);
                m_PlayerFlee.GetComponent<PlayerControllerFlee>().m_Jar = m_GrabAbleObject.transform;
            }
            m_FalseGrabbedJar.SetActive(false);
        }

        m_GrabAbleObject.transform.position = transform.position + m_GrabOffset;
        m_GrabAbleObject.GetComponent<Rigidbody>().AddForce((transform.forward * (m_ThrowForce + (actualSpeed*25f))) + (transform.up * m_ThrowForce));

        m_HoldSomething = false;
    }
}
