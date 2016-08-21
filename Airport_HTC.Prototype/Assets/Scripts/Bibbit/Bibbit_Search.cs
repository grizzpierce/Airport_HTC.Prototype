using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bibbit_Search : MonoBehaviour {

    public List<GameObject> m_AllSpawners = new List<GameObject>();
    public GameObject m_ClosestSpawner;

    public bool m_IsSpawnerClose = false;
    public float m_MaxDistance = 5f;

    private bool m_TimerDone = false;
    private float m_ElapsedTime;
    private float m_StartTime;

    private bool m_Lerping = false;
    private float m_JourneyLength;
    private Vector3 m_Pos;
    private Vector3 m_End;

    private float m_MovementSpeed = 1f;

    // Use this for initialization
    void Start ()
    {
        if (gameObject.GetComponent<Bibbit_Stick>() != null)
        {
            Destroy(gameObject.GetComponent<Bibbit_Stick>());
        }

        GameObject[] tempSpawners = GameObject.FindGameObjectsWithTag("Spawner");
        for (int i = 0; i < tempSpawners.Length; ++i)
        {
            m_AllSpawners.Add(tempSpawners[i]);
        }

        m_StartTime = Time.time;
    }

    void CheckSpawners()
    {
        m_Pos = gameObject.transform.position;

        if (m_AllSpawners.Count > 1)
        {
            // COMPARE ALL THE SPAWNERS FOUND
        }

        else if(m_AllSpawners.Count == 1)
        {
            float dist = Vector3.Distance(transform.position, m_AllSpawners[0].transform.position);

            if (dist < m_MaxDistance)
            {
                Debug.Log("Spawner Close Enough!");
                m_ClosestSpawner = m_AllSpawners[0];
                m_IsSpawnerClose = true;
            }
        }
    }

    /*    How to do Bibbit Search:

            1. Check OverlapSphere
            2. Store all flags and spawners
            3. Compare all flags, spawners with bibbit position
                a. If a flag is closest:
                    i. Move the bibbit to that flag
                    ii. Set the path to the closest spawner that contains that flag
                b. If a spawner is closest:
                    i. Move the bibbit to the spawner and set it on the path
                c. If nothing available or open:
                    i. Bibbit flees and disappears forever
    */

    // Update is called once per frame
    void Update ()
    {
        m_ElapsedTime = Time.time - m_StartTime;
        //Debug.Log(m_ElapsedTime);

        if (m_ElapsedTime >= 1f && m_TimerDone == false)
        {
            m_TimerDone = true;
            CheckSpawners();

            if (m_IsSpawnerClose)
            {
                m_StartTime = Time.time;

                m_End = m_ClosestSpawner.transform.position;
                m_JourneyLength = Vector3.Distance(m_Pos, m_End);

                gameObject.GetComponentInChildren<Animation>().PlayQueued("Hop");

                m_Lerping = true;
            }

            else
            {
                m_StartTime = Time.time;

                m_End = ScatterPos();
                m_JourneyLength = Vector3.Distance(m_Pos, m_End);

                m_MovementSpeed += (2*m_MovementSpeed);
                gameObject.GetComponentInChildren<Animation>().PlayQueued("Hop");

                m_Lerping = true;
            }
        }

        if (m_Lerping)
        {
            Lerp(m_Pos, m_End);
        }
	}

    // SETS RANDOM DIRECTION IN A FAKE CIRCLE OF RANDOM OPTIONS
    Vector3 ScatterPos()
    {
        int decision = (int)Random.Range(1, 3);

        switch(decision)
        {
            case 1:
                return new Vector3(transform.position.x + 20f, 0, 0);

            case 2:
                return new Vector3(0, 0, transform.position.z + 20f);

            case 3:
                return new Vector3(transform.position.z + 20f, 0, transform.position.z + 20f);

            default:
                return new Vector3(0, 0, 0);
        }
    }

    void Lerp(Vector3 _start, Vector3 _end)
    {
        float distCovered = (Time.time - m_StartTime) * m_MovementSpeed;
        float fracJourney = distCovered / m_JourneyLength;
        transform.position = Vector3.Lerp(_start, _end, fracJourney);

        if (gameObject.transform.position == _end)
        {
            m_Lerping = false;

            GetComponent<Rigidbody>().freezeRotation = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            GetComponent<SphereCollider>().isTrigger = true;

            if (m_IsSpawnerClose)
            {
                GetComponent<Rigidbody>().freezeRotation = true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                GetComponent<SphereCollider>().isTrigger = true;

                gameObject.AddComponent<Bibbit_Movement>();
                // Note: Removed as the Spawner doesn't keep track of the bibbits anymore.
                // m_ClosestSpawner.GetComponent<Bibbit_LineSpawner>().AddBibbit(gameObject);
                Destroy(gameObject.GetComponent<Bibbit_Search>());
            }

            else
                Destroy(gameObject);

        }
    }


}
