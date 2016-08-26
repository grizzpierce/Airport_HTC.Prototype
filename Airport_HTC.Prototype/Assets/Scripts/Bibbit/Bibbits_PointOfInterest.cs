using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bibbits_PointOfInterest : MonoBehaviour
{
    public float StartOffSet = 1.0f;

    public float Duration = 1.0f;
    public bool ShouldCoolDown = false;
    public float CoolDownDuration = 2.0f;
    public float ActivationDuration = 1f;
    public float DeactivationDuration = 1f;
    public AnimationCurve ActivationScaleCurve = new AnimationCurve();
    public AnimationCurve DeactivationScaleCurve = new AnimationCurve();


    public delegate void OnPOIActivatedDelegate(Bibbits_PointOfInterest poi);
    public delegate void OnPOIDeactivatedDelegate(Bibbits_PointOfInterest poi);

    public event OnPOIActivatedDelegate OnPOIActivated;
    public event OnPOIDeactivatedDelegate OnPOIDeactivated;

    public GroupPathFollowing PathLogic;

    private List<Transform> m_MembersAtDestination = new List<Transform>();
    private List<Transform> m_MembersInPOI = new List<Transform>();

    public IEnumerator Start()
    {
        GetComponent<MeshRenderer>().enabled = false;

        PathLogic.OnTransformAtDestination += OnTransformAtDestination;

        yield return new WaitForSeconds(StartOffSet);

        StartCoroutine(ActivateCoroutine());
    }

    public void OnDisable()
    {
        PathLogic.OnTransformAtDestination -= OnTransformAtDestination;
    }

    private void ActivatePOI()
    {
        if (OnPOIActivated != null)
        {
            OnPOIActivated(this);
        }
    }

    IEnumerator ActivateCoroutine()
    {
        GetComponent<MeshRenderer>().enabled = true;

        float duration = ActivationDuration;
        Vector3 initialLocalScale = transform.localScale;
        while (duration > 0)
        {
            transform.localScale = ActivationScaleCurve.Evaluate((ActivationDuration - duration)/ ActivationDuration) * initialLocalScale;
            duration -= Time.deltaTime;
            yield return null;
        }
        transform.localScale = initialLocalScale;
        ActivatePOI();
    }

    private void DeactivatePOI()
    {
        if (OnPOIDeactivated != null)
        {
            OnPOIDeactivated(this);
        }

        if (ShouldCoolDown)
        {
            StartCoroutine(CoolDownCoroutine());
        }
    }

    public IEnumerator DeactivationCoroutine()
    {
        yield return new WaitForSeconds(Duration);

        float duration = DeactivationDuration;
        Vector3 initialLocalScale = transform.localScale;
        while (duration > 0)
        {
            transform.localScale = DeactivationScaleCurve.Evaluate((DeactivationDuration - duration) / DeactivationDuration) * initialLocalScale;
            duration -= Time.deltaTime;
            yield return null;
        }
        transform.localScale = initialLocalScale;

        GetComponent<MeshRenderer>().enabled = false;

        DeactivatePOI();
    }

    IEnumerator CoolDownCoroutine()
    {
        yield return new WaitForSeconds(CoolDownDuration);
        StartCoroutine(ActivateCoroutine());
    }

    public void AddTransformToPOI(Transform transformToPOI)
    {
        Debug.Assert(!m_MembersInPOI.Contains(transformToPOI));
        m_MembersInPOI.Add(transformToPOI);
        PathLogic.AddTransformToMove(transformToPOI);
    }

    public void RemoveTransformFromPOI(Transform transformToPOI)
    {
        Debug.Assert(m_MembersInPOI.Contains(transformToPOI));
        m_MembersInPOI.Remove(transformToPOI);

        PathLogic.RemoveTransformToMove(transformToPOI);

        if (m_MembersAtDestination.Contains(transformToPOI))
        {
            m_MembersAtDestination.Remove(transformToPOI);
            // TODO: Abort deactivation process. clinel. 2016-08-21.
        }
    }

    public void OnTransformAtDestination(Transform transform)
    {
        Debug.Assert(!m_MembersAtDestination.Contains(transform));

        m_MembersAtDestination.Add(transform);
        if (m_MembersAtDestination.Count == 1)
        {
            StartCoroutine(DeactivationCoroutine());
        }
    }

    public void GetNeighbouringMembers(Transform referenceBibbit, ref List<Transform> neighbours, int maxNbNeighbours)
    {
        int nbMembers = m_MembersInPOI.Count;
        for (int i = nbMembers - 1; i >= 0; --i)
        {
            if (m_MembersInPOI[i] != referenceBibbit)
            {
                neighbours.Add(m_MembersInPOI[i]);
                --maxNbNeighbours;
                if (maxNbNeighbours == 0)
                    break;
            }
        }
    }

}
