using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bibbit_Pathfinder : MonoBehaviour {

    private float m_CastRadius = 5f;
    public List<GameObject> m_hitFlags = new List<GameObject>();

    private bool m_NewColAdded = false;
    private bool m_Reversed = false;
    private bool m_Done = false;

    // LERP VALUES
    private GameObject m_CurrentFlag;
    private GameObject m_NextFlag;

    // GETTERS & SETTERS
    public List<GameObject> GetCurrentPath() { return m_hitFlags; }
    public void SetCastRadius(float _radius) { m_CastRadius = _radius; }
    public void SetFlag(GameObject _obj)     { m_hitFlags.Add(_obj); }

    void Start()
    {
        
    }

    // FUNCTION: void Look()
    /*-------------------------------------------------------------------------------------
    
    OCCURENCE: Whenever landing on a new flag

    PROCESS:
    1.) Preform an OverlapSphere check to see if there are any *NEW* flags
    2.) Decide what to do depending on how many flags there are
        a.) If none, then either
              i.) Reverse() is started if traffic is incoming
             ii.) Object is killed if traffic is outgoing
        b.) If one, add the new flag to m_PathPoints - comprised of Transform points
        c.) If more than one, check which flag is closer using distance checks

    -------------------------------------------------------------------------------------*/

    public void Look()
    {
        m_NewColAdded = false;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_CastRadius);
        List<Collider> currentFlags = new List<Collider>();

        // THE SPHERECAST THAT CHECKS FOR FLAGS
        for (int i = 0; i < hitColliders.Length; ++i)               
        {
            if (hitColliders[i].tag == "Flag Point")                
            {
                if (m_hitFlags.Contains(hitColliders[i].gameObject) != true)   
                {
                    m_NewColAdded = true;
                    currentFlags.Add(hitColliders[i]);
                }
            }
        }

        // THE CHECK TO SEE IF THERE WERE ANY FLAGS FOUND
        if (currentFlags != null && m_NewColAdded == true)          
        {
            // IF THERE WAS ONLY ONE
            if(currentFlags.Count == 1)                             
            {
                m_CurrentFlag = m_hitFlags[m_hitFlags.Count - 1];
                m_hitFlags.Add(currentFlags[0].gameObject);
                m_NextFlag = m_hitFlags[m_hitFlags.Count - 1];

                gameObject.transform.position = m_hitFlags[m_hitFlags.Count - 1].transform.position;

                Look();
            }

            // IF THERE WERE MULTIPLE
            else if(currentFlags.Count > 1)                         
            {
                m_CurrentFlag = m_hitFlags[m_hitFlags.Count - 1];
                m_hitFlags.Add(CompareFlags(currentFlags));
                m_NextFlag = m_hitFlags[m_hitFlags.Count - 1];

                gameObject.transform.position = m_hitFlags[m_hitFlags.Count - 1].transform.position;

                Look();
            }
        }

        // THE CHECK IF NO FLAGS WERE FOUND
        else if (m_NewColAdded == false )
        {
            Debug.Log("Nothing!");
        }
    }

    public void Clear()
    {
        m_hitFlags.Clear();
    }

    // MANY FLAGS COME IN, ONLY ONE REMAINS
    GameObject CompareFlags(List<Collider> _currentflags)
    {
        int count = _currentflags.Count;

        while (count > 1)
        {
            float firstDist = Hypotenuse(_currentflags[0].transform.position.x, _currentflags[0].transform.position.z);
            float secondDist = Hypotenuse(_currentflags[1].transform.position.x, _currentflags[1].transform.position.z);

            if(firstDist < secondDist)
                _currentflags.Remove(_currentflags[1]);
            else
                _currentflags.Remove(_currentflags[0]);

            count--;
        }

        return _currentflags[0].gameObject;
    }

    // SIMPLE HYPOTENUSE CALCULATOR FOR FLAG COMPARISONS
    private float Hypotenuse(float _x, float _z)
    {
        float length = Mathf.Abs((gameObject.transform.position.x) - _x);
        float width = Mathf.Abs((gameObject.transform.position.y) - _z);

        if (length == 0)
            length = .01f;

        if (width == 0)
            width = .01f;

        return Mathf.Sqrt((Mathf.Pow(length, 2)) + (Mathf.Pow(width, 2)));
    }


    // FUNCTION: void Reverse()
    /*-------------------------------------------------------------------------------------

    OCCURENCE: On last available point in incoming traffic

    PROCESS:
    1.) Clear the m_PathPoints
    2.) Record the current point as it's starting point
    3.) Do a Look() check to see next point
        a.) However, if there's no point available then kill the object 

    -------------------------------------------------------------------------------------*/

    private void Reverse()
    {
        GameObject currentPoint = m_hitFlags[m_hitFlags.Count-1];
        m_hitFlags.Clear();
        m_hitFlags.Add(currentPoint);
        Look();
        m_Done = true;
    }

    // DISPLAYS THE SIZE OF THE LOOKING SPHERE
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_CastRadius);
    }

}
