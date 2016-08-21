using UnityEngine;
using System.Collections;

public class NoVRGrabbing : MonoBehaviour
{
    public Transform BibbitHolder;

    void Awake()
    {
        Debug.Assert(BibbitHolder != null);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit result;

        if (Physics.Raycast(transform.position, transform.forward, out result))
        {
            if (result.transform.CompareTag("Bibbit"))
            {
                Debug.Log("Hit bibbit!", result.transform);

                if (Input.GetMouseButton(0))
                {
                    VRTK_InteractableObject interactableObject = result.transform.GetComponent<VRTK_InteractableObject>();
                    Debug.Assert(interactableObject != null);
                    InteractableObjectEventArgs args = new InteractableObjectEventArgs();
                    args.interactingObject = result.transform.gameObject;
                    interactableObject.OnInteractableObjectGrabbed(args);
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            int nbChildren = BibbitHolder.childCount;
            for (int i = nbChildren - 1; i >= 0; --i)
            {
                Transform holderChild = BibbitHolder.GetChild(i);
                if (holderChild.CompareTag("Bibbit"))
                {
                    VRTK_InteractableObject interactableObject = holderChild.GetComponent<VRTK_InteractableObject>();
                    Debug.Assert(interactableObject != null);
                    InteractableObjectEventArgs args = new InteractableObjectEventArgs();
                    args.interactingObject = holderChild.gameObject;

                    interactableObject.OnInteractableObjectUngrabbed(args);
                    break;
                }
            }
        }
    }
}
