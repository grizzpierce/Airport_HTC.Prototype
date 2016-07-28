using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuseboxBehaviour : MonoBehaviour {

    GameObject[] m_AllFuseSwitches;
    public bool m_AllLocked = true;
    public GameObject[] m_LockedSwitches;

    public List<GameObject> m_Teleporters = new List<GameObject>();
    bool m_LightsOn = false;
    bool m_TurnedOnce = false;
    bool m_Active = true;

    public float m_Delay = .5f;
    private int m_LightCount = 0;
    private float m_Timer = 0;
    public AudioClip m_SpotlightAudio;

    // SETS IF THE FUSEBOX CAN BE INTERACTED WITH AND SETS IF SWITCHES CAN BE INTERACTED
    public void SetActive(bool _isActive)
    {
        m_Active = _isActive;
        // Debug.Log("FuseBox is active: " + _isActive);
        if (m_AllFuseSwitches != null)
        {
            if (m_AllLocked)
            {
                for (int i = 0; i < m_AllFuseSwitches.Length; ++i)
                {
                    m_AllFuseSwitches[i].GetComponent<LightSwitchBehaviour>().SetIfLocked(!_isActive);
                }
            }
            else
            {
                for (int i = 0; i < m_LockedSwitches.Length; ++i)
                {
                    m_LockedSwitches[i].GetComponent<LightSwitchBehaviour>().SetIfLocked(!_isActive);
                }
            }

        }
    }

    void Awake ()
    {
        // Stores all fuse switches and teleporters
        m_AllFuseSwitches = GameObject.FindGameObjectsWithTag("Light Switch");
        GameObject[] teleporters = GameObject.FindGameObjectsWithTag("Teleport Point");

        // Checks if any stored teleporters are off
        for (int i = 0; i < teleporters.Length; ++i)
        {
            if(teleporters[i].transform.parent.GetComponent<TeleportShellBehaviour>().IsOffOnStart)
            {
                m_Teleporters.Add(teleporters[i].transform.parent.gameObject);
            }
        }
    }
	
	void Update ()
    {
        if (m_Active)
        {
            // Constantly checks if lights are still off
            if (m_LightsOn == false)
            {
                // States they're in the process of turning on
                bool allLightsOn = true;

                // Checks if all lights are switched up
                for (int i = 0; i < m_AllFuseSwitches.Length; ++i)
                {
                    // If any aren't, then lights are still off
                    if (m_AllFuseSwitches[i].GetComponent<LightSwitchBehaviour>().GetIfSwitchOn() == false)
                    {
                        allLightsOn = false;
                    }
                }

                // If they end up all being on, then lights are on
                if (allLightsOn == true)
                {
                    m_LightsOn = allLightsOn;
                }
            }

            // If the lights are on though
            else
            {
                // And they haven't been turned on once
                if (!m_TurnedOnce)
                {
                    AudioSource[] spotlightAudio = new AudioSource[m_Teleporters.Count];

                    // Then it turns on all lights
                    for (int i = 0; i < m_Teleporters.Count; ++i)
                    {
                        m_Teleporters[i].GetComponent<TeleportShellBehaviour>().IsActive(true);
                        m_Teleporters[i].GetComponent<TeleportShellBehaviour>().PlaySound();
                    }

                    m_TurnedOnce = true;
                }

            }
        }
	}
}
