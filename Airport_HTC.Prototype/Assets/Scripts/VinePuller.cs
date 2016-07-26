using UnityEngine;
using System.Collections;

public class VinePuller : MonoBehaviour {

    VRTK_InteractableObject m_GrabbableObj;
    Rigidbody m_RB;

	// Use this for initialization
	void Start () {
        m_GrabbableObj = gameObject.GetComponent<VRTK_InteractableObject>();
        m_RB = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(m_GrabbableObj.IsGrabbed() != true)
        {
            m_RB.constraints = RigidbodyConstraints.FreezeAll;
        }

        else
        {
            m_RB.constraints = RigidbodyConstraints.None;
        }
	}
}
