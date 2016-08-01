using UnityEngine;
using System.Collections;

public class OilSpilBehaviour : MonoBehaviour {

    public GameObject m_BubblePrefab;
    private GameObject newBubbles;

    void Start()
    {
        newBubbles = (GameObject)Instantiate(m_BubblePrefab, new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z), Quaternion.Euler(new Vector3(-90, 0, 0)));
        newBubbles.transform.parent = gameObject.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
            Destroy(gameObject);
    }

}
