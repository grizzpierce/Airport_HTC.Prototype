using UnityEngine;
using System.Collections;

public class BibbitCleaner_Follow : MonoBehaviour {

    public GameObject m_LeadBibbit;
    public BibbitCleaner_Grabbed m_Grabbed;


    void Start()
    {
        if (GetComponent<BibbitCleaner_Idle>() != null)
        {
            transform.GetChild(0).transform.position = transform.position;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

            Destroy(GetComponent<BibbitCleaner_Idle>());
            transform.GetChild(0).GetComponent<Animation>().Stop();
        }
    }

	void Update ()
    {
	    if(m_LeadBibbit != null)
        {
            if(m_Grabbed == null)
                m_Grabbed = m_LeadBibbit.GetComponent<BibbitCleaner_Grabbed>();


            if (m_Grabbed.m_IntObj.IsGrabbed())
                transform.position = m_LeadBibbit.transform.position;

            else
            {
                if (m_Grabbed.m_NewCrowd != null)
                {
                    if (m_Grabbed.m_NewCrowd.GetComponent<BibbitCleaner_CrowdData>())
                    {
                        m_Grabbed.m_NewCrowd.GetComponent<BibbitCleaner_CrowdData>().m_AddBibbit(gameObject);
                        transform.parent = m_Grabbed.m_NewCrowd.transform;
                        transform.position = new Vector3(m_Grabbed.m_NewCrowd.transform.position.x + Random.Range(-1f, 1f), m_Grabbed.m_NewCrowd.transform.position.y, m_Grabbed.m_NewCrowd.transform.position.z + Random.Range(-1f, 1f));
                        transform.GetChild(0).GetComponent<Animation>().Play();
                        gameObject.AddComponent<BibbitCleaner_Idle>();
                    }
                }

            }
        }
    }
}
