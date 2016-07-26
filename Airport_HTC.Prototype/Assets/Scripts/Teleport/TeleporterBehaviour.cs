using UnityEngine;
using System.Collections;

public class TeleporterBehaviour : MonoBehaviour
{
    public Material m_InactiveMat;
    public Material m_ActiveMat;

    public void Highlight(bool _isHighlighted)
    {
        if (_isHighlighted == true)
        {
            gameObject.GetComponent<Renderer>().material = m_ActiveMat;
        }

        else
        {
            gameObject.GetComponent<Renderer>().material = m_InactiveMat;
        }
    }


    public void IsActive(bool _active)
    {
        if (_active == true)
        {
            gameObject.GetComponent<SphereCollider>().enabled = true;
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        
        else
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
