using UnityEngine;
using System.Collections;

public class Bibbit_Stick : MonoBehaviour {

    private GameObject m_StuckToObject;
    private VRTK_ControllerEvents m_Controller;

    private bool m_Sticking = true;

    public Animation m_Anim;
    public AnimationClip m_Orbit;

    public void SetStuckObject(GameObject _obj)
    {
        m_StuckToObject = _obj;
        m_Controller = _obj.GetComponent<VRTK_ControllerEvents>();
    }

    public void PassAnimation(AnimationClip _clip)
    {
        m_Orbit = _clip;
    }

    void Start()
    {
        if (gameObject.GetComponent<Bibbit_Movement>() != null)
        {
            Destroy(gameObject.GetComponent<Bibbit_Movement>());
        }

        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            Destroy(gameObject.GetComponent<Rigidbody>());
        }

        m_Anim = transform.FindChild("bibbit_mesh").GetComponent<Animation>();

        if (gameObject.GetComponentInChildren<Animation>() != null)
        {
            m_Anim.Stop();
        }

    }
	
    void Update()
    {
        if (!m_Anim.isPlaying && m_Sticking)
        {
            m_Anim.PlayQueued("Spin");
        }

        if (m_Controller.gripPressed == true)
        {
            // Drop that shit
            gameObject.AddComponent<Rigidbody>();
            m_Anim.Stop();
            gameObject.GetComponent<SphereCollider>().isTrigger = false;
            gameObject.GetComponent<SphereCollider>().center = new Vector3(0, 0, 0);
            m_Sticking = false;
            gameObject.AddComponent<Bibbit_Search>();
        }
    }

	void FixedUpdate ()
    {
	    if (m_StuckToObject != null && m_Sticking)
        {
            gameObject.transform.position = m_StuckToObject.transform.position;
        }
	}
}
