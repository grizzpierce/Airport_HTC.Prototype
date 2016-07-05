using UnityEngine;
using System.Collections;

public class Bibbit_LineSpawner : MonoBehaviour {

    private GameObject m_PathfinderObj;
    private Bibbit_Pathfinder m_Pathfinder;
    public GameObject m_Bibbit;
    private float m_ElapsedTime = 0;
    private float m_Time;

	// Use this for initialization
	void Start ()
    {
        m_PathfinderObj = new GameObject(gameObject.name + "'s Pathfinder");
        m_PathfinderObj.transform.position = transform.position;
        m_Pathfinder = m_PathfinderObj.AddComponent<Bibbit_Pathfinder>();
        m_PathfinderObj.AddComponent<SphereCollider>();
        m_Pathfinder.SetCastRadius(10f);
        m_Pathfinder.SetFlag(gameObject);
        m_Pathfinder.Look();


        GameObject newBib = (GameObject)Instantiate(m_Bibbit, transform.position, Quaternion.identity);
        newBib.GetComponent<Bibbit_Movement>().SetPathFinder(m_PathfinderObj);
        m_Time = Time.time;
    }

    void Update ()
    {
        m_ElapsedTime = Time.time - m_Time;


        if (m_ElapsedTime >= 10)
        {
            Debug.Log("TEN SECONDS");
            m_Pathfinder.Clear();
            m_Pathfinder.Look();
        }
	}
}
