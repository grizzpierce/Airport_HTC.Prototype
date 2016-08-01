using UnityEngine;
using System.Collections;

public class TeleportShellBehaviour : MonoBehaviour
{
    public bool IsOffOnStart = false;
    GameObject m_LightShaft;
    GameObject m_TeleportShaft;

    private Light m_Light;
    private ParticleSystem[] m_Particles = new ParticleSystem[2];

    public Color m_InactiveColor;
    public Color m_ActiveColor;

    public Material m_InactiveMat;
    public Material m_ActiveMat;

    public Vector3 TeleportPoint;

    private bool m_isAudioPrepping = false;
    private AudioSource m_Audio;
    private float m_Timer = 0;
    private float m_Delay;

    public float m_SpeakerVolume;

    public Vector3 GetTelePoint() { return TeleportPoint; }


    public void PlaySound() { m_Audio.Play(); }

    public void PlaySound(float _delay)
    {
        m_Delay = _delay;
        m_isAudioPrepping = true;
    }

    public float GetVolume()
    {
        return m_SpeakerVolume;
    }


    void Update()
    {
        if (m_isAudioPrepping)
        {
            m_Timer += .1f;

            if (m_Timer > m_Delay)
                m_Audio.Play();
        }

        if (m_Audio.isPlaying)
            m_isAudioPrepping = false;
    }




    void Awake()
    {
        m_Audio = GetComponent<AudioSource>();
        m_LightShaft = transform.FindChild("prop_lightshaft").gameObject;
        m_TeleportShaft = transform.FindChild("teleportshaft").gameObject;
        m_Light = transform.FindChild("Spotlight").GetComponent<Light>();

        m_Particles[0] = transform.FindChild("volumetric").GetComponent<ParticleSystem>();
        m_Particles[1] = transform.FindChild("volumetric (1)").GetComponent<ParticleSystem>();
    }

    void Start()
    {
        if (IsOffOnStart)
        {
            m_TeleportShaft.GetComponent<BoxCollider>().enabled = false;
            m_LightShaft.GetComponent<MeshRenderer>().enabled = false;
            m_LightShaft.GetComponent<MeshCollider>().enabled = false;
            m_Light.enabled = false;

            for (int i = 0; i < m_Particles.Length; ++i)
            {
                m_Particles[i].Stop();
            }
        }
    }

    public void IsActive(bool _active)
    {
        if (_active == true)
        {
            m_TeleportShaft.GetComponent<BoxCollider>().enabled = true;
            m_LightShaft.GetComponent<MeshRenderer>().enabled = true;
            m_LightShaft.GetComponent<MeshCollider>().enabled = true;
            m_Light.enabled = true;

            for (int i = 0; i < m_Particles.Length; ++i)
            {
                m_Particles[i].Stop();
            }
        }

        else
        {
            m_TeleportShaft.GetComponent<BoxCollider>().enabled = false;
            m_LightShaft.GetComponent<MeshRenderer>().enabled = false;
            m_LightShaft.GetComponent<MeshCollider>().enabled = false;
            m_Light.enabled = false;

            for (int i = 0; i < m_Particles.Length; ++i)
            {
                m_Particles[i].Play();
            }
        }
    }

    public void Highlight(bool _isHighlighted)
    {
        if (_isHighlighted == true)
        {
            m_LightShaft.GetComponent<MeshRenderer>().material = m_ActiveMat;
            m_Light.color = m_ActiveColor;
        }

        else
        {
            m_LightShaft.GetComponent<MeshRenderer>().material = m_InactiveMat;
            m_Light.color = m_InactiveColor;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(GetTelePoint(), new Vector3(2.5f,1f,2f));
    }
}