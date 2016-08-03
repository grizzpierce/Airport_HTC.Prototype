using UnityEngine;
using System.Collections;

public class Boarding_Pass : MonoBehaviour {

    VRTK_InteractableObject m_IntObj;

    private float m_GrabbedTimer;
    private bool m_GrabbedYet = false;
    private bool m_TimerStarted = false;
    public float m_FadeDelay = 1f;
    public float m_FadeRate = 1f;

    void Awake ()
    {
        m_IntObj = GetComponent<VRTK_InteractableObject>();
	}

	void Update ()
    {
        if(m_IntObj.IsGrabbed())
        {
            if (!m_GrabbedYet)
                m_GrabbedYet = true;
        }

        else
        {
            if (m_GrabbedYet)
            {
                if(!m_TimerStarted)
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
	}
}
