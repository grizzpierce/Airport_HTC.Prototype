using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bibbit_LineSpawner : MonoBehaviour
{
    public float DistToGround = 1.0f;
    public float DropSpeed = 15.0f;

    class DroppedBibbit
    {
        public Transform Transform;
        public Vector3 Origin;
        public Vector3 Ground;
        public float Ratio = 0f;
    }

    private List<DroppedBibbit> m_BibbitsToDrop = new List<DroppedBibbit>();

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

    public delegate void OnBibbitSpawnedDelegate(Bibbit_LineSpawner spawner, Transform bibbit);
    public delegate void OnBibbitUnspawnedDelegate(Bibbit_LineSpawner spawner, Transform bibbit);
    public event OnBibbitSpawnedDelegate OnBibbitSpawned;
    public event OnBibbitUnspawnedDelegate OnBibbitUnspawned;

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

        // HACK: We simulate dropping the bibbits to the ground. Should use physics. clinel 2016-08-19.
        bool isGrounded = false;
        RaycastHit hit;
        if (Physics.Raycast(_bibbit.transform.position, Vector3.down, out hit, LayerMask.GetMask("Floor")))
        {
            isGrounded = hit.distance < DistToGround;
        }

        if (isGrounded)
        {
            PathLogic.AddTransformToMove(_bibbit.transform);
        }
        else
        {
            DroppedBibbit droppedBibbit = new DroppedBibbit();
            droppedBibbit.Transform = _bibbit.transform;
            droppedBibbit.Ratio = 0.0f;
            droppedBibbit.Origin = _bibbit.transform.position;
            droppedBibbit.Ground = hit.point;

            if (m_BibbitsToDrop.Count > 0)
            {
                m_BibbitsToDrop.Add(droppedBibbit);
            }
            else
            {
                m_BibbitsToDrop.Add(droppedBibbit);
                StartCoroutine(DropBibbits());
            }
        }
    }

    private IEnumerator DropBibbits()
    {
        while (m_BibbitsToDrop.Count > 0)
        {
            int nbBibbitsToDrop = m_BibbitsToDrop.Count;
            for (int i = nbBibbitsToDrop - 1; i >= 0; --i)
            {
                DroppedBibbit bibbitToDrop = m_BibbitsToDrop[i];

                float duration = Vector3.Distance(bibbitToDrop.Origin, bibbitToDrop.Ground) / DropSpeed;
                bibbitToDrop.Ratio += Time.deltaTime / duration;
                // TODO: Use better than Lerp. clinel 2016-08-19.
                bibbitToDrop.Transform.position = Vector3.Lerp(bibbitToDrop.Origin, bibbitToDrop.Ground, bibbitToDrop.Ratio);
                if (bibbitToDrop.Ratio >= 1f)
                {
                    m_BibbitsToDrop.RemoveAt(i);
                    PathLogic.AddTransformToMove(bibbitToDrop.Transform);
                }
            }

            yield return null;
        }
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
        AddBibbit(newBib);

        if (OnBibbitSpawned != null)
        {
            OnBibbitSpawned(this, newBib.transform);
        }
    }

    private void UnspawnBibbit(GameObject bibbit)
    {
        RemoveBibbit(bibbit);

        if (OnBibbitUnspawned != null)
        {
            OnBibbitUnspawned(this, bibbit.transform);
        }
    }

    void Awake()
    {
        m_BibbitsRateWait = new WaitForSeconds(m_BibbitRate);
    }

    IEnumerator Start()
    {
        GroupManager.Instance.RegisterSpawner(this);

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

    public void GetNeighbouringBibbits(Transform referenceBibbit, ref List<Transform> neighbours, int maxNbNeighbours)
    {
        PathLogic.GetNeighbouringMembers(referenceBibbit, ref neighbours, maxNbNeighbours);
    }

    IEnumerator MaintainMinimalBibbitsCount()
    {
        while (true)
        {
            if (m_SpawnedBibbits.Count < m_MaxBibbits / 2)
            {
                Debug.Log("Adding additional bibbit...");
                SpawnBibbit();
            }
            yield return m_BibbitsRateWait;
        }

    }

}
