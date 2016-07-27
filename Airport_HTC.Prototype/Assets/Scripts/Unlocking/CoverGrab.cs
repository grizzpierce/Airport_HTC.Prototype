using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoverGrab : MonoBehaviour {

    public AnimationClip m_CoverAnim;
    public GameObject m_Lock;

    private bool m_AlreadyTouched = false;
    private VRTK_InteractableObject m_IntObj;
    private Animation m_Anim;

    private float m_DelayTimer;
    private float m_Elapsed;
    public float m_DelayAmount = 1;
    private bool m_TimerStarted;

    private AudioSource m_Audio;
    public AudioClip m_SwingOpen;

    void Start()
    {
        m_IntObj = GetComponent<VRTK_InteractableObject>();
        m_Anim = transform.parent.gameObject.GetComponent<Animation>();
        m_Audio = transform.parent.gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (m_Lock.GetComponent<LockBehaviour>().GetIfUnlocked())
        {
            if (!m_Anim.isPlaying)
            {
                if (!m_TimerStarted)
                {
                    m_DelayTimer = Time.time;
                    m_TimerStarted = true;
                }

                m_Elapsed = Time.time - m_DelayTimer;

                if (m_Elapsed < m_DelayAmount)
                {
                    m_Elapsed = Time.time - m_DelayTimer;
                }

                else
                {
                    if (m_IntObj.IsTouched() && m_AlreadyTouched != true)
                    {
                        Debug.Log("Touched");
                        m_AlreadyTouched = true;

                        m_Anim.clip = m_CoverAnim;
                        m_Anim.Play();

                        m_Audio.clip = m_SwingOpen;
                        m_Audio.Play();
                    }
                }
            }
        }
    }
}
