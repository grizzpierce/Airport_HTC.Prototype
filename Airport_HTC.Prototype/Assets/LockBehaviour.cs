using UnityEngine;
using System.Collections;

public class LockBehaviour : MonoBehaviour {

    public AudioSource m_Audio;
    public AnimationClip m_UnlockAnim;
    public AudioClip m_UnlockAudio;

    private AnimationClip newAnim;
    private GameObject m_Parent;
    private Animation m_Anim;
    private bool m_IsUnlocked = false;
    
    public bool GetIfUnlocked() { return m_IsUnlocked; }

    void Start()
    {
        m_Parent = gameObject.transform.parent.gameObject;
        m_Anim = m_Parent.GetComponent<Animation>();
    }

	void OnTriggerEnter(Collider other)
    {
        Debug.Log("test");
        if (other.gameObject.tag == "Bibbit")
        {
            Debug.Log("Unlock!");
            m_IsUnlocked = true;
            Destroy(other.gameObject);

            /* UNLOCK POLISH FX

            m_Audio.clip = m_UnlockAudio;
            m_Audio.loop = false;
            m_Audio.Play();
            */

            m_Anim.clip = m_UnlockAnim;
            m_Anim.Play();
            

        }
    }
}
