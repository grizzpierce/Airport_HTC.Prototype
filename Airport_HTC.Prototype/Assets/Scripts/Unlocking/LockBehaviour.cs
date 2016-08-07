using UnityEngine;
using System.Collections;

public class LockBehaviour : MonoBehaviour
{
    private GameObject m_Parent;
    private Animation m_Anim;
    private AudioSource m_Audio;
    private bool m_IsUnlocked = false;
    public GameObject[] m_UnlockLayers;

    public AnimationClip m_UnlockAnim;

    public bool m_IsUnlockAudio = true;
    public AudioClip m_UnlockAudio;

    public AudioClip m_BibbitPop;
    private GameObject m_BibbitAudio;
    private AudioSource m_BibbitAS;

    public bool GetIfUnlocked() { return m_IsUnlocked; }

    void Start()
    {
        m_Parent = gameObject.transform.parent.gameObject;
        m_Anim = m_Parent.GetComponent<Animation>();

        if (m_IsUnlockAudio)
            m_Audio = m_Parent.GetComponent<AudioSource>();
    }

	void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bibbit")
        {
            Debug.Log("Unlock!");
            m_IsUnlocked = true;

            m_BibbitAudio = (GameObject)Instantiate(new GameObject(), other.transform.position, Quaternion.identity);
            m_BibbitAS = m_BibbitAudio.AddComponent<AudioSource>();
            m_BibbitAS.clip = m_BibbitPop;
            m_BibbitAS.Play();
            Destroy(other.gameObject);
            
            if (m_IsUnlockAudio)
            {
                m_Audio.clip = m_UnlockAudio;
                m_Audio.loop = false;
                m_Audio.Play();
            }

            m_Anim.clip = m_UnlockAnim;
            m_Anim.Play();

            for (int i = 0; i < m_UnlockLayers.Length; ++i)
                m_UnlockLayers[i].layer = 0;
        }
    }
}
