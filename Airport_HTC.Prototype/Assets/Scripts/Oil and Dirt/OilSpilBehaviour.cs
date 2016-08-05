using UnityEngine;
using System.Collections;

public class OilSpilBehaviour : MonoBehaviour {

    public bool m_IsFuseboxOil = false;
    public GameObject m_BubblePrefab;
    private GameObject newBubbles;
    private bool m_StartEaten = false;

    // LERP STUFF
    float startTime;
    float journeyLength = 0;
    Vector3 startMarker;
    Vector3 endMarker;
    public float speed = .5f;

    void Start()
    {
        if(!m_IsFuseboxOil)
        {
            newBubbles = (GameObject)Instantiate(m_BubblePrefab, new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z), Quaternion.Euler(new Vector3(-90, 0, 0)));
            newBubbles.transform.parent = gameObject.transform;
        }
    }

    public GameObject Eaten()
    {
        // LERP GAMEOBJECT DOWN
        if (!m_IsFuseboxOil)
        {
            if (journeyLength == 0)
            {
                startTime = Time.time;
                startMarker = transform.position;
                endMarker = new Vector3(startMarker.x, startMarker.y - 1, startMarker.z);
                journeyLength = Vector3.Distance(startMarker, endMarker);
            }

            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startMarker, endMarker, fracJourney);


            if (transform.position == endMarker)
            {
                Destroy(gameObject);
                return null;
            }

            else
                return gameObject;
        }
        else
        {
            GetComponent<Dirt_Blocking>().Unlock();
            Destroy(gameObject);
            return null;
        }

    }

}
