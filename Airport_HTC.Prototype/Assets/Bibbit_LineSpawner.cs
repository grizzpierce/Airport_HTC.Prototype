using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bibbit_LineSpawner : MonoBehaviour {

    public GameObject[] m_Bibbit;
    
    public bool m_IsPathStatic = false;
    public float m_PathfinderRate = 5;
    public float m_BibbitRate = 5;
    public int m_MaxBibbits = 10;

    private GameObject m_PathfinderObj;
    private Bibbit_Pathfinder m_Pathfinder;
    public List<GameObject> m_StoredPath = new List<GameObject>();
    public List<GameObject> m_PrevStoredPath = new List<GameObject>();

    private List<GameObject> m_SpawnedBibbits = new List<GameObject>();

    private float m_PathElapsedTime = 0;
    private float m_BibbElapsedTime = 0;
    private float m_PathTime;
    private float m_BibbTime;

    // Use this for initialization
    void Start ()
    {
        Debug.Log(m_Bibbit.Length);
        m_PathTime = Time.time;
        m_BibbTime = Time.time;

        PathfinderSetup();
        CheckPath();
    }

    private void PathfinderSetup()
    {
        m_PathfinderObj = new GameObject(gameObject.name + "'s Pathfinder");
        m_PathfinderObj.transform.position = transform.position;
        m_Pathfinder = m_PathfinderObj.AddComponent<Bibbit_Pathfinder>();
        m_PathfinderObj.AddComponent<SphereCollider>();
        m_Pathfinder.SetCastRadius(10f);
        m_Pathfinder.SetFlag(gameObject);
    }

    void Update ()
    {
        // Ability to decide if it's a static path
        if (!m_IsPathStatic)
        {
            m_PathElapsedTime = Time.time - m_PathTime;

            if (m_PathElapsedTime >= m_PathfinderRate)
            {
                m_PathTime = Time.time;
                //Debug.Log("TEN SECONDS");
                CheckPath();
            }
        }

        if (m_SpawnedBibbits.Count < m_MaxBibbits)
        {
            m_BibbElapsedTime = Time.time - m_BibbTime;

            if (m_BibbElapsedTime >= m_BibbitRate)
            { 
                //Debug.Log("Current Bibbit Count: " + (m_SpawnedBibbits.Count + 1));
                m_BibbTime = Time.time;
                GameObject newBib = (GameObject)Instantiate(m_Bibbit[(int)Random.Range(0, m_Bibbit.Length)], transform.position, Quaternion.identity);
                newBib.GetComponent<Bibbit_Movement>().SetPathNodes(m_StoredPath);
                m_SpawnedBibbits.Add(newBib);
            }
        }
	}

    private void CheckPath()
    {
        m_Pathfinder.Look();
        CloneFlags();

        if (m_PrevStoredPath.Count != 0)
            ComparePaths();
    }

    // ADDS PATHFINDERS PATH TO THE SPAWNER
    private void CloneFlags()
    {
        // CHECKS IF THERE ARE FLAGS STORED YET
        if (m_StoredPath.Count != 0)
        {
            m_PrevStoredPath.Clear();

            // IF SO, STORE THEM NOW
            for (int i = 0; i < m_StoredPath.Count; ++i)
            {
                m_PrevStoredPath.Add(m_StoredPath[i]);
                //Debug.Log(" Previous Item Added: " + m_PrevStoredPath[i]);
            }
        }

        // CLEARS THE STORED PATH AND ADDS THE NEW CURRENT PATH
        m_StoredPath.Clear();
        for (int i = 0; i < m_Pathfinder.GetCurrentPath().Count; ++i)
        {
            m_StoredPath.Add(m_Pathfinder.GetCurrentPath()[i]);
            //Debug.Log(m_StoredPath[i]);
        }
    }

    private void ComparePaths()
    {
        if (m_PrevStoredPath.Count != m_StoredPath.Count)
        {
            /*
                Destroy Bibbits
            
            Destroy(newBib);
            newBib = (GameObject)Instantiate(m_Bibbit, transform.position, Quaternion.identity);
            newBib.GetComponent<Bibbit_Movement>().SetPathNodes(m_StoredPath);
            */
        }
        else
        {
            //Debug.Log("Checking individual nodes");
        }



        // 1. Check if path is the same
        // 2. Compare current bibbits
        // 3. Delete + remove any bibbits with wrong path
    }

}
