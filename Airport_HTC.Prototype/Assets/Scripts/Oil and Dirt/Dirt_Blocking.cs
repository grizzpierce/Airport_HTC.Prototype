using UnityEngine;
using System.Collections;

public class Dirt_Blocking : MonoBehaviour {

    public GameObject m_BlockedItem;
    private string m_BlockedItemType;

    public bool m_NotStartBlocked = false; 


	void Start ()
    {
	    if (m_BlockedItem.tag == "Fusebox")
        {
            m_BlockedItemType = "Fusebox";
            m_BlockedItem.GetComponent<FuseboxBehaviour>().SetActive(m_NotStartBlocked);
        }
        if (m_BlockedItem.tag == "Spawner")
        {
            m_BlockedItemType = "Spawner";
            m_BlockedItem.GetComponent<Bibbit_LineSpawner>().SetIfSpawningActive(m_NotStartBlocked);
        }
	}

    public void Unlock()
    {
        if (m_BlockedItemType == "Fusebox")
        {
            m_BlockedItem.GetComponent<FuseboxBehaviour>().SetActive(true);
        }
        if (m_BlockedItemType == "Spawner")
        {
            m_BlockedItem.GetComponent<Bibbit_LineSpawner>().SetIfSpawningActive(true);
        }
    }



}
