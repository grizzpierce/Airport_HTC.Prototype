using UnityEngine;
using System.Collections;

public class VinePuller : MonoBehaviour {

    VRTK_InteractableObject m_GrabbableObj;
    Rigidbody m_RB;

    private AudioSource m_AudioSource;
    public AudioClip m_VinePulling;
    public AudioClip m_Idle;

	
	void Start ()
    {
        m_GrabbableObj = GetComponent<VRTK_InteractableObject>();
        m_RB = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();

        m_AudioSource.clip = m_Idle;
        m_AudioSource.Play();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(m_GrabbableObj.IsGrabbed() != true)
        {
            m_RB.constraints = RigidbodyConstraints.FreezeAll;

            m_AudioSource.Stop();
            m_AudioSource.clip = m_Idle;
            m_AudioSource.Play();
        }

        else
        {
            m_RB.constraints = RigidbodyConstraints.None;

            m_AudioSource.Stop();
            m_AudioSource.clip = m_VinePulling;
            m_AudioSource.Play();
        }
	}
}
