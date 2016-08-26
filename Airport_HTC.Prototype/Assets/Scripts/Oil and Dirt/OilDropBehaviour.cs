using UnityEngine;
using System.Collections;

public class OilDropBehaviour : MonoBehaviour
{
    public Transform m_SpillLocation;
    public GameObject m_SpillPrefab;
    public GameObject newSpill;
    public Bibbits_PointOfInterest POIToActivate;
    ParticleSystem m_PS;
    bool m_HasCollided = false;
    public bool m_ContinueDrip = true;

    public bool GetIfOilSpilled()
    {
        if (newSpill != null || m_PS.isPlaying)
            return true;

        else
            return false;
    }

    public GameObject GetOilSpill()
    {
        if (newSpill != null)
            return newSpill;

        else if (m_PS.isPlaying)
            return null;

        else
            return null;
    }

    void Awake()
    {
        m_PS = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!m_ContinueDrip)
        {
            if (m_HasCollided == true)
                m_PS.Stop();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        m_HasCollided = true;

        POIToActivate.DoActivation();

        //float newY = other.transform.position.y + (other.transform.localScale.y/2);
        //newSpill = (GameObject)Instantiate(m_SpillPrefab, m_SpillLocation.position, Quaternion.identity);
        //newSpill.transform.parent = transform.parent;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(m_SpillLocation.position, .1f);
    }
}
