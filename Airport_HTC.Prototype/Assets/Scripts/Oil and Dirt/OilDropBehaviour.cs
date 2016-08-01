using UnityEngine;
using System.Collections;

public class OilDropBehaviour : MonoBehaviour
{
    
    public GameObject m_SpillPrefab;
    public GameObject newSpill;
    ParticleSystem m_PS;
    bool m_HasCollided = false;

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
        if(m_HasCollided == true)
            m_PS.Stop();
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("Collision is hitting");

        if (m_HasCollided != true)
        {
            m_HasCollided = true;

            float newY = other.transform.position.y + (other.transform.localScale.y/2);
            newSpill = (GameObject)Instantiate(m_SpillPrefab, new Vector3(transform.position.x, newY, transform.position.z), Quaternion.identity);
        }
    }
}
