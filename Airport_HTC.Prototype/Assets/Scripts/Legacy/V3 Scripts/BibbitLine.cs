using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BibbitLine : MonoBehaviour {

    // CUSTOMIZABLE VARIABLES
    public GameObject m_Spawn;
    public List<GameObject> m_LineFlags;
    public List<GameObject> m_Bibbit_Types;
    public int m_MaxBibbits;
    public float m_BibbitBaseSpeed = 1.0F;
    public float m_BibbitSpeedRange;
    public float m_BibbitBaseSpawnRate;
    public float m_BibbitSpawnRange;

    // BIBBIT VARIABLES

    private List<GameObject> m_SpawnedBibbits = new List<GameObject>();

    // BIBBIT TIMER
    private float m_StartTime;
    private float m_ElapsedTime;
    private float m_TrueSpawnRate;
                                                                                                                    /*
    XXXXXXXXXXXXXXXXXXXXXXX
    || THE BEEF IS HERE  ||
    XXXXXXXXXXXXXXXXXXXXXXX                                                                 
                                                                                                                    */
    // THE BEEF
    void Start ()
    {
        m_StartTime = Time.time;
        m_TrueSpawnRate = m_BibbitBaseSpawnRate;
	}
	
    // MORE BEEF
	void Update ()
    {
        FlagSearch();
        FlagCheck();

        m_ElapsedTime = Time.time - m_StartTime;

        if (m_LineFlags.Count > 0)
        {
            if (m_SpawnedBibbits.Count < m_MaxBibbits)
            {
                if (m_ElapsedTime >= m_TrueSpawnRate || m_SpawnedBibbits.Count == 0)
                {
                    SpawnNewBibbit();
                    m_StartTime = Time.time;
                    m_TrueSpawnRate = m_BibbitBaseSpawnRate + Random.Range(-m_BibbitBaseSpawnRate, m_BibbitSpawnRange);
                }
            }
        }

        CheckIfBibbitsDone();
	}

    // SEARCHES FOR NEW FLAGS
    void FlagSearch()
    {
        GameObject[] allFlags = GameObject.FindGameObjectsWithTag("Flag");

        for (int i = 0; i < allFlags.Length; ++i)
        {
            if (allFlags[i].GetComponent<LineFlag>().GetIfObjGrabbed() == false)
            {
                if (allFlags[i].GetComponent<LineFlag>().GetIfGrounded())
                {
                    // Debug.Log("Flag Found!");
                    m_LineFlags.Add(allFlags[i]);
                    allFlags[i].tag = "Discovered";
                }
            }
        }
    }

    // CHECKS IF ANY FLAGS ARE GRABBED
    void FlagCheck()
    {
        bool m_AreAnyGrabbed = false;

        for (int i = 0; i < m_LineFlags.Count; ++i)
        {
            if (m_LineFlags[i].GetComponent<LineFlag>().GetIfObjGrabbed() == true)
            {
                m_AreAnyGrabbed = true;
            }
        }

        if (m_AreAnyGrabbed == true)
        {
            //Debug.Log("A Flag Is Grabbed");

            for (int i = 0; i < m_SpawnedBibbits.Count; ++i)
            {
                m_SpawnedBibbits[i].GetComponent<Cleaning_Bibbit>().Stop();
            }
        }
        else
        {
            //Debug.Log("No Flags are Grabbed");

            for (int i = 0; i < m_SpawnedBibbits.Count; ++i)
            {
                m_SpawnedBibbits[i].GetComponent<Cleaning_Bibbit>().Play();
            }
        }
    }

    // CREATES A BIBBIT AND SETS PATHING
    void SpawnNewBibbit()
    {
        GameObject temp = (GameObject)Instantiate(m_Bibbit_Types[Random.Range(0, m_Bibbit_Types.Count)], m_Spawn.transform.position, Quaternion.identity);
        m_SpawnedBibbits.Add(temp);
        temp.AddComponent<Cleaning_Bibbit>();
        temp.GetComponent<Cleaning_Bibbit>().SetSpeed(m_BibbitBaseSpeed + Random.Range(0.0f,m_BibbitSpeedRange));
        temp.GetComponent<Cleaning_Bibbit>().AddToTravelFlags(m_Spawn.GetComponent<LineFlag>().GetRandomFlagTransform());
        SetToPath(temp);
    }

    // CHECKS IF ANY BIBBITS REACHES THE END
    void CheckIfBibbitsDone()
    {
        for (int i = 0; i < m_SpawnedBibbits.Count; ++i)
        {
            if (m_SpawnedBibbits[i].GetComponent<Cleaning_Bibbit>().GetIfCompletedRoute() == true)
            {
                GameObject temp = m_SpawnedBibbits[i];
                m_SpawnedBibbits.RemoveAt(i);
                Destroy(temp);
            }
        }
    }

    // ADDS FLAGS TO INDIVIDUAL SPAWNED BIBBITS
    void SetToPath(GameObject _bibbit)
    {
        for (int i = 0; i < m_LineFlags.Count; ++i)
        {
            _bibbit.GetComponent<Cleaning_Bibbit>().AddToTravelFlags(m_LineFlags[i].GetComponent<LineFlag>().GetRandomFlagTransform());
        }
    }
}
