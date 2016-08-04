using UnityEngine;
using System.Collections;

public class BibbitCleaner_InitMove : MonoBehaviour {

    public GameObject m_Parent;

    public Vector3 startMarker;
    public Vector3 endMarker;

    public float speed = 1.0F;
    public bool m_LerpOn = false;

    private float startTime;
    private float journeyLength = 0;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (m_LerpOn)
        {
            if (journeyLength == 0)
                journeyLength = Vector3.Distance(startMarker, endMarker);

            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startMarker, endMarker, fracJourney);
        }

        if (transform.position == endMarker)
        {
            transform.parent = m_Parent.transform;
            gameObject.AddComponent<BibbitCleaner_Idle>();
        }
    }
}
