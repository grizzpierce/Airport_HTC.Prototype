using UnityEngine;
using System.Collections;

public class BibbitCleaner_Grabbed : MonoBehaviour {

    public Animation m_Anim;
    public VRTK_InteractableObject m_IntObj;
    public GameObject m_GrabbingController;
    public bool m_StartGrabbed = false;

    public GameObject m_PrevCrowd;
    public GameObject m_NewCrowd;

    private float m_DroppedTimer;
    public float m_DropDelay = 1f;

    

	// Update is called once per frame
	void Update ()
    {
	    if (m_StartGrabbed)
        {
            if(m_IntObj.IsGrabbed())
            {
                if (GetComponent<BibbitCleaner_Idle>() != null)
                {
                    Destroy(GetComponent<BibbitCleaner_Idle>());
                    m_Anim.Stop();

                    m_PrevCrowd = transform.parent.gameObject;
                    m_PrevCrowd.GetComponent<BibbitCleaner_CrowdData>().m_BibbitGrabbed(gameObject);


                    transform.GetChild(0).transform.position = transform.position;
                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                }

                transform.position = m_GrabbingController.transform.position;
            }

            else
            {
                if(gameObject.GetComponent<Collider>().isTrigger == true)
                {
                    gameObject.GetComponent<Collider>().isTrigger = false;
                    m_DroppedTimer = Time.time;
                }

                float elapsed = Time.time - m_DroppedTimer;

                if (elapsed > m_DropDelay)
                {
                    m_NewCrowd = new GameObject("Bibbit Crowd");
                    m_NewCrowd.transform.position = gameObject.transform.position;
                    m_NewCrowd.AddComponent<BibbitCleaner_CrowdData>();
                    m_NewCrowd.GetComponent<BibbitCleaner_CrowdData>().m_AddBibbit(gameObject);
                    transform.parent = m_NewCrowd.transform;

                    m_Anim.Play();

                    gameObject.AddComponent<BibbitCleaner_Idle>();
                }
            }
        }
	}
}
