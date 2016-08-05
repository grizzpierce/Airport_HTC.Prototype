using UnityEngine;
using System.Collections;

public class BibbitCleaner_Follow : MonoBehaviour {

    public GameObject m_LeadBibbit;
    public BibbitCleaner_Grabbed m_Grabbed;
    public Vector3 m_FollowPos;

    void Start()
    {
        if (GetComponent<BibbitCleaner_Idle>() != null)
        {
            transform.GetChild(0).transform.position = transform.position;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

            m_FollowPos = new Vector3(Random.Range(-.1f, .1f), Random.Range(-.1f, .1f), Random.Range(-.1f, .1f));

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
                transform.position = FollowLocation();

            else
            {
                transform.position = FollowLocation();

                if (m_Grabbed.m_NewCrowd != null)
                {
                    if (m_Grabbed.m_NewCrowd.GetComponent<BibbitCleaner_CrowdData>())
                    {
                        m_Grabbed.m_NewCrowd.GetComponent<BibbitCleaner_CrowdData>().m_AddBibbit(gameObject);
                        transform.parent = m_Grabbed.m_NewCrowd.transform;
                        transform.position = new Vector3(m_Grabbed.m_NewCrowd.transform.position.x + Random.Range(-.25f, .5f), m_Grabbed.m_NewCrowd.transform.position.y, m_Grabbed.m_NewCrowd.transform.position.z + Random.Range(-.25f, .25f));
                        transform.GetChild(0).GetComponent<Animation>().Play();
                        gameObject.AddComponent<BibbitCleaner_Idle>();
                    }
                }
            }
        }
    }

    private Vector3 FollowLocation() { return new Vector3(m_LeadBibbit.transform.position.x + m_FollowPos.x, m_LeadBibbit.transform.position.y + m_FollowPos.y, m_LeadBibbit.transform.position.z + m_FollowPos.z); }

}
