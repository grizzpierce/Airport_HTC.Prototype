﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OilDropBehaviour : MonoBehaviour
{
    
    public GameObject m_SpillPrefab;
    ParticleSystem m_PS;
    bool m_HasCollided = false;

    void Start()
    {
        m_PS = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if(m_HasCollided == true)
        {
            m_PS.Stop();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (m_HasCollided != true)
        {
            m_HasCollided = true;

            float newY = other.transform.position.y + (other.transform.localScale.y/2);
            GameObject newSpill = (GameObject)Instantiate(m_SpillPrefab, new Vector3(transform.position.x, newY, transform.position.z), Quaternion.identity);
        }
    }
}
