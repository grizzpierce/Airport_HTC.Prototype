using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OilAlternating : MonoBehaviour {

    public List<GameObject> m_OilDrop;
    public int m_OilStage = 0;

    public bool m_CurrentOilActive;

    void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
            m_OilDrop.Add(transform.GetChild(i).gameObject);
    }

    void Start()
    {
        m_OilDrop[m_OilStage].GetComponent<OilData>().Play();
    }

    void Update()
    {
        if (!m_OilDrop[m_OilStage].GetComponent<OilData>().GetIfActive())
                SwitchOilDrop();
    }

    private void SwitchOilDrop()
    {
        m_OilDrop[m_OilStage].GetComponent<OilData>().Stop();

        if (m_OilStage + 1 < m_OilDrop.Count)
            m_OilStage++;

        else
            m_OilStage = 0;

        m_OilDrop[m_OilStage].GetComponent<OilData>().Play();
    }
}
