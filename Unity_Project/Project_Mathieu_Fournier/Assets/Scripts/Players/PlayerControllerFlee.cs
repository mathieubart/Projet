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
    private PlayerFleeUI m_PlayerUI;
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
    private Vector3 m_MoveDirection;
    private Vector3 m_Offset = new Vector3(0f, 0f, 0f);
    [HideInInspector]
    public Transform m_Jar;
    private Transform m_Parent;
    private Rigidbody m_Rigid;
    private BaseEffect m_PowerUp01;
    private BaseEffect m_PowerUp02;

    //PROTO Only, To show Feedback!
    public GameObject m_MusicImage;



    private void Start()
    {
        m_Direction = Vector3.zero;
        m_MoveDirection = Vector3.zero;
        m_Jar = null;
        m_Parent = null;
        m_Rigid = GetComponent<Rigidbody>();
        m_PlayerUI.SetText(m_Points);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) && !RaycastPlayerForward())
        {
            m_MoveDirection = transform.forward * m_Speed;
  
        }
        else if(Input.GetKey(KeyCode.S))
        {
            m_MoveDirection = -transform.forward * m_Speed;
        }
        else
        {
            m_MoveDirection = Vector3.zero;
        }

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
                OnHold(m_Jar);
                m_IsInAJar = true;
            }
            else 
            {
                OnRelease();
                m_IsInAJar = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.Q) && m_PowerUp01 != null)
        {
            ActivatePowerUp(0);
            m_PowerUp01 = null;
        }

        if(Input.GetKeyDown(KeyCode.E) && m_PowerUp02 != null)
        {
            ActivatePowerUp(1);
            m_PowerUp02 = null;
        }
    }

    private void FixedUpdate()
    {
        if (!m_HisHeld && IsGrounded() && !m_IsInAJar)
        {
            Move();
        }
    }



    //Grab Tokens/Powerups or take a Jar reference if at range
    private void OnTriggerEnter(Collider aCol)
    {
        if(aCol.tag == "Token")
        {
            aCol.gameObject.SetActive(false);
            m_Points++;
            m_PlayerUI.SetText(m_Points);
        }
        else if(aCol.tag == "Jar")
        {
            m_Jar = aCol.GetComponent<Transform>();
        }
        else if(aCol.tag == "Saxophone" || aCol.tag == "Boot")
        {
            AddPowerUp(aCol.gameObject);
        }

    }

    //Remove the jar reference if he is no longer at range
    private void OnTriggerExit(Collider aCol)
    {
        if(aCol.tag == "Jar")
        {
            m_Jar = null;
        }
    }

    //Move the player forward or backward
    public void Move()
    {
        float velocityY = m_Rigid.velocity.y;

        m_MoveDirection.y = velocityY;
        m_Rigid.velocity = m_MoveDirection;
    }

    //Rotate the player to face his direction
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

    //Return true if the player touch the ground
    private bool IsGrounded()
    {
        bool isGrounded = false;
        if(!isGrounded)
        {
            isGrounded = Physics.Raycast(m_GroundRaycaster.position + new Vector3(0f, 0.2f, 0f), -transform.up, 0.53f, ~LayerMask.GetMask("PlayerGrab"));
        }
        return isGrounded;
    }

    //Return true if something is in front of the player
    private bool RaycastPlayerForward()
    {
        bool raycastPlayerForward = false;

        for (int i = 0; i < m_FrontRaycasters.Count; i++)
        {
            Ray frontRay = new Ray(m_FrontRaycasters[i].position, gameObject.transform.forward);
            if(Physics.Raycast(frontRay, 0.75f, LayerMask.NameToLayer("Default")))
            {
                raycastPlayerForward = true;
                continue;
            }
        }
        return raycastPlayerForward;
    }

    //Set the player parameters when it hide in a jar or when he is grabbed
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

    //Reset the player parameters when it is released by a player or exit the jar
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

    //Add a powerup to the player if a slot (UI Slot see **PlayerFleeUI**) is empty.
    public void AddPowerUp(GameObject aPowerUp)
    {
        if(m_PowerUp01 == null)
        {
            if(aPowerUp.tag == "Saxophone")
            {
                m_PowerUp01 = gameObject.AddComponent<SaxophoneEffect>();
                m_PlayerUI.ShowPowerUp01("Saxophone");         
            }
            else if(aPowerUp.tag == "Boot")
            {
                //TODO: 
                //m_PowerUp01 = gameObject.AddComponent<BootEffect>(); 
                //m_PlayerUI.ShowPowerUp01("Boot");        
            }

            aPowerUp.SetActive(false);
        }
        else if(m_PowerUp02 == null)
        {
            if(aPowerUp.tag == "Saxophone")
            {
                m_PowerUp02 = gameObject.AddComponent<SaxophoneEffect>();
                m_PlayerUI.ShowPowerUp02("Saxophone");            
            }
            else if(aPowerUp.tag == "Boot")
            {
                //TODO:
                //m_PowerUp02 = gameObject.AddComponent<BootEffect>();    
                //m_PlayerUI.ShowPowerUp02("Boot");     
            }

            aPowerUp.SetActive(false);
        }
    }

    //Activate a powerup if there is a powerup in the input corresponding Slot.
    private void ActivatePowerUp(int aSlot)
    {
        if(aSlot == 0)
        {
            m_PowerUp01.PlayEffect();
        }
        else if(aSlot == 1)
        {
            m_PowerUp02.PlayEffect();
        }

        m_PlayerUI.HidePowerUp(aSlot);
    }
}