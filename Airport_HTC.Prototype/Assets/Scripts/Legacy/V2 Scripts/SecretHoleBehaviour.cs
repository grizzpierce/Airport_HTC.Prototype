using UnityEngine;
using System.Collections;

public class SecretHoleBehaviour : MonoBehaviour {

    VRTK_InteractableObject m_InterObj;
    GameObject m_TouchingObject;

    bool m_AwakeVibration = false;

    public void SetAwakeVibration(bool _isvibrationawoken) { m_AwakeVibration = _isvibrationawoken; }

    // Use this for initialization
    void Start ()
    {
        m_InterObj = gameObject.GetComponent<VRTK_InteractableObject>();
        if (m_InterObj != null)
        {
            //Debug.Log("Script Located");
        }

    }



    void Update()
    {
        if (m_AwakeVibration == true)
        {
            if (m_InterObj.IsTouched() == true)
            {
                m_TouchingObject = m_InterObj.GetTouchingObject();
                m_TouchingObject.GetComponent<VRTK_ControllerActions>().TriggerHapticPulse(1, 125);
            }
        }
    }
}
