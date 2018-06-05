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
    private GameObject m_HeldObject;
    private List<GameObject> m_GrabablePots = new List<GameObject>();

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
    }

    private void FixedUpdate()
    {
        if (m_Direction != Vector3.zero && IsNothingInFrontOfPlayer())
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

    public void Grab()
    {
        if (m_GrabAbleObject.name == "CharacterFlee")
        {
            m_GrabAbleObject.GetComponent<PlayerControllerFlee>().Hide(gameObject.transform, gameObject.layer);
            m_GrabAbleObject.GetComponent<PlayerControllerFlee>().m_IsGrabbed = true;
            m_FalseGrabbedCharacter.SetActive(true);

            m_HeldObject = m_GrabAbleObject;
        }
        else if (m_GrabAbleObject.tag == "Jar")
        {
            m_GrabAbleObject.GetComponent<Renderer>().enabled = false;
            m_FalseGrabbedJar.SetActive(true);

            if(m_PlayerFlee.GetComponent<PlayerControllerFlee>().m_IsInAJar)
            {
                m_PlayerFlee.GetComponent<PlayerControllerFlee>().SetCameraTarget(gameObject.transform);
                m_PlayerFlee.GetComponent<PlayerControllerFlee>().m_Jar = m_FalseGrabbedJar.transform;
            }

            m_HeldObject = m_GrabAbleObject;
        }
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

        m_HeldObject = null;
    }

    //Spherecast to find all the pots inside grabable range. return list of pots gameobject
    private void RaycastGrabablePots()
    {
        m_GrabablePots.Clear();
        RaycastHit[] spherecastHifos;
        spherecastHifos = Physics.SphereCastAll(transform.position, 2f, transform.position, 0f, LayerMask.GetMask("Jar"));
        
        for (int i = 0; i < spherecastHifos.Length; i++)
        {
            m_GrabablePots.Add(spherecastHifos[i].collider.gameObject);
            Debug.Log(m_GrabablePots[i]);
        }
    }


    //Take the array of object returned by the sphere cast and assign the closest pot.
    private GameObject GetClosestPot()
    {
        GameObject closestPot = null;
        float closestDistance = 100000f;
        RaycastGrabablePots();

        for (int i = 0; i < m_GrabablePots.Count; i++)
        {
            if(Vector3.Magnitude(m_GrabablePots[i].transform.position - transform.position) < closestDistance)
            {
                closestDistance = Vector3.Magnitude(m_GrabablePots[i].transform.position - transform.position);
                closestPot = m_GrabablePots[i];
            }
        }

        return closestPot;
    }

    private void OnTriggerEnter(Collider aCol)
    {
        //If the character hold nothing && the fleeing character isnt in range.
        //Look for the closest grabable object.
        if(m_HeldObject == null)
        {
            if (aCol.name == "CharacterFlee")
            {
                m_GrabAbleObject = aCol.gameObject;
            }
            else if (aCol.name != "CharacterFlee" && aCol.tag == "Jar")
            {
                m_GrabAbleObject = GetClosestPot();
            }
        }
    }

    private void OnTriggerExit(Collider aCol)
    {
        //If the character hold nothing && the fleeing character isnt in range.
        // look for the closest grabable object.      
        if (m_HeldObject == null)
        {
            if (m_GrabAbleObject != null && m_GrabAbleObject.name != "CharacterFlee")
            {
                m_GrabAbleObject = GetClosestPot();
            }
        }
    }
}
