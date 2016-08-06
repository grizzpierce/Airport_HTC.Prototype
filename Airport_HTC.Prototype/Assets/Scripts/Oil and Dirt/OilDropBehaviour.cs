using UnityEngine;
using System.Collections;

public class OilDropBehaviour : MonoBehaviour
{
    public Vector3 m_SpillLocation;
    public GameObject m_SpillPrefab;
    public GameObject newSpill;
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
        if(!m_ContinueDrip)
        {
            if (m_HasCollided == true)
                m_PS.Stop();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        //Debug.Log("Collision is hitting");

        if (m_HasCollided != true)
        {
            m_HasCollided = true;

            float newY = other.transform.position.y + (other.transform.localScale.y/2);
            newSpill = (GameObject)Instantiate(m_SpillPrefab, m_SpillLocation, Quaternion.identity);
            newSpill.transform.parent = transform.parent;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(m_SpillLocation, .1f);
    }
}
