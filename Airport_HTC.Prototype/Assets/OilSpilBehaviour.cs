using UnityEngine;
using System.Collections;

public class OilSpilBehaviour : MonoBehaviour {

    public GameObject m_BubblePrefab;
    bool m_BubblesCreated = false;
    
    void Start()
    {
        GameObject newBubbles = (GameObject)Instantiate(m_BubblePrefab, new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z), Quaternion.Euler(new Vector3(-90, 0, 0)));
    }

	void Update ()
    {
        /*
	    if(this.GetComponent<Animation>().isPlaying == false && m_BubblesCreated == false)
        {
            GameObject newBubbles = (GameObject)Instantiate(m_BubblePrefab, new Vector3(transform.position.x, transform.position.y+.1f, transform.position.z), Quaternion.Euler(new Vector3(-90, 0, 0)));
            m_BubblesCreated = true;
        }
        */
	}
}
