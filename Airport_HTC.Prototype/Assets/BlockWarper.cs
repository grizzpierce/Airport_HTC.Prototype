using UnityEngine;
using System.Collections;

public class BlockWarper : MonoBehaviour {

    VRTK_InteractableObject m_IntObj;
    VRTK_SimplePointer m_CurrentController;

	// Use this for initialization
	void Start ()
    {
        m_IntObj = GetComponent<VRTK_InteractableObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (m_IntObj.IsGrabbed())
        {
            m_CurrentController = m_IntObj.GetGrabbingObject().GetComponent<VRTK_SimplePointer>();
            m_CurrentController.enabled = false;
        }

        else
        {
            if(m_CurrentController != null)
            {
                m_CurrentController.enabled = true;
                m_CurrentController = null;
            }
        }

	}

    void OnDestroy()
    {
        if (m_CurrentController != null)
        {
            m_CurrentController.enabled = true;
            m_CurrentController = null;
        }
    }

}
