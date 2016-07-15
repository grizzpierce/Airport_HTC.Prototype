using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuseboxBehaviour : MonoBehaviour {

    GameObject[] m_FuseSwitches;
    public List<GameObject> m_Teleporters = new List<GameObject>();
    bool m_LightsOn = false;
    bool m_TurnedOnce = false;

    void Start ()
    {
        m_FuseSwitches = GameObject.FindGameObjectsWithTag("Light Switch");
        GameObject[] teleporters = GameObject.FindGameObjectsWithTag("Teleport Point");

        for (int i = 0; i < teleporters.Length; ++i)
        {
            if(teleporters[i].transform.parent.GetComponent<TeleportShellBehaviour>().IsOffOnStart)
            {
                m_Teleporters.Add(teleporters[i].transform.parent.gameObject);
            }
        }


    }
	
	void Update () {

        if (m_LightsOn == false)
        {
            bool allLightsOn = true;

            for (int i = 0; i < m_FuseSwitches.Length; ++i)
            {
                if(m_FuseSwitches[i].GetComponent<LightSwitchBehaviour>().GetIfSwitchOn() == false)
                {
                    //Debug.Log("A light is off");
                    allLightsOn = false;
                }    
            }

            if (allLightsOn == true)
            {
                m_LightsOn = true;
            }
        }

        else
        {
            if (!m_TurnedOnce)
            {
                for (int i = 0; i < m_Teleporters.Count; ++i)
                {
                    //Debug.Log("Lights Turning On!");
                    m_Teleporters[i].GetComponent<TeleportShellBehaviour>().IsActive(true);
                }
                m_TurnedOnce = true;
            }

        }
	
	}
}
