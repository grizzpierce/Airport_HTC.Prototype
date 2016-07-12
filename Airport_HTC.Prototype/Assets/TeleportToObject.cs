using UnityEngine;
using System.Collections;

public class TeleportToObject : MonoBehaviour
{
    // CONTROLLER 
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

    private GameObject m_LeftPointed;
    private GameObject m_RightPointed;


    void Start()
    {
        m_LeftPointer = m_LeftVive.GetComponent<VRTK_SimplePointer>();
        m_LeftController = m_LeftVive.GetComponent<VRTK_ControllerEvents>();

        m_RightPointer = m_RightVive.GetComponent<VRTK_SimplePointer>();
        m_RightController = m_RightVive.GetComponent<VRTK_ControllerEvents>();

        m_CurrentTeleportPoint.GetComponent<TeleportShellBehaviour>().IsActive(false);
    }

    void Update()
    {
        if (m_LeftPointer.getHitObject() != null)
        {
            if (m_LeftPointer.getHitObject().tag == "Teleport Point")
            {
                m_LeftPointed = m_LeftPointer.getHitObject();
                m_LeftPointed.GetComponent<TeleportShellBehaviour>().Highlight(true);

                if (m_LeftController.grabPressed && m_LeftTeleported == false)
                {

                    Debug.Log("Left Pointer hit: " + m_LeftPointer.getHitObject().name);
                    if (m_CurrentTeleportPoint != null) { m_CurrentTeleportPoint.GetComponent<TeleportShellBehaviour>().IsActive(true); }

                    m_LeftTeleported = true;
                    transform.position = m_LeftPointed.GetComponent<TeleportShellBehaviour>().GetTelePoint();

                    m_CurrentTeleportPoint = m_LeftPointer.getHitObject();
                    m_CurrentTeleportPoint.GetComponent<TeleportShellBehaviour>().IsActive(false);
                }
            }

            else
            {
                if (m_LeftPointed != null)
                {
                    m_LeftPointed.GetComponent<TeleportShellBehaviour>().Highlight(false);
                    m_LeftPointed = null;
                }
            }
        }
        else
        {
            if (m_LeftPointed != null)
            {
                m_LeftPointed.GetComponent<TeleportShellBehaviour>().Highlight(false);
                m_LeftPointed = null;
            }
        }

        if (m_RightPointer.getHitObject() != null)
        {
            if (m_RightPointer.getHitObject().tag == "Teleport Point")
            {
                m_RightPointed = m_RightPointer.getHitObject();
                m_RightPointed.GetComponent<TeleportShellBehaviour>().Highlight(true);

                if (m_RightController.grabPressed && m_RightTeleported == false)
                {
                    Debug.Log("Right Pointer hit: " + m_RightPointer.getHitObject().name);
                    if (m_CurrentTeleportPoint != null) { m_CurrentTeleportPoint.GetComponent<TeleportShellBehaviour>().IsActive(true); }

                    m_RightTeleported = true;
                    transform.position = m_RightPointed.GetComponent<TeleportShellBehaviour>().GetTelePoint();

                    m_CurrentTeleportPoint = m_RightPointer.getHitObject();
                    m_CurrentTeleportPoint.GetComponent<TeleportShellBehaviour>().IsActive(false);
                }
            }

            else
            {
                if (m_RightPointed != null)
                {
                    m_RightPointed.GetComponent<TeleportShellBehaviour>().Highlight(false);
                    m_RightPointed = null;
                }
            }
        }

        else
        {
            if (m_RightPointed != null)
            {
                m_RightPointed.GetComponent<TeleportShellBehaviour>().Highlight(false);
                m_RightPointed = null;
            }
        }

        if (m_LeftController.grabPressed != true && m_LeftTeleported == true)
        {
            m_LeftTeleported = false;
        }

        if (m_RightController.grabPressed != true && m_RightTeleported == true)
        {
            m_RightTeleported = false;
        }

    }
}
