using UnityEngine;
using System.Collections;

public class lerp_test : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;

    public float speed = 1.0F;

    private float startTime;
    private float journeyLength;

    private bool currentLerpOn = false;


    void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }


    void Update()
    {
        if (currentLerpOn == true)
        {
            Lerp(startMarker, endMarker);
        }

        else
        {
            Transform temp = startMarker.transform;
            startMarker = endMarker;
            endMarker = temp;
            currentLerpOn = true;

            startTime = Time.time;
            journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        }
    }


    void Lerp(Transform _start, Transform _end)
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        Debug.Log(fracJourney);
        transform.position = Vector3.Lerp(_start.position, _end.position, fracJourney);

        if (gameObject.transform.position == _end.position)
        {
            currentLerpOn = false;
        }
    }
}
