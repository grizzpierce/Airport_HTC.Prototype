using UnityEngine;
using System.Collections;

public class CoverGrab : MonoBehaviour {

    public AnimationClip m_CoverAnim;
    public GameObject m_Lock;

    private bool m_AlreadyTouched = false;
    private VRTK_InteractableObject m_IntObj;
    private Animation m_Anim;
    

    void Start()
    {
        m_IntObj = GetComponent<VRTK_InteractableObject>();
        m_Anim = transform.parent.gameObject.GetComponent<Animation>();

    }


    void Update()
    {
        if (m_Lock.GetComponent<LockBehaviour>().GetIfUnlocked())
        {
            if (m_IntObj.IsTouched() && m_AlreadyTouched != true)
            {
                Debug.Log("Touched");
                m_AlreadyTouched = true;

                m_Anim.clip = m_CoverAnim;
                m_Anim.Play();
            }
        }
    }
}
