using UnityEngine;
using System.Collections;

public class Bibbit_SingleSpawn : MonoBehaviour {

    public GameObject m_BibbitPrefab;
    public GameObject m_CurrentBibbit;

    public bool m_SpawningStarted = false;
    public float m_SpawnTimer;
    public float m_SpawnDelay = 1f;

	void Start ()
    {
        Spawn();
	}
	
	void Update ()
    {
        if(m_CurrentBibbit != null)
        {
            if(m_CurrentBibbit.tag == "Bibbit")
            {
                if (m_CurrentBibbit.GetComponent<VRTK_InteractableObject>().IsGrabbed())
                    m_CurrentBibbit = null;
            }

            if(m_CurrentBibbit.tag == "Boarding Pass")
                if (m_CurrentBibbit.transform.GetChild(0).GetComponent<VRTK_InteractableObject>().IsGrabbed())
                    m_CurrentBibbit = null;
        }
        else
        {
            if(!m_SpawningStarted)
            {
                m_SpawnTimer = Time.time;
                m_SpawningStarted = true;
            }

            float elapsed = Time.time - m_SpawnTimer;

            if(elapsed >= m_SpawnDelay)
            {
                Spawn();
            }
        }
	}

    void Spawn()
    {
        m_CurrentBibbit = (GameObject)Instantiate(m_BibbitPrefab, transform.position, Quaternion.identity);
        m_SpawningStarted = false;
    }
}
