using UnityEngine;
using System.Collections;

public class GroundCollider : MonoBehaviour {

    private bool m_IsGrounded = false;

    public bool GetIsGrounded() { return m_IsGrounded; }

    void OnTriggerEnter(Collider col)
    {
        // Debug.Log(col.gameObject.name + " Entered " + gameObject.name + "'s Trigger!");

        if (col.gameObject.tag == "Ground")
        {
            m_IsGrounded = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        // Debug.Log(col.gameObject.name + " Exited " + gameObject.name + "'s Trigger!");

        if (col.gameObject.tag == "Ground")
        {
            m_IsGrounded = false;
        }
    }
}
