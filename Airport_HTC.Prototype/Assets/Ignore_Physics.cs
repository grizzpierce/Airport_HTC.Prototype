using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ignore_Physics : MonoBehaviour {

    public List<string> m_IgnoreTags = new List<string>();
    private int m_IgnoreStage = 0;

	void Awake ()
    {
        if (m_IgnoreStage < m_IgnoreTags.Count)
        {
            SetIgnore(m_IgnoreTags[m_IgnoreStage]);
        }
    }

    public void AddIgnore(string _tag)
    {
        m_IgnoreTags.Add(_tag);
        SetIgnore(_tag);
    }

    private void SetIgnore(string _tag)
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag(_tag);

        for (int i = 0; i < temp.Length; ++i)
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), temp[i].GetComponent<Collider>());
        }

        m_IgnoreStage++;
    }
}
