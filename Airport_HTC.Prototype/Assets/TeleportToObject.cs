using UnityEngine;
using System.Collections;

public class TeleportToObject : MonoBehaviour
{

    public GameObject m_LeftVive;
    VRTK_SimplePointer m_LeftPointer;
    VRTK_ControllerEvents m_LeftController;

    public GameObject m_RightVive;
    VRTK_SimplePointer m_RightPointer;
    VRTK_ControllerEvents m_RightController;

    bool m_LeftTeleported = false;
    bool m_RightTeleported = false;

    public GameObject m_CurrentTeleportPoint;
    GameObject m_PrevTeleportPoint;

    // Use this for initialization
    void Start()
    {
        m_LeftPointer = m_LeftVive.GetComponent<VRTK_SimplePointer>();
        m_LeftController = m_LeftVive.GetComponent<VRTK_ControllerEvents>();

        m_RightPointer = m_RightVive.GetComponent<VRTK_SimplePointer>();
        m_RightController = m_RightVive.GetComponent<VRTK_ControllerEvents>();

        m_CurrentTeleportPoint.GetComponent<TeleporterBehaviour>().TurnOff();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_LeftPointer.getHitObject() != null)
        {
            if (m_LeftPointer.getHitObject().tag == "Teleport Point")
            {
                if (m_LeftController.gripPressed && m_LeftController.grabPressed && m_LeftTeleported == false)
                {
                    Debug.Log("Left Pointer hit: " + m_LeftPointer.getHitObject().name);
                    if (m_CurrentTeleportPoint != null) { m_CurrentTeleportPoint.GetComponent<TeleporterBehaviour>().TurnOn(); }

                    m_LeftTeleported = true;
                    //Debug.Log(transform.parent.gameObject.name);
                    transform.position = new Vector3(m_LeftPointer.getHitObject().transform.position.x, m_LeftPointer.getHitObject().GetComponent<TeleporterBehaviour>().GetFloorY(), m_LeftPointer.getHitObject().transform.position.z);

                    m_CurrentTeleportPoint = m_LeftPointer.getHitObject();
                    m_CurrentTeleportPoint.GetComponent<TeleporterBehaviour>().TurnOff();

                    Debug.Log("TELEPORT!!!");
                }
            }
        }

        if (m_RightPointer.getHitObject() != null)
        {
            if (m_RightPointer.getHitObject().tag == "Teleport Point")
            {
                if (m_RightController.gripPressed && m_RightController.grabPressed && m_RightTeleported == false)
                {
                    Debug.Log("Right Pointer hit: " + m_RightPointer.getHitObject().name);
                    if (m_CurrentTeleportPoint != null) { m_CurrentTeleportPoint.GetComponent<TeleporterBehaviour>().TurnOn(); }

                    m_RightTeleported = true;
                    //Debug.Log(transform.parent.gameObject.name);
                    transform.position = new Vector3(m_RightPointer.getHitObject().transform.position.x, m_RightPointer.getHitObject().GetComponent<TeleporterBehaviour>().GetFloorY(), m_RightPointer.getHitObject().transform.position.z);

                    m_CurrentTeleportPoint = m_RightPointer.getHitObject();
                    m_CurrentTeleportPoint.GetComponent<TeleporterBehaviour>().TurnOff();

                    Debug.Log("TELEPORT!!!");
                }
            }
        }

        if (m_LeftController.gripPressed != true && m_LeftController.grabPressed != true && m_LeftTeleported == true)
        {
            m_LeftTeleported = false;
        }

        if (m_RightController.gripPressed != true && m_RightController.grabPressed != true && m_RightTeleported == true)
        {
            m_RightTeleported = false;
        }

    }
}
