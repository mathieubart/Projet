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

    private bool m_IsMoving = false;
    private float m_RotationStep;
    private Vector3 m_NewDir;
    private Vector3 m_Direction;
    private Rigidbody m_Rigid;
    private GameObject m_GrabAbleObject;
    private GameObject m_HeldObject;
    private List<GameObject> m_GrabablePots = new List<GameObject>();

    //MathFournier : To Be Removed, To Add FeedBack During The Prototyte
    [SerializeField]
    private GameObject m_FreezeImage;

    private void Start()
    {
        m_Direction = Vector3.zero;
        m_Rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //MathFournier : If/Else To Be Removed, Here To Add FeedBack During The Prototype;
        if(m_Speed != 0)
        {
            m_FreezeImage.SetActive(false);
        }
        else
        {
            m_FreezeImage.SetActive(true);
        }


        if (Input.GetKey(KeyCode.UpArrow) && IsNothingInFrontOfPlayer())
        {
            m_IsMoving = true;
        }
        else
        {
            m_IsMoving = false;
        }

        SetDirection();
        Rotate();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(m_HeldObject != null)
            {
                Throw();
            }
	        else if (m_GrabAbleObject != null)
            {        
                Grab();            
            }      
        }
    }

    private void FixedUpdate()
    {
        if (m_IsMoving)
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
            if(Physics.Raycast(frontRay, 0.75f, LayerMask.NameToLayer("Default")))
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

    //Spherecast to find all the pots inside grabable range. return list of pots gameobject
    private void RaycastGrabablePots()
    {
        m_GrabablePots.Clear();
        RaycastHit[] spherecastHifos;
        spherecastHifos = Physics.SphereCastAll(transform.position, 1.5f, transform.up, 0.9f * transform.localScale.y, LayerMask.GetMask("Jar"));
        
        for (int i = 0; i < spherecastHifos.Length; i++)
        {
            m_GrabablePots.Add(spherecastHifos[i].collider.gameObject);
        }
    }

    private GameObject RaycastCharacterFlee()
    {
        GameObject characterFound = null;
        RaycastHit[] spherecastHifo;
        spherecastHifo = Physics.SphereCastAll(transform.position, 1.5f, transform.up ,0.9f * transform.localScale.y, LayerMask.GetMask("PlayerFlee"));

        if(spherecastHifo.Length != 0)
        {
            characterFound = spherecastHifo[0].collider.gameObject;
        }
        return characterFound;
    }

    //Take the array of object returned by the sphere cast and assign the Best GrabableObject.
    //Priority CharacterFlee > ClosestPot.
    private GameObject GetGrabableObject()
    {
        GameObject grabableObject = null;

        grabableObject = RaycastCharacterFlee();

        if(grabableObject == null)
        {
            float closestDistance = 100000f;
            RaycastGrabablePots();

            for (int i = 0; i < m_GrabablePots.Count; i++)
            {
                if(Vector3.Magnitude(m_GrabablePots[i].transform.position - transform.position) < closestDistance)
                {
                    closestDistance = Vector3.Magnitude(m_GrabablePots[i].transform.position - transform.position);
                    grabableObject = m_GrabablePots[i];
                }
            }
        }

        return grabableObject;
    }

    private void OnTriggerEnter(Collider aCol)
    {
        //If the character hold nothing.
        //Look for the closest grabable object.
        if(m_HeldObject == null)
        {
            m_GrabAbleObject = GetGrabableObject();
        }
    }

    private void OnTriggerExit(Collider aCol)
    {
        m_GrabAbleObject = null;

        // look for the closest grabable object.      
        if (m_HeldObject == null)
        {              
             m_GrabAbleObject = GetGrabableObject();
        }
    }

    private void OnCollisionEnter(Collision aCollision)
    {
        if(aCollision.collider.tag == "Jar")
        {       
            foreach (ContactPoint point in aCollision)
            {
                if(point.point.y > 1.2f)
                {
                    gameObject.AddComponent<JarStunEffect>();
                    break;
                }
            }
        }
    }

    private void Grab()
    {
        if(m_GrabAbleObject.name == "CharacterFlee")
        {
            m_GrabAbleObject.GetComponent<PlayerControllerFlee>().OnHold(transform);
            m_GrabAbleObject.GetComponent<Renderer>().enabled = true;
            m_HeldObject = m_GrabAbleObject;
            m_GrabAbleObject = null;
        }
        else if(m_GrabAbleObject.tag == "Jar")
        {
            m_GrabAbleObject.GetComponent<Jar>().OnHold(transform);
            m_HeldObject = m_GrabAbleObject;
            m_GrabAbleObject = null;
        }
    }

    private void Throw()
    {
        float actualSpeed = Vector3.Magnitude(m_Rigid.velocity);

        if(m_HeldObject.name == "CharacterFlee")
        {
            m_HeldObject.GetComponent<PlayerControllerFlee>().OnRelease();
        }
        else if(m_HeldObject.tag == "Jar")
        {
            m_HeldObject.GetComponent<Jar>().OnRelease();
        }

        m_HeldObject.GetComponent<Rigidbody>().AddForce((transform.forward * (m_ThrowForce + (actualSpeed * 25f))) + (transform.up * m_ThrowForce));

        m_HeldObject = null;
    }

    public void SetSpeed(float a_NewSpeed)
    {
        m_Speed = a_NewSpeed;
    }
}