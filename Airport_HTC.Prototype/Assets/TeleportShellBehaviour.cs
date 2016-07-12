using UnityEngine;
using System.Collections;

public class TeleportShellBehaviour : MonoBehaviour {

    public bool IsOffOnStart = false;
    private GameObject Teleporter;

    public Material m_InactiveMat;
    public Material m_ActiveMat;

    public Vector3 TeleportPoint;

    public Vector3 GetTelePoint() { return TeleportPoint; }

    void Start()
    {
        //Teleporter = transform.GetChild(0).gameObject;

        if (IsOffOnStart)
        {
            gameObject.GetComponent<MeshCollider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void IsActive(bool _active)
    {
        if (_active == true)
        {
            gameObject.GetComponent<MeshCollider>().enabled = true;
            gameObject.gameObject.GetComponent<MeshRenderer>().enabled = true;
            //Teleporter.GetComponent<TeleporterBehaviour>().IsActive(true);
        }

        else
        {
            gameObject.GetComponent<MeshCollider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            //Teleporter.GetComponent<TeleporterBehaviour>().IsActive(false);
        }
    }



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

}
