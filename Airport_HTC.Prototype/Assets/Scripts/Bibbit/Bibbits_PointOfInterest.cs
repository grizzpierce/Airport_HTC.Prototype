using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bibbits_PointOfInterest : MonoBehaviour
{
    public SkinnedMeshRenderer POIMeshRenderer;
    public Animation POIAnimation;

	public bool FadeOnDeactivate = true;

    public float Duration = 1.0f;
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

    public void Start()
    {
		if (POIMeshRenderer != null)
			POIMeshRenderer.enabled = false;

        PathLogic.OnTransformAtDestination += OnTransformAtDestination;
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

    public void DoActivation()
    {
		if (POIMeshRenderer != null)
			POIMeshRenderer.enabled = true;
		if (POIAnimation != null)
			POIAnimation.Play();
        ActivatePOI();
    }

    private void DeactivatePOI()
    {
        if (OnPOIDeactivated != null)
        {
            OnPOIDeactivated(this);
        }
    }

    public IEnumerator DeactivationCoroutine()
    {
        yield return new WaitForSeconds(Duration);

		if (FadeOnDeactivate) {
			float duration = DeactivationDuration;
			Color initialColor = POIMeshRenderer.material.color;
			Color tmpColor = initialColor;

			while (duration > 0) {
				tmpColor.a = DeactivationScaleCurve.Evaluate ((DeactivationDuration - duration) / DeactivationDuration);
				POIMeshRenderer.material.color = tmpColor;
				duration -= Time.deltaTime;
				yield return null;
			}

			POIMeshRenderer.enabled = false;
			POIMeshRenderer.material.color = initialColor;
			POIAnimation.Stop ();
		}

        DeactivatePOI();
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
