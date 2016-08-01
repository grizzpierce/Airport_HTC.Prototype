using UnityEngine;
using System.Collections;

public class OilData : MonoBehaviour {

    public GameObject m_OilDropPref;
    public bool m_PlayOnAwake = false;
    public bool m_Played = false;

    public GameObject m_OilDrop;
    public OilDropBehaviour m_DropBeh;
    public ParticleSystem m_DropPS;

    public bool m_OilSpilled = false;
    public GameObject m_OilSpill;

    public void Play()
    {
        if (m_DropPS != null)
            m_DropPS.Play();

        m_Played = true;
    }

    public void Stop()
    {
        if (m_DropPS != null)
        {
            DestroyDrop();
            CreateNewDrop();
        }

        m_Played = false;
        m_OilSpilled = false;
        m_OilSpill = null;
    }

    public bool GetIfActive()
    {
        if (m_Played)
        {
            if (m_OilSpill != null)
                return true;

            else if (m_DropPS.isPlaying == true)
                return true;

            else
                return true;
        }

        else
            return false;
    }


    void Awake()
    {
        CreateNewDrop();

        if (m_PlayOnAwake)
            Play();
    }

    void Update()
    {
        if (m_DropBeh.GetIfOilSpilled() && m_DropBeh.GetOilSpill() != null)
        {
            m_OilSpilled = true;
            m_OilSpill = m_DropBeh.GetOilSpill();
        }

        if (m_OilSpilled && m_DropBeh.GetOilSpill() == null)
            Stop();
    }

    private void CreateNewDrop()
    {
        m_OilDrop = (GameObject)Instantiate(m_OilDropPref, transform.position, Quaternion.Euler(Vector3.zero));
        m_OilDrop.transform.parent = gameObject.transform;

        m_DropBeh = m_OilDrop.GetComponent<OilDropBehaviour>();
        m_DropPS = m_OilDrop.GetComponent<ParticleSystem>();
    }

    private void DestroyDrop()
    {
        Destroy(m_OilDrop);
        m_DropBeh = null;
        m_DropPS = null;
    }
}
