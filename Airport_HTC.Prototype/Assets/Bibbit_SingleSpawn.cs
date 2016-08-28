using UnityEngine;
using System.Collections.Generic;

public class Bibbit_SingleSpawn : MonoBehaviour {

    public GameObject m_BibbitPrefab;
    public GameObject m_CurrentBibbit;

    public List<Transform> Bibbits_Pool = new List<Transform>();
    int m_PoolIndex = 0;

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
             else if(m_CurrentBibbit.tag == "Boarding Pass")
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
        Transform newBibbit = Bibbits_Pool[m_PoolIndex];
        m_PoolIndex = (m_PoolIndex + 1) % Bibbits_Pool.Count;
        m_CurrentBibbit = newBibbit.gameObject;
        if (newBibbit.GetComponent<Rigidbody>() != null)
        {
            newBibbit.GetComponent<Rigidbody>().MovePosition(transform.position);
        }
        else
        {
            newBibbit.transform.position = transform.position;
        }
        m_SpawningStarted = false;
    }
}
