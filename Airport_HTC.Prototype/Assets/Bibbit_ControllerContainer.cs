using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bibbit_ControllerContainer : MonoBehaviour {

    List<GameObject> m_StuckBibbits = new List<GameObject>();
    public int m_MaxBibbits = 3;

    public bool RoomForMoreBibbits()
    {
        if (m_StuckBibbits.Count < m_MaxBibbits)
            return true;
        
        else
            return false;
    }

    public void AddBibbit(GameObject _obj)
    {
        m_StuckBibbits.Add(_obj);
    }

    public void RemoveBibbit(GameObject _obj)
    {
        m_StuckBibbits.Remove(_obj);
    }
}
