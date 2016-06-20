using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BibbitsMananger : MonoBehaviour {

    public GameObject[] m_BibbitHoles;
    public GameObject[] m_BibbitPrefabs;

    int m_CurrentHole;
    private GameObject m_CurrentBibbit = null;
    private GameObject m_PrevBibbit = null;
    private bool m_IsPlaying = false;


    int BibbitType;

    bool isBibbitPlaced;

    // Use this for initialization
    void Start ()
    {
        PlaceNewBibbit(Random.Range(0, m_BibbitPrefabs.Length));
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isBibbitPlaced == false)
        {
            PlaceNewBibbit(Random.Range(0, m_BibbitPrefabs.Length));
        }

        CheckHiddenBibbit();
        // Check if bibbit is still there
        // if bibbit is gone then confirm buffer
        /*
        if (m_PrevBibbit != null)
        {
            if (m_PrevBibbit.GetComponent<VRTK_InteractableObject>().IsGrabbed())
            {
                //Debug.Log("IsGrabbed");
                m_IsPlaying = false;
                m_CurrentBibbit.GetComponent<Bibbit_Behaviour>().AudioIO(false);

            }

            else if (m_PrevBibbit.GetComponent<VRTK_InteractableObject>().IsGrabbed() != true && m_IsPlaying == false)
            {
                //Debug.Log("IsntGrabbed");
                m_IsPlaying = true;
                m_CurrentBibbit.GetComponent<Bibbit_Behaviour>().AudioIO(true);
            }
        }
        */
    }

    private void PlaceNewBibbit(int _bibbittype)
    {
        //Debug.Log("New Bibbit Placed");
        m_CurrentHole = Random.Range(0, m_BibbitHoles.Length);
        m_CurrentBibbit = (GameObject)Instantiate(m_BibbitPrefabs[_bibbittype], m_BibbitHoles[m_CurrentHole].transform.FindChild("Bibbit Area").position, Quaternion.identity);
        m_CurrentBibbit.name = Random.Range(0, 100).ToString();
        m_CurrentBibbit.GetComponent<Bibbit_Behaviour>().FreezeBibbit();
        m_BibbitHoles[m_CurrentHole].GetComponentInChildren<SecretHoleBehaviour>().SetAwakeVibration(true);

        //Debug.Log("isBibbitPlaced: " + isBibbitPlaced);
        isBibbitPlaced = true;

        //m_CurrentBibbit.GetComponent<Bibbit_Behaviour>().SetMaterial(m_BibbitMaterials[Random.Range(0, m_BibbitMaterials.Length)]);
    }

    private void CheckHiddenBibbit()
    {
        if (m_CurrentBibbit.GetComponent<VRTK_InteractableObject>().IsGrabbed())
        {
            m_BibbitHoles[m_CurrentHole].GetComponentInChildren<SecretHoleBehaviour>().SetAwakeVibration(false);
            isBibbitPlaced = false;
            m_PrevBibbit = m_CurrentBibbit;
        } 
    }



}
