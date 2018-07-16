using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControllerFlee : MonoBehaviour
{
    [SerializeField]
    private PlayerFleeData m_PlayerData;
    private PlayerID m_ID;

    private float m_Speed;
    public float Speed
    {
        get{ return m_Speed;}
    }
    private float m_RotationSpeed;

    [SerializeField]
    private Transform m_GroundRaycaster;
    [SerializeField]
    private CinemachineFreeLook m_Cinemachine;
    [SerializeField]
    private List<Transform> m_FrontRaycasters = new List<Transform>();
    [SerializeField]
    private GameObject m_TokenTrigger;
    [SerializeField]
    private GameObject m_MoneyBag;

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
    private BaseEffect[] m_PowerUps = new BaseEffect[2];

    public Action<int> OnPointChanged;
    public Action<int, PowerupType> OnPowerupAdded;
    public Action<int> OnPowerupRemoved;

    //PROTO Only, To show Feedback!
    public GameObject m_MusicImage;

    private void Awake()
    {
        if(m_PlayerData != null)
        {
            m_ID = m_PlayerData.ID;
            m_Speed = m_PlayerData.Speed;
            m_RotationSpeed = m_PlayerData.RotationSpeed;
        }
        else
        {
            Debug.LogError("You forgot to put assign a PlayerData in the inspector. Mathieu F");
        }
    }

    private void Start()
    {
        m_Direction = Vector3.zero;
        m_MoveDirection = Vector3.zero;
        m_Jar = null;
        m_Parent = null;
        m_Rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetAxis("Forward_" + m_ID.ToString()) > 0f && !RaycastPlayerForward())
        {
            m_MoveDirection = transform.forward * m_Speed;
  
        }
        else if(Input.GetAxis("Forward_" + m_ID.ToString()) < 0f)
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

        if(Input.GetButtonDown("Action_" + m_ID.ToString()) && m_Jar != null)
        {
            if(!m_IsInAJar)
            {
                m_Jar.GetComponent<Jar>().m_PlayerHidden = this;
                OnHold(m_Jar);
                m_IsInAJar = true;
            }
            else 
            {
                m_Jar.GetComponent<Jar>().m_PlayerHidden = null;
                OnRelease();
                m_IsInAJar = false;
            }
        }

        if(Input.GetButtonDown("Powerup01_" + m_ID.ToString()) && m_PowerUps[0] != null)
        {
            ActivatePowerUp(0);
        }

        if(Input.GetButtonDown("Powerup02_" + m_ID.ToString()) && m_PowerUps[1] != null)
        {
            ActivatePowerUp(1);
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
            GrowBag();

            aCol.gameObject.SetActive(false);
            m_Points++;
            OnPointChanged(m_Points);
        }
        else if(aCol.tag == "Jar")
        {
            m_Jar = aCol.GetComponent<Transform>();
        }
        else if(aCol.tag == "Chest")
        {
            if(aCol.GetComponent<Chest>() != null)
            for (int i = 0; i < m_PowerUps.Length; i++)
            {
                if(m_PowerUps[i] == null)
                {
                    aCol.GetComponent<Chest>().Loot(this, i);
                    break;
                }
            }
        }
        else if(aCol.tag == "Saxophone")
        {
            for (int i = 0; i < m_PowerUps.Length; i++)
            {
                if(m_PowerUps[i] == null)
                {
                    AddPowerUp(i, PowerupType.Saxophone);
                    aCol.gameObject.SetActive(false);
                    break;
                }
            }
        }
        /* Boots Are Not In The Game Yet. MathF
        else if(aCol.tag == "Boot")
        {
            for (int i = 0; i < m_PowerUps.Length; i++)
            {
                if(m_PowerUps[i] == null)
                {
                    AddPowerUp(i, PowerupType.Boots);
                    aCol.gameObject.SetActive(false);
                }
            }        
        }
        */
    }

    //Remove the jar reference if he is no longer at range
    private void OnTriggerExit(Collider aCol)
    {
        if(gameObject.layer == LayerMask.NameToLayer("PlayerFlee") && aCol.tag == "Jar")
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
            if (Input.GetAxis("Horizontal_" + m_ID.ToString()) < 0f)
            {
                m_Direction -= transform.right;
            }
            if (Input.GetAxis("Horizontal_" + m_ID.ToString()) > 0f)
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

    //Grow The Bag Size or reset the Size if the bool is true.
    public void GrowBag()
    {      
        if(m_MoneyBag.transform.localScale.y <= 1f)
        {
            m_MoneyBag.transform.localScale += new Vector3(0f, 0.1f, 0f); 
        }
        else if(m_MoneyBag.transform.localScale.z <= 1.5f)
        {
            m_MoneyBag.transform.localScale += new Vector3(0.1f, 0f, 0.1f); 
        }     
    }

    public void ResetBag()
    {
        m_MoneyBag.transform.localScale = new Vector3(1f, 0.5f, 1f);
        m_Points = 0;
        OnPointChanged(0);
    }

    //Set the player parameters when it hide in a jar or when he is grabbed
    public void OnHold(Transform a_Parent)
    {
        m_HisHeld = true;
        m_Rigid.isKinematic = true;
        m_Parent = a_Parent;

        gameObject.layer = LayerMask.NameToLayer("HeldPlayer");
        m_TokenTrigger.layer = LayerMask.NameToLayer("HeldPlayer");

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
        m_TokenTrigger.layer = LayerMask.NameToLayer("Token");
    }

    //Add a powerup to the player if a slot (UI Slot see **PlayerFleeUI**) is empty.
    public void AddPowerUp(int a_Slot, PowerupType a_Type)
    {
        switch (a_Type)
        {
            case PowerupType.Saxophone:
            {
                m_PowerUps[a_Slot] = gameObject.AddComponent<SaxophoneEffect>();

                OnPowerupAdded(a_Slot, PowerupType.Saxophone);
                break;
            }  
            /* No Boots In The Game Yet MathF              
            case PowerupType.Boots:
            {
                m_PowerUps[a_Slot] = gameObject.AddComponent<BootEffect>();

                OnPowerupAdded(a_Slot, PowerupType.Boots);
                break;
            }
            */  
        }
    }

    //Activate a powerup if there is a powerup in the input corresponding Slot.
    private void ActivatePowerUp(int a_Slot)
    {
        m_PowerUps[a_Slot].PlayEffect();
        m_PowerUps[a_Slot] = null;
        OnPowerupRemoved(a_Slot);
    }

    public void SetSpeed(float a_NewSpeed)
    {
        m_Speed = a_NewSpeed;
    }

    public int GetPoints()
    {
        return m_Points;
    }
}