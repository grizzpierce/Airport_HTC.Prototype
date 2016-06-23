using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightsOn : MonoBehaviour {

    GameObject[] m_OffLights;
    bool m_LightsOn = false;

    void Start()
    {
        m_OffLights = GameObject.FindGameObjectsWithTag("Off Light");

        if(m_OffLights != null)
        {
            for (int i = 0; i < m_OffLights.Length; ++i)
            {
                Debug.Log(m_OffLights[i].name);
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Light Switch")
        {
            if (m_LightsOn == false)
            {
                m_LightsOn = true;
                Debug.Log("Lights are on!");

                if (m_OffLights != null)
                {
                    for (int i = 0; i < m_OffLights.Length; ++i)
                    {
                        m_OffLights[i].GetComponent<Light>().enabled = true;
                    }
                }

                // EDIT THIS WHEN NEW MODEL IS IN
                other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;


            }
        }
    }



}
