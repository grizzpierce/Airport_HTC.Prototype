using UnityEngine;
using System.Collections;

public class lerp_test : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;

    public float speed = 1.0F;

    private float startTime;
    private float journeyLength;

    private bool currentLerpDone = true;


    void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }


    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
    }


    void Lerp(Transform _start, Transform _end)
    {

    }


}
