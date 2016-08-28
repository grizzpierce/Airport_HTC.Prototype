using UnityEngine;
using System.Collections;

public class TrunkController : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_Audio;

    private bool m_ControllerCollided = false;

    void OnTriggerEnter(Collider col)
    {
        if (!m_ControllerCollided && col.tag == "Controller")
        {
            m_ControllerCollided = true;
            AudioSource a = GetComponent<AudioSource>();
            if (a != null)
            {
                a.Play();
            }
            var anim = gameObject.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("Trigger_Open");
            }

        }
    }
}
