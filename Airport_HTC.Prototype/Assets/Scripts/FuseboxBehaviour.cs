using UnityEngine;
using System.Collections;

public class FuseboxBehaviour : MonoBehaviour {

    GameObject[] m_FuseSwitches;
    GameObject[] m_OffLights;
    bool m_LightsOn = false;


    // Use this for initialization
    void Start () {
        m_FuseSwitches = GameObject.FindGameObjectsWithTag("Light Switch");
        m_OffLights = GameObject.FindGameObjectsWithTag("Off Light");
    }
	
	// Update is called once per frame
	void Update () {

        if (m_LightsOn == false)
        {
            bool allLightsOn = true;

            for (int i = 0; i < m_FuseSwitches.Length; ++i)
            {
                if(m_FuseSwitches[i].GetComponent<LightSwitchBehaviour>().GetIfSwitchOn() == false)
                {
                    Debug.Log("A light is off");
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
            for (int i = 0; i < m_OffLights.Length; ++i)
            {
                m_OffLights[i].GetComponent<Light>().enabled = true;
            }
        }
	
	}
}
