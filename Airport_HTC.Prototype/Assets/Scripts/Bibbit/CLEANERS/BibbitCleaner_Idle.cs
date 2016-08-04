using UnityEngine;
using System.Collections;

public class BibbitCleaner_Idle : MonoBehaviour {

    VRTK_InteractableObject m_IntObj;
    Rigidbody m_RB;
    Animation m_Anim;
    bool m_Grabbed;

	// Use this for initialization
	void Start ()
    {
        m_RB = GetComponent<Rigidbody>();
        m_IntObj = GetComponent<VRTK_InteractableObject>();
        m_Anim = transform.GetChild(0).GetComponent<Animation>();

        m_RB.constraints = RigidbodyConstraints.FreezePosition;

        if (GetComponent<BibbitCleaner_InitMove>() != null)
            Destroy(GetComponent<BibbitCleaner_InitMove>());

        if (GetComponent<BibbitCleaner_Grabbed>() != null)
            Destroy(GetComponent<BibbitCleaner_Grabbed>());

        if (GetComponent<BibbitCleaner_Follow>() != null)
            Destroy(GetComponent<BibbitCleaner_Follow>());

        if (GetComponent<Collider>().isTrigger == false)
            GetComponent<Collider>().isTrigger = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(m_IntObj.IsGrabbed())
        {
            gameObject.AddComponent<BibbitCleaner_Grabbed>();
            GetComponent<BibbitCleaner_Grabbed>().m_IntObj = m_IntObj;
            GetComponent<BibbitCleaner_Grabbed>().m_GrabbingController = m_IntObj.GetGrabbingObject();
            GetComponent<BibbitCleaner_Grabbed>().m_Anim = m_Anim;
            GetComponent<BibbitCleaner_Grabbed>().m_StartGrabbed = true;
        }
	}
}
