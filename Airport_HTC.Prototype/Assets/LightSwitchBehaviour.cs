using UnityEngine;
using System.Collections;

public class LightSwitchBehaviour : MonoBehaviour {

    private VRTK_InteractableObject m_SwitchObj;
    private Rigidbody m_RB;
    bool m_AlreadyTouched = false;

    void Start()
    {
        m_SwitchObj = gameObject.GetComponent<VRTK_InteractableObject>();
        m_RB = gameObject.GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (m_SwitchObj.IsTouched() && m_AlreadyTouched != true)
        {
            m_AlreadyTouched = true;
            m_RB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        }

        else
        {
            m_AlreadyTouched = false;
            m_RB.constraints = RigidbodyConstraints.FreezeAll;
        }
    }


}
