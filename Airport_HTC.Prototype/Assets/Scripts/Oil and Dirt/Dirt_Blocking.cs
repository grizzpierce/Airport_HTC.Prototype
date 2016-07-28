using UnityEngine;
using System.Collections;

public class Dirt_Blocking : MonoBehaviour {

    public GameObject m_BlockedItem;
    private string m_BlockedItemType;

	void Start ()
    {
	    if (m_BlockedItem.tag == "Fusebox")
        {
            m_BlockedItemType = "Fusebox";
            m_BlockedItem.GetComponent<FuseboxBehaviour>().SetActive(false);
        }
        if (m_BlockedItem.tag == "Spawner")
        {
            m_BlockedItemType = "Spawner";
            m_BlockedItem.GetComponent<Bibbit_LineSpawner>().SetIfSpawningActive(false);
        }
	}
	
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Bibbit")
        {
            if (m_BlockedItemType == "Fusebox")
            {
                m_BlockedItem.GetComponent<FuseboxBehaviour>().SetActive(true);
            }
            if (m_BlockedItemType == "Spawner")
            {
                m_BlockedItem.GetComponent<Bibbit_LineSpawner>().SetIfSpawningActive(true);
            }

            Destroy(gameObject); // Animation has different tiers
        }
    }
}
