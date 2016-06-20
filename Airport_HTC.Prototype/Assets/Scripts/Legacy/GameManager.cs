using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    VRTK_ControllerEvents m_LeftController;
    VRTK_ControllerEvents m_RightController;

    //bool m_IsJengaBlocked = false;
    //public GameObject m_JengaBlock;
    //private GameObject m_LastBlock;

    //public GameObject m_FarMom;
    //public GameObject m_CloseMom;
    //bool m_IsMomClose = false;


    // Use this for initialization
    void Start ()
    {
        m_LeftController = transform.FindChild("Controller (left)").GetComponent<VRTK_ControllerEvents>();
        m_RightController = transform.FindChild("Controller (right)").GetComponent<VRTK_ControllerEvents>();

        if (m_LeftController != null)
        {
           // Debug.Log("Left is Loaded");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(m_LeftController.touchpadPressed == true || m_RightController.touchpadPressed == true)
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        /*

        if (m_LeftController.gripPressed == true && m_IsJengaBlocked == false)
        {
            m_LastBlock = (GameObject)Instantiate(m_JengaBlock, new Vector3(0, 2.5f, 0), Quaternion.identity);
            m_IsJengaBlocked = true;
        }

        if (m_LeftController.gripPressed == false)
        {
            m_IsJengaBlocked = false;
        }

        if (m_RightController.gripPressed == true && m_IsMomClose == false)
        {
            m_FarMom.GetComponent<Renderer>().enabled = false;
            m_CloseMom.GetComponent<Renderer>().enabled = true;
            m_IsMomClose = true;
        }

        if (m_RightController.gripPressed == false && m_IsMomClose == true)
        {
            m_FarMom.GetComponent<Renderer>().enabled = true;
            m_CloseMom.GetComponent<Renderer>().enabled = false;
            m_IsMomClose = false;
        }
        */

    }
}
