using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private bool m_CamCanGoZ = true;
    private bool m_CamCanGoX = true;


    public GameObject m_Target;
    public Vector3 m_Offset;
    public float m_Zmin = 200f, m_Zmax = 200f, m_Xmin = 100f, m_Xmax = 100f;


    private void Start()
    {
        m_Offset = transform.position - m_Target.transform.position;
        transform.position = m_Target.transform.position + m_Offset;
    }


    private void Update()
    {
        if (m_Target.transform.position.z >= m_Zmax || m_Target.transform.position.z <= m_Zmin)
        {
            m_CamCanGoZ = false;
        }
        else
        {
            m_CamCanGoZ = true;
        }

        if (m_Target.transform.position.x >= m_Xmax || m_Target.transform.position.x <= m_Xmin)
        {
            m_CamCanGoX = false;
        }
        else
        {
            m_CamCanGoX = true;
        }


        if (m_CamCanGoZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, m_Target.transform.position.z);
        }

        if (m_CamCanGoX)
        {
            transform.position = new Vector3(m_Target.transform.position.x, transform.position.y, transform.position.z);
        }

        transform.position = new Vector3(transform.position.x + m_Offset.x, m_Target.transform.position.y + m_Offset.y, transform.position.z + m_Offset.z);

    }
}
