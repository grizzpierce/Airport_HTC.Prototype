using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BibbitCleaner_CrowdData : MonoBehaviour {

    // OVERALL DATA
    public List<GameObject> m_CrowdBibbits = new List<GameObject>();
    public int m_StartAmount;
    public GameObject m_CurrentSpill;
    bool m_Started = false;
    public int m_FollowerAmount = 3;

    public AudioSource m_AS;
    public AudioClip m_BibbitSound;

    // WAITING STATE
    bool m_Waiting = false;
    bool m_WaitStarted = false;
    float m_WaitTimer;
    public float m_WaitDelay = 3f;

    // MOVEMENT PHASE
    bool m_Moving = false;
    public float speed = 1.0F;
    private Vector3 startMarker;
    private Vector3 endMarker;
    private float startTime;
    private float journeyLength = 0;

    // SEARCHING PHASE
    bool m_Searching = false;
    float m_SearchRadius = 3f;


    // CROWD ADDITION AND REMOVAL
    public void m_AddBibbit(GameObject _bib)
    {
        m_CrowdBibbits.Add(_bib);
    }
    public void m_AddBibbit(List<GameObject> _bibs)
    {
        for (int i = 0; i < _bibs.Count; ++i)
            m_CrowdBibbits.Add(_bibs[i]);
    }

    public void m_RemoveBibbit(GameObject _bib)
    {
        m_CrowdBibbits.Remove(_bib);
    }
    public void m_RemoveBibbit(List<GameObject> _bibs)
    {
        for (int i = 0; i < _bibs.Count; ++i)
            m_CrowdBibbits.Remove(_bibs[i]);
    }

    public void m_BibbitGrabbed(GameObject _bib)
    {
        m_CrowdBibbits.Remove(_bib);
        _bib.transform.parent = null;

        for (int i = 0; i < m_FollowerAmount; ++i)
        {
            m_CrowdBibbits[0].AddComponent<BibbitCleaner_Follow>();
            m_CrowdBibbits[0].GetComponent<BibbitCleaner_Follow>().m_LeadBibbit = _bib;
            m_CrowdBibbits.Remove(m_CrowdBibbits[i]);
        }
    }

    void Start()
    {
        m_AS = gameObject.AddComponent<AudioSource>();
        m_AS.clip = Resources.Load("humming_loop") as AudioClip;
        m_AS.volume = .5f;
        m_AS.loop = true;
    }

    void Update()
    {
        if (m_CrowdBibbits.Count == transform.childCount)
        {
            if (!m_Started)
            {
                SortBibbits();
                IgnoreOtherBibbits();
                m_Started = true;
                m_Waiting = true;
                m_AS.Play();
            }
        }

        if (m_Waiting)
            Wait();

        if(m_Searching)
            Search();

        if(m_Moving)
            Movement();
    }

    private void SortBibbits()
    {
        for (int i = 0; i < m_CrowdBibbits.Count; ++i)
        {
            m_CrowdBibbits[i].transform.parent = null;
        }

        for (int i = 0; i < m_CrowdBibbits.Count; ++i)
        {
            m_CrowdBibbits[i].transform.parent = gameObject.transform;
        }
    }

    private void IgnoreOtherBibbits()
    {
        for (int i = 0; i < m_CrowdBibbits.Count; ++i)
        {
            m_CrowdBibbits[i].AddComponent<Ignore_Physics>();
            m_CrowdBibbits[i].AddComponent<Ignore_Physics>().AddIgnore("Bibbit");
        }
    }

    private void Wait()
    {
        if(!m_WaitStarted)
        {
            m_WaitTimer = Time.time;
            m_WaitStarted = true;
        }

        float elapsed = Time.time - m_WaitTimer;

        if(elapsed > m_WaitDelay)
        {
            Debug.Log("Wait Over!");
            m_Waiting = false;
            m_WaitStarted = false;
            m_Searching = true;
        }
    }

    private void Search()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_SearchRadius);
        List<Transform> oilLocations = new List<Transform>();

        if (hitColliders.Length > 0)
        {
            for (int i = 0; i < hitColliders.Length; ++i)
            {
                if (hitColliders[i].tag == "Oil")
                {
                    oilLocations.Add(hitColliders[i].transform);
                }
            }
        }

        if (oilLocations.Count >= 1)
        {
            if(oilLocations.Count == 1)
                m_CurrentSpill = oilLocations[0].gameObject;

            else
                m_CurrentSpill = CompareOil(oilLocations);

            endMarker = new Vector3(m_CurrentSpill.transform.position.x, transform.position.y, m_CurrentSpill.transform.position.z);
            m_Moving = true;
        }

        else
        {
            Debug.Log("No Spills");
            m_Waiting = true;
        }

        m_Searching = false;
    }

    private GameObject CompareOil(List<Transform> _oil)
    {
        Transform currentShortest = _oil[0];

        for(int i = 0; i < _oil.Count; i++)
        {
            float currentBest = Vector3.Distance(currentShortest.position, transform.position);
            float contender = Vector3.Distance(_oil[i].position, transform.position);

            if(contender < currentBest)
            {
                currentShortest = _oil[i];
            }
        }

        return currentShortest.gameObject;
    }

    private void Movement()
    {
        if (journeyLength == 0)
        {
            startTime = Time.time;
            startMarker = transform.position;
            journeyLength = Vector3.Distance(startMarker, endMarker);
        }

        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startMarker, endMarker, fracJourney);

        if (transform.position == endMarker)
        {
            m_CurrentSpill = m_CurrentSpill.GetComponent<OilSpilBehaviour>().Eaten();

            if(m_CurrentSpill == null)
            {
                m_Moving = false;
                m_Waiting = true;
                journeyLength = 0;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, m_SearchRadius);
    }
}
