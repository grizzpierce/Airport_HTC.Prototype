using UnityEngine;
using System.Collections;

public class Airport_Speakers : MonoBehaviour {

    public float m_CurrentVolume = 0;
    private AudioSource m_AudioSource;

	void Start ()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.Play();
	}
	
    public void CheckVolume(float _vol)
    {
        if (_vol > m_CurrentVolume)
        {
            m_AudioSource.volume = _vol;
            m_CurrentVolume = _vol;
        }
    }
}
