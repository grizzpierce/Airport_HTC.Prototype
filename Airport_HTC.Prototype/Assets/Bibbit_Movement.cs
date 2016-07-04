using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bibbit_Movement : MonoBehaviour {

    public GameObject m_PathfinderObject;
    private Bibbit_Pathfinder m_Pathfinder;
    private List<GameObject> m_PathNodes;

    // LERP VARIABLES
    public float m_MovementSpeed = 1f;
    private int m_FlagCount = 0;
    private bool m_CurrentLerpOn = false;
    private float m_StartTime;
    private float m_JourneyLength;

	void Start ()
    {
        m_Pathfinder = m_PathfinderObject.GetComponent<Bibbit_Pathfinder>();
        m_PathNodes = m_Pathfinder.GetCurrentPath();
        m_StartTime = Time.time;
    }
	
	void Update ()
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
                m_JourneyLength = Vector3.Distance(m_PathNodes[m_FlagCount].transform.position, m_PathNodes[m_FlagCount + 1].transform.position);

                if (m_FlagCount > m_PathNodes.Count + 1)
                {

                    m_CurrentLerpOn = false;
                }
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
        Debug.Log(fracJourney);
        transform.position = Vector3.Lerp(_start.position, _end.position, fracJourney);

        if (gameObject.transform.position == _end.position)
        {
            m_CurrentLerpOn = false;
        }
    }
}
