using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flag_Data : MonoBehaviour {

    public List<GameObject> m_PathSpawners = new List<GameObject>();    

    public void AddPathSpawner(GameObject _spawner)
    {
        m_PathSpawners.Add(_spawner);
    }

    public GameObject[] GetPathSpawners()
    {

        if (m_PathSpawners.Count > 0)
        {
            GameObject[] pathspawners = new GameObject[m_PathSpawners.Count];

            for (int i = 0; i < m_PathSpawners.Count; ++i)
            {
                pathspawners[i] = m_PathSpawners[i];
            }

            return pathspawners;
        }

        else
            return null;

    }
}
