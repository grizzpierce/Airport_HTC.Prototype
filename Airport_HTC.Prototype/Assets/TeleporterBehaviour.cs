using UnityEngine;
using System.Collections;

public class TeleporterBehaviour : MonoBehaviour
{
    public float FloorY;
    public bool m_IsTeleportOn;
    public Material m_InactiveMat;
    public Material m_ActiveMat;






    public float GetFloorY() { return FloorY; }

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

    public void TurnOff()
    {
        gameObject.GetComponent<SphereCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void TurnOn()
    {
        gameObject.GetComponent<SphereCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }



    void Start()
    {
        if(m_IsTeleportOn != true)
        {

        }
    }


}
