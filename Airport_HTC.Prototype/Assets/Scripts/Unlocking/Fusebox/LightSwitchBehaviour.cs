using UnityEngine;
using System.Collections;

public class LightSwitchBehaviour : MonoBehaviour {

    public AnimationClip m_OnAnim;
    public AnimationClip m_OffAnim;
    private Animation m_Anim;
    public bool m_AlreadyTouched = false;
    public bool m_LightOn = false;
    public bool m_IsLocked = false;
    public bool m_ControllerCollided = false;

    private AudioSource m_Audio;

    public bool GetIfSwitchOn() { return m_LightOn; }

    public void SetIfLocked(bool _locked)
    {
        m_IsLocked = _locked;
    }

    void Start()
    {
        m_Anim = gameObject.GetComponent<Animation>();
        m_Audio = gameObject.GetComponent<AudioSource>();

        m_Anim.clip = m_OffAnim;
        m_Anim.Play();
    }

    void Update()
    {
        if (!m_IsLocked)
        {

            if (m_ControllerCollided && m_AlreadyTouched != true)
            {
                m_AlreadyTouched = true;

                if (m_LightOn == false)
                {
                    m_Anim.clip = m_OnAnim;
                    m_Anim.Play();
                    m_Audio.Play();
                    m_LightOn = true;
                }

                else
                {
                    m_Anim.clip = m_OffAnim;
                    m_Anim.Play();
                    m_Audio.Play();
                    m_LightOn = false;
                }
            }

            /* FUNCTIONALITY TO TURN SWITCH BACK OFF

            else if (m_ControllerCollided != true)
            {
                m_AlreadyTouched = false;
            }
            */
        }
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Controller")
        {
            m_ControllerCollided = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Controller")
        {
            m_ControllerCollided = false;
        }
    }
}
