using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LineFlag : MonoBehaviour {

    GameObject m_Sign;
    List<Transform> m_FlagOptions = new List<Transform>();
    bool m_IsGrounded = false;

    public bool GetIfObjGrabbed()
    {
        return transform.FindChild("Sign").GetComponent<VRTK_InteractableObject>().IsGrabbed();
    }

    public bool GetIfGrounded()
    {
        return transform.FindChild("Sign").gameObject.transform.FindChild("Ground Collider").GetComponent<GroundCollider>().GetIsGrounded();
    }

    void Start()
    {
        if (gameObject.name != "SPAWN")
        {
            m_Sign = transform.FindChild("Sign").gameObject;

            for (int i = 0; i < m_Sign.transform.childCount; ++i)
            {
                if (m_Sign.transform.GetChild(i).name != "Ground Collider" || m_Sign.transform.GetChild(i).name != "floorlight")
                {
                    //Debug.Log(m_Sign.transform.name + "'s " + m_Sign.transform.GetChild(i).name);
                    m_FlagOptions.Add(m_Sign.transform.GetChild(i));
                }
            }
        }

        else
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                if (transform.GetChild(i).name != "Sign")
                {
                    //Debug.Log(transform.name + "'s " + transform.GetChild(i).name);
                    m_FlagOptions.Add(transform.GetChild(i));
                }
            }
        }


    }


    public Transform GetRandomFlagTransform()
    {
        return m_FlagOptions[(int)Random.Range(0, m_FlagOptions.Count)];
    }
}
