using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bibbit_LineSpawner : MonoBehaviour {


    // CUSTOMIZABLE PARTS OF SPAWNER
    public GameObject[] m_BibbitTypes;
    public bool m_IsPathStatic = false;
    public float m_PathfinderRate = 5;
    public bool m_SpawningActive = true;
    public GroupPathFollowing PathLogic;

    // CUSTOMIZABLE PARTS OF BIBBIT CREATION
    public float m_BibbitRate = 5;
    public int m_MaxBibbits = 10;

    [Range(0, 3)]
    public int m_Priority; // NOTE: FULLY IMPLEMENT PRIORITY BASED SYSTEMS


    // VARIABLES FOR PATHFINDER SYSTEM
    private GameObject m_PathfinderObj;
    private Bibbit_Pathfinder m_Pathfinder;
    private List<GameObject> m_StoredPath = new List<GameObject>();
    private List<GameObject> m_PrevStoredPath = new List<GameObject>();

    // VARIABLES FOR BIBBIT CREATION SYSTEM
    private List<GameObject> m_SpawnedBibbits = new List<GameObject>();
    private WaitForSeconds m_BibbitsRateWait;

    // SETS IF SPAWNING IS ACTIVE
    public void SetIfSpawningActive(bool _isSpawning)
    {
        m_SpawningActive = _isSpawning;
    }

    // ADDS BIBBIT TO CONTAINED BIBBITS LIST
    public void AddBibbit(GameObject _bibbit)
    {
        m_SpawnedBibbits.Add(_bibbit);
        PathLogic.AddTransformToMove(_bibbit.transform);
    }

    // REMOVES BIBBIT TO CONTAINED BIBBITS LIST
    public void RemoveBibbit(GameObject _bibbit)
    {
        m_SpawnedBibbits.Remove(_bibbit);
        PathLogic.RemoveTransformToMove(_bibbit.transform);
    }

    // SPAWNS A SIGNLE BIBBIT AND SETS IT TO PATH
    private void SpawnBibbit()
    {
        GameObject newBib = (GameObject)Instantiate(m_BibbitTypes[(int)Random.Range(0, m_BibbitTypes.Length)], transform.position, Quaternion.identity);
        PathLogic.AddTransformToMove(newBib.transform);
        m_SpawnedBibbits.Add(newBib);
    }

    void Awake()
    {
        m_BibbitsRateWait = new WaitForSeconds(m_BibbitRate);
    }

    IEnumerator Start ()
    {
        if (m_SpawningActive)
        {
            for (int bibbitsCount = 0; bibbitsCount < m_MaxBibbits; ++bibbitsCount)
            {
                SpawnBibbit();
                yield return m_BibbitsRateWait;
            }
        }

        StartCoroutine(MaintainMinimalBibbitsCount());
    }

    IEnumerator MaintainMinimalBibbitsCount()
    {
        while (true)
        {
            if (m_SpawnedBibbits.Count < m_MaxBibbits/2)
            {
                Debug.Log("Adding additional bibbit...");
                SpawnBibbit();
            }
            yield return m_BibbitsRateWait;
        }

	}

}
