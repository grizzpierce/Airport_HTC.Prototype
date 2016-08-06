using UnityEngine;
using System.Collections;

public class Boarding_Pass : MonoBehaviour {

    VRTK_InteractableObject m_IntObj;
    private bool m_GrabbedYet = false;

    void Awake ()
    {
        m_IntObj = GetComponent<VRTK_InteractableObject>();
	}

    void Start()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

	void Update ()
    {
        if(m_IntObj.IsGrabbed())
        {
            if (!m_GrabbedYet)
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                m_GrabbedYet = true;
            }
        }
	}
}
