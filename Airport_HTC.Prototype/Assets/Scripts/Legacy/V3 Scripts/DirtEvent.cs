using UnityEngine;
using System.Collections;

public class DirtEvent : MonoBehaviour
{
    public GameObject m_ParticleFX;
    public GameObject m_Seed;
    private GameObject newSeed;
    private GameObject m_DirtObj;
    private GameObject m_CurrentParticle;
    private int m_ElapsedBibbit;
    private bool m_HasBeenPlayed = false;

    // !LEGACY! LERPING VARIABLES
    //public float m_MovementRate;
    //public float m_MovementSpeed;
    private float m_StartTime;
    private float m_JourneyLength;
    private bool m_IsLerpOver = true;
    GameObject CurrentPos;
    GameObject GoalPos;



    // THE BEEF
    void Start()
    {

        m_DirtObj = transform.FindChild("prop_dirt_pile").gameObject;

        // CurrentPos = new GameObject();     // !LEGACY!
        // GoalPos = new GameObject();        // !LEGACY!
    }

    // TRIGGERS 
    void OnTriggerExit(Collider col)
    {
        Debug.Log(col.gameObject.name);
        if (col.gameObject.tag == "Bibbit")
        {
            /* LEGACY
            if (m_IsLerpOver)
            {
                Debug.Log("Bibbit Left!");
                ++m_ElapsedBibbit;
            }
            */
            ++m_ElapsedBibbit;

            if (m_ElapsedBibbit >= 20 && m_DirtObj.GetComponent<Animation>().isPlaying != true && m_HasBeenPlayed != true)
            {
                Debug.Log("Time for a cleanup!");
                GameObject newSeed = (GameObject)Instantiate(m_Seed, new Vector3(transform.position.x, 1f, transform.position.z), Quaternion.identity);
                m_CurrentParticle = (GameObject)Instantiate(m_ParticleFX, transform.position, Quaternion.identity);
                m_DirtObj.GetComponent<Animation>().Play();
                m_HasBeenPlayed = true;

                //MoveDirtPile();          
            }
        }
    }

    void Update()
    {
        if(m_HasBeenPlayed == true && m_DirtObj.GetComponent<Animation>().isPlaying != true)
        {
            m_CurrentParticle.GetComponent<ParticleSystem>().Stop();
            Destroy(gameObject);
        }
    }
    
    /* !LEGACY! MOVES DIRT PILE DOWN USING TWO GENERATED EMPTY GAMEOBJECTS
    void MoveDirtPile()
    {
        if (CurrentPos.transform.position != gameObject.transform.position)
        {
            Debug.Log("Resetting some lerping values...");

            CurrentPos.transform.position = gameObject.transform.position;
            GoalPos.transform.position = gameObject.transform.position;
            GoalPos.transform.Translate(0f, m_MovementRate, 0f);
            m_JourneyLength = Vector3.Distance(CurrentPos.transform.position, GoalPos.transform.position);
            m_StartTime = Time.time;
        }

        Lerp(CurrentPos.transform, GoalPos.transform);

        if (m_IsLerpOver)
        {
            Debug.Log("Lerp is complete!");
            m_ElapsedBibbit = 0;
            m_IsLerpOver = true;
        }
    }

    // !LEGACY! LERP FUNCTION FOR DIRT PILE
    void Lerp(Transform _start, Transform _end)
    {
        Debug.Log("Lerping, Please Hold...");

        if(m_IsLerpOver == true)
        {
            m_IsLerpOver = false;
        }
        

        float distCovered = (Time.time - m_StartTime) * m_MovementSpeed;
        float fracJourney = distCovered / m_JourneyLength;
        transform.position = Vector3.Lerp(_start.position, _end.position, fracJourney);
        
        if (gameObject.transform.position == _end.position)
        {
            Debug.Log("It's Done Moving!");
            m_IsLerpOver = true;
        }
    }*/
    

}
