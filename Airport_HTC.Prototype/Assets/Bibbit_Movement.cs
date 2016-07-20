using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bibbit_Movement : MonoBehaviour {

    public GameObject m_Spawner;
    public GameObject m_PathfinderObject;
    private Bibbit_Pathfinder m_Pathfinder;
    private List<GameObject> m_PathNodes = new List<GameObject>();
    private List<GameObject> m_RevPathNodes = new List<GameObject>();
    private bool m_IsPlaying = true;
    private bool m_IsReversed = false;

    public AnimationClip m_Orbit;

    // LERP VARIABLES
    public float m_MovementSpeed = 1f;
    private int m_FlagCount = -1;
    private bool m_CurrentLerpOn = false;
    private float m_StartTime;
    private float m_JourneyLength;

    // GETTERS & SETTERS
    public void SetPathFinder(GameObject _obj)
    {
        m_PathfinderObject = _obj;
        m_Pathfinder = m_PathfinderObject.GetComponent<Bibbit_Pathfinder>();
        m_PathNodes = m_Pathfinder.GetCurrentPath();
        m_StartTime = Time.time;
        ReverseStoredPath();
    }

    public void SetSpawner(GameObject _spawner)
    {
        m_Spawner = _spawner;
    }
    public void SetPathNodes(List<GameObject> _nodes)
    {
        m_IsPlaying = false;
        m_PathNodes.Clear();

        for (int i = 0; i < _nodes.Count; ++i)
        {
            m_PathNodes.Add(_nodes[i]);
        }

        ReverseStoredPath();
        m_IsPlaying = true;
    }
	


	void Update ()
    {
        if(transform.rotation.eulerAngles != new Vector3(0,0,0))
            transform.rotation = Quaternion.Euler(0, 0, 0);

        if (m_PathfinderObject != null || m_PathNodes.Count != 0)
        {
            if (m_IsPlaying)
                Movement();
        }
    }

    void Movement()
    {
            if (m_IsReversed != true)
                Forward();

            else
                Backward();

    }

    // CONTROLS FORWARD MOVEMENT
    void Forward()
    {
        if (m_FlagCount < m_PathNodes.Count - 1)
        {
            if (m_CurrentLerpOn == true)
            {
                Travel((m_PathNodes[m_FlagCount]).transform, m_PathNodes[m_FlagCount + 1].transform);
            }

            else
            {
                m_CurrentLerpOn = true;
                ++m_FlagCount;
                m_StartTime = Time.time;

                
                if (m_FlagCount != m_PathNodes.Count - 1)
                    m_JourneyLength = Vector3.Distance(m_PathNodes[m_FlagCount].transform.position, m_PathNodes[m_FlagCount + 1].transform.position);

                if (m_FlagCount > m_PathNodes.Count + 1)
                    m_CurrentLerpOn = false;

            }
        }

        else if (m_FlagCount >= m_PathNodes.Count - 1)
        {
            if (m_FlagCount < 1)
            {
                Debug.LogError("Not enough flags");
            }

            else
            {
                m_IsReversed = true;
                m_FlagCount = 0;
            }
        }
    }

    // CONTROLS BACKWARD MOVEMENT
    void Backward()
    {
        if (m_FlagCount < m_RevPathNodes.Count - 1)
        {
            if (m_CurrentLerpOn == true)
            {
                Travel((m_RevPathNodes[m_FlagCount]).transform, m_RevPathNodes[m_FlagCount + 1].transform);
            }

            else
            {
                m_CurrentLerpOn = true;
                ++m_FlagCount;
                m_StartTime = Time.time;

                if (m_FlagCount != m_RevPathNodes.Count - 1)
                    m_JourneyLength = Vector3.Distance(m_RevPathNodes[m_FlagCount].transform.position, m_RevPathNodes[m_FlagCount + 1].transform.position);

                if (m_FlagCount > m_RevPathNodes.Count + 1)
                    m_CurrentLerpOn = false;
            }
        }

        else if (m_FlagCount >= m_RevPathNodes.Count - 1)
        {
            if (m_FlagCount < 1)
            {
                Debug.LogError("Not enough flags");
            }

            else
            {
                m_IsReversed = false;
                m_FlagCount = 0;
            }
        }
    }

    // FUNCTION: void Travel()
    /*-------------------------------------------------------------------------------------

    OCCURENCE: Whenever the object is active and there is an available path

    PROCESS:
                
   -------------------------------------------------------------------------------------*/

    private void Travel(Transform _start, Transform _end)
    {
        float distCovered = (Time.time - m_StartTime) * m_MovementSpeed;
        float fracJourney = distCovered / m_JourneyLength;
        transform.position = Vector3.Lerp(_start.position, _end.position, fracJourney);

        if (gameObject.transform.position == _end.position)
        {
            m_CurrentLerpOn = false;
        }
    }

    // STORES THE REVERSED PATH OF BIBBIT
    private void ReverseStoredPath()
    {
        for (int i = m_PathNodes.Count; i > 0; --i)
        {
            m_RevPathNodes.Add(m_PathNodes[i - 1]);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Controller")
        {
            // CHECK IF ENOUGH SPACE IN CONTAINER
            if (collider.GetComponent<Bibbit_ControllerContainer>().RoomForMoreBibbits())
            {
                collider.GetComponent<Bibbit_ControllerContainer>().AddBibbit(gameObject);
                m_Spawner.GetComponent<Bibbit_LineSpawner>().RemoveBibbit(gameObject);

                gameObject.AddComponent<Bibbit_Stick>();
                gameObject.GetComponent<Bibbit_Stick>().SetStuckObject(collider.gameObject);
                gameObject.GetComponent<Bibbit_Stick>().PassAnimation(m_Orbit);
            }
        }
    }
}
