using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cleaning_Bibbit : MonoBehaviour {

    // CUSTOMIZABLE VARIABLES
    public float m_MovementSpeed;

    // LERPING FLAGS
    private List<Transform> m_TravelFlags = new List<Transform>();
    private List<Transform> m_BackFlags = new List<Transform>();

    // LERPING VARIABLES
    private float m_StartTime;
    private float m_JourneyLength;
    private bool m_IsMovingForward = true;
    private bool m_IsDoneMoving = false;
    private bool m_CompletedRoute = false;
    private int m_ForwardReps = 0;
    private int m_BackwardReps = 0;
    private bool m_PlayRoute = true;
                                                                                                /*
    XXXXXXXXXXXXXXXXXXXXXXX
    || THE BEEF IS HERE  ||
    XXXXXXXXXXXXXXXXXXXXXXX                                                                                                                                                   */


    // GETTERS AND SETTERS
    public bool GetIfCompletedRoute() { return m_CompletedRoute; }
    public void SetSpeed(float _speed) { m_MovementSpeed = _speed; }


    public void AddToTravelFlags(Transform _transform)
    {
        m_TravelFlags.Add(_transform);
        /*Physics.IgnoreCollision(_transform.gameObject.transform.parent.FindChild("Sign").gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        if (_transform.gameObject.transform.parent.gameObject.name != "SPAWN")
        {
            Physics.IgnoreCollision(_transform.parent.FindChild("Sign").gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
            Physics.IgnoreCollision(_transform.parent.FindChild("Sign").FindChild("Ground Collider").gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        }
        */
    }

    // THE BEEF
	void Start ()
    {
        m_StartTime = Time.time;
	}
	
    // MORE BEEF
	void Update ()
    {

        if (m_TravelFlags.Count > 0 || m_BackFlags.Count > 0)
        {
            if(m_PlayRoute == true)
            {
                if (m_IsMovingForward)
                    MoveForward();

                else
                    MoveBackward();
            }

        }

        if (m_ForwardReps == m_BackwardReps && m_ForwardReps > 0)
            m_CompletedRoute = true;
    }

    public void Stop()
    {
        m_PlayRoute = false;
    }

    public void Play()
    {
        m_PlayRoute = true;
    }

    // MOVE BIBBIT FORWARD ON PATH
    void MoveForward()
    {
        //Debug.Log("Moving Forward!");

        Lerp(m_TravelFlags[0], m_TravelFlags[1]);

        if (m_IsDoneMoving)
        {
            m_BackFlags.Add(m_TravelFlags[0]);
            m_TravelFlags.RemoveAt(0);
            m_IsDoneMoving = false;
        }

        //Debug.Log("Travel Flag Count: " + m_TravelFlags.Count);
        if (m_TravelFlags.Count == 1)
            {
                //Debug.Log("Done Moving Forward!");
                m_BackFlags.Add(m_TravelFlags[0]);
                m_TravelFlags.RemoveAt(0);

                m_IsMovingForward = false;
                m_IsDoneMoving = false;
                ++m_ForwardReps;
                //Debug.Log("FORWARD REPS: " + m_ForwardReps);
                m_BackFlags.Reverse();
            }
    }

    // MOVE BIBBIT BACKWARD ON PATH
    void MoveBackward()
    {
        //Debug.Log("Moving Backward!");

        Lerp(m_BackFlags[0], m_BackFlags[1]);

        if (m_IsDoneMoving)
        {
            m_TravelFlags.Add(m_BackFlags[0]);
            m_BackFlags.RemoveAt(0);
            m_IsDoneMoving = false;
        }

        if (m_BackFlags.Count == 1)
        {
            //Debug.Log("Done Moving Backward!");
            m_TravelFlags.Add(m_BackFlags[0]);
            m_BackFlags.RemoveAt(0);

            m_IsMovingForward = true;
            m_IsDoneMoving = false;
            ++m_BackwardReps;
            //Debug.Log("BACKWARD REPS: " + m_BackwardReps);
            m_TravelFlags.Reverse();
        }
    }

    // LERP FUNCTION (USES TWO FLAG PARAMETERS)
    void Lerp(Transform _start, Transform _end)
    {
        gameObject.transform.position = _start.position;

        if (gameObject.transform.position == _start.position)
        {
            m_JourneyLength = Vector3.Distance(_start.position, _end.position);
        }


        float distCovered = (Time.time - m_StartTime) * m_MovementSpeed;
        float fracJourney = distCovered / m_JourneyLength;
        transform.position = Vector3.Lerp(_start.position, _end.position, fracJourney);

        if (gameObject.transform.position == _end.position)
        {
            // isDoneWithMove = true;
            m_StartTime = Time.time;
            m_IsDoneMoving = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

}
