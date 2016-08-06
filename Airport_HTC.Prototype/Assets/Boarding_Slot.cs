using UnityEngine;
using System.Collections;

public class Boarding_Slot : MonoBehaviour {

    private float m_GrabbedTimer;
    private bool m_Inserted = false;
    private bool m_TimerStarted = false;
    public float m_FadeDelay = 1f;
    public float m_FadeRate = 1f;
	
	// Update is called once per frame
	void Update ()
    {
        if (m_Inserted)
        {
            if (!m_TimerStarted)
            {
                m_GrabbedTimer = Time.time;
                m_TimerStarted = true;
            }

            float elapsed = Time.time - m_GrabbedTimer;

            if (elapsed >= m_FadeDelay)
            {
                SteamVR_Fade.View(Color.black, m_FadeRate);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Boarding Pass")
        {
            if(!m_Inserted)
                m_Inserted = true;
        }
    }
}
