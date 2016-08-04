using UnityEngine;
using System.Collections;

public class BibbitCleaner_Grabbed : MonoBehaviour {

    public Animation m_Anim;
    public VRTK_InteractableObject m_IntObj;
    public GameObject m_GrabbingController;
    public bool m_StartGrabbed = false;

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
                    transform.parent = null;
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
                    GameObject crowd = new GameObject("Bibbit Crowd");
                    crowd.transform.position = gameObject.transform.position;
                    crowd.AddComponent<BibbitCleaner_CrowdData>();
                    crowd.GetComponent<BibbitCleaner_CrowdData>().m_AddBibbit(gameObject);
                    transform.parent = crowd.transform;

                    m_Anim.Play();

                    gameObject.AddComponent<BibbitCleaner_Idle>();
                }
            }
        }
	}
}
