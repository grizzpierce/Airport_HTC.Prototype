using UnityEngine;
using System.Collections;

public class TeleporterBehaviour : MonoBehaviour
{

    public float FloorY;

    public float GetFloorY() { return FloorY; }

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
}
