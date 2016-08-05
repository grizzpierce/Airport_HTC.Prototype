using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BibbitCleaner_Spawner : MonoBehaviour {

    public GameObject[] m_BibbitTypes;
    public Vector3 m_IdleSpot;

    private GameObject m_BibbitCrowd;
    private BibbitCleaner_CrowdData m_CrowdData;

    bool m_HasSpawned = false;
    public int m_MaximumBibbits = 10;
    public float m_SquareDim = 1f; 
    private List<GameObject> m_SpawnedBibbits = new List<GameObject>();
    public List<Vector3> m_RandomLocation = new List<Vector3>();

    public AudioClip m_BibbitAudio;

	void Start ()
    {
        CreateCrowd();

        for (int i = 0; i < m_MaximumBibbits; ++i)
        {
            Vector3 temp = new Vector3(m_BibbitCrowd.transform.position.x + Random.Range(-m_SquareDim, m_SquareDim), m_BibbitCrowd.transform.position.y, m_BibbitCrowd.transform.position.z + Random.Range(-m_SquareDim, m_SquareDim));
            m_RandomLocation.Add(temp);
        }
	}
	

    private void CreateCrowd()
    {
        m_BibbitCrowd = new GameObject("Bibbit Crowd");
        m_CrowdData = m_BibbitCrowd.AddComponent<BibbitCleaner_CrowdData>();
        m_BibbitCrowd.transform.position = m_IdleSpot;
    }


	void Update ()
    {
        if (!m_HasSpawned)
        {
            SpawnBibbits();
            m_HasSpawned = true;
        }

	}

    void SpawnBibbits()
    {
        for(int i = 0; i < m_MaximumBibbits; ++i)
        {
            GameObject temp = (GameObject)Instantiate(m_BibbitTypes[(int)Random.Range(0, 4)], transform.position, Quaternion.identity);
            m_SpawnedBibbits.Add(temp);

            m_SpawnedBibbits[i].GetComponent<BibbitCleaner_InitMove>().startMarker = transform.position;
            m_SpawnedBibbits[i].GetComponent<BibbitCleaner_InitMove>().endMarker = m_RandomLocation[i];
            m_SpawnedBibbits[i].GetComponent<BibbitCleaner_InitMove>().m_Parent = m_BibbitCrowd;
            m_SpawnedBibbits[i].GetComponent<BibbitCleaner_InitMove>().m_LerpOn = true;
        }

        m_CrowdData.m_AddBibbit(m_SpawnedBibbits);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(m_IdleSpot, new Vector3(1 + m_SquareDim, 1, 1 + m_SquareDim));
    }
}
