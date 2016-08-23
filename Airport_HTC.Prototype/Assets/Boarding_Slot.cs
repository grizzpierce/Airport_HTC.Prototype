using UnityEngine;
using System.Collections;

public class Boarding_Slot : MonoBehaviour {

    public GameObject m_CameraRig;
    public GameObject m_BoardingPass;
    private float m_GrabbedTimer;
    private bool m_Inserted = false;
    private bool m_TimerStarted = false;
    public float m_FadeDelay = 1f;
    public float m_FadeRate = 1f;
    private bool m_Fading = false;
    [SerializeField] DemoEnd _demoEnd;
	// Update is called once per frame
	void Update ()
    {
        if (m_Inserted)
        {
            m_BoardingPass.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            if (!m_TimerStarted)
            {
                m_GrabbedTimer = Time.time;
                m_TimerStarted = true;
            }

            float elapsed = Time.time - m_GrabbedTimer;

            if (elapsed >= m_FadeDelay && !m_Fading)
            {
                //NEIL uncomment this to go back to previous behavior
                //SteamVR_Fade.View(Color.black, m_FadeRate);
                //NEIL comment this to go back to previous behavior
                _demoEnd.EndDemo();


                m_CameraRig.GetComponent<TeleportToObject>().enabled = false;
                m_Fading = true;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Boarding Pass")
        {
            if(!m_Inserted)
            {
                m_BoardingPass = col.gameObject;
                m_Inserted = true;
            }
                
        }
    }
}
