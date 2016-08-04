using UnityEngine;
using System.Collections;

public class BibbitCleaner_Idle : MonoBehaviour {

    VRTK_InteractableObject m_IntObj;
    Rigidbody m_RB;

	// Use this for initialization
	void Start ()
    {
        m_RB = GetComponent<Rigidbody>();

        m_RB.constraints = RigidbodyConstraints.FreezePosition;

        if (GetComponent<BibbitCleaner_InitMove>() != null)
            Destroy(GetComponent<BibbitCleaner_InitMove>());
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
