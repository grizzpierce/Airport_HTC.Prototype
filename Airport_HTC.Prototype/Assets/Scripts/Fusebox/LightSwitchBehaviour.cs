using UnityEngine;
using System.Collections;

public class LightSwitchBehaviour : MonoBehaviour {

    public AnimationClip m_OnAnim;
    public AnimationClip m_OffAnim;
    private VRTK_InteractableObject m_SwitchObj;
    private Animation m_Anim;
    private bool m_AlreadyTouched = false;
    private bool m_LightOn = false;
    private bool m_IsLocked = false;

    private AudioSource m_Audio;

    public bool GetIfSwitchOn() { return m_LightOn; }

    public void SetIfLocked(bool _locked)
    {
        m_IsLocked = _locked;
    }

    void Start()
    {
        m_SwitchObj = gameObject.GetComponent<VRTK_InteractableObject>();
        m_Anim = gameObject.GetComponent<Animation>();
        m_Audio = gameObject.GetComponent<AudioSource>();

        m_Anim.clip = m_OffAnim;
        m_Anim.Play();
    }

    void Update()
    {
        if (!m_IsLocked)
        {
            if (m_SwitchObj.IsTouched() && m_AlreadyTouched != true)
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

            else if (m_SwitchObj.IsTouched() != true)
            {
                m_AlreadyTouched = false;
            }
        }
    }
}
