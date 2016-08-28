using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bibbit_LineSpawner : MonoBehaviour
{
    // CUSTOMIZABLE PARTS OF SPAWNER
    public GameObject[] m_BibbitTypes;
    public bool m_IsPathStatic = false;
    public float m_PathfinderRate = 5;
    public bool m_SpawningActive = true;
    public int m_MaxBibbits;

    // CUSTOMIZABLE PARTS OF BIBBIT CREATION
    public float m_BibbitRate = 5;

    [Range(0, 3)]
    public int m_Priority; // NOTE: FULLY IMPLEMENT PRIORITY BASED SYSTEMS

    public delegate void OnBibbitSpawnedDelegate(Bibbit_LineSpawner spawner, Transform bibbit);
    public delegate void OnBibbitUnspawnedDelegate(Bibbit_LineSpawner spawner, Transform bibbit);
    public event OnBibbitSpawnedDelegate OnBibbitSpawned;
    public event OnBibbitUnspawnedDelegate OnBibbitUnspawned;

    // VARIABLES FOR BIBBIT CREATION SYSTEM
    private List<GameObject> m_SpawnedBibbits = new List<GameObject>();
    private WaitForSeconds m_BibbitsRateWait;
    private int m_AdditionalBibbitsToSpawn = 0;

    // SETS IF SPAWNING IS ACTIVE
    public void SetIfSpawningActive(bool _isSpawning)
    {
        m_SpawningActive = _isSpawning;
    }

    // SPAWNS A SIGNLE BIBBIT AND SETS IT TO PATH
    private void SpawnBibbit()
    {
        GameObject newBib = (GameObject)Instantiate(m_BibbitTypes[(int)Random.Range(0, m_BibbitTypes.Length)], transform.position, Quaternion.identity);

        if (OnBibbitSpawned != null)
        {
            OnBibbitSpawned(this, newBib.transform);
        }
    }

    private void UnspawnBibbit(GameObject bibbit)
    {
        if (OnBibbitUnspawned != null)
        {
            OnBibbitUnspawned(this, bibbit.transform);
        }
    }

    public void Awake()
    {
        m_BibbitsRateWait = new WaitForSeconds(m_BibbitRate);
    }

    void Start()
    {
        GroupManager.Instance.RegisterSpawner(this);

        if (m_SpawningActive)
        {
            SpawnAdditionalBibbits(m_MaxBibbits);
        }
    }

    public void SpawnAdditionalBibbits(int nbNewBibbits)
    {
        // TODO: Fix that readding bibbits to the group doesn't stop from adding additional bibbits. clinel . 2016-08-21.
        if (m_AdditionalBibbitsToSpawn == 0)
        {
            m_AdditionalBibbitsToSpawn = nbNewBibbits;
            StartCoroutine(SpawnAdditionalBibbitsCoroutine());
        }
        else
        {
            m_AdditionalBibbitsToSpawn += nbNewBibbits;
        }
    }

    IEnumerator SpawnAdditionalBibbitsCoroutine()
    {
        while (m_AdditionalBibbitsToSpawn > 0)
        {
            yield return m_BibbitsRateWait;

            SpawnBibbit();
            --m_AdditionalBibbitsToSpawn;
        }
    }
}
