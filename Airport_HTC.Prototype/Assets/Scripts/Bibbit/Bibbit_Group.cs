using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Bibbit_Group : MonoBehaviour
{
    public float DistToGround = 1.0f;
    public float DropSpeed = 15.0f;
    public int MinGroupSize = 0;
    public GroupPathFollowing PathLogic;
    public List<Bibbits_PointOfInterest> POIs;
    public Bibbit_LineSpawner Spawner;

    private List<Transform> m_BibbitsToDrop = new List<Transform>();

    private List<GameObject> m_GroupMembers = new List<GameObject>();

    private List<Bibbits_PointOfInterest> m_ActivePOIs = new List<Bibbits_PointOfInterest>();

    public void Start()
    {
        GroupManager.Instance.RegisterGroup(this);

        int nbPOIs = POIs.Count;
        for (int i = 0; i < nbPOIs; ++i)
        {
            Bibbits_PointOfInterest poi = POIs[i];
            poi.OnPOIActivated += OnPOIActivated;
            poi.OnPOIDeactivated += OnPOIDeactivated;
        }
    }

    public void OnPOIActivated(Bibbits_PointOfInterest poi)
    {
        Debug.Assert(!m_ActivePOIs.Contains(poi));
        m_ActivePOIs.Add(poi);

        if (m_ActivePOIs.Count == 1)
        {
            int nbGroupMembers = m_GroupMembers.Count;
            for (int i = 0; i < nbGroupMembers; ++i)
            {
                GameObject groupMember = m_GroupMembers[i];
                Transform groupMemberTransform = groupMember.transform;

                if (PathLogic != null)
                {
                    PathLogic.RemoveTransformToMove(groupMemberTransform);
                }

                poi.AddTransformToPOI(groupMemberTransform);
            }
        }
    }

    public void OnPOIDeactivated(Bibbits_PointOfInterest poi)
    {
        Debug.Assert(m_ActivePOIs.Contains(poi));

        bool shouldRemoveFromPOI = false;

        if (m_ActivePOIs[0] == poi)
        {
            shouldRemoveFromPOI = true;
        }

        m_ActivePOIs.Remove(poi);

        if (m_ActivePOIs.Count > 0)
        {
            if (shouldRemoveFromPOI)
            {
                Bibbits_PointOfInterest newPOI = m_ActivePOIs[0];

                int nbGroupMembers = m_GroupMembers.Count;
                for (int i = 0; i < nbGroupMembers; ++i)
                {
                    GameObject groupMember = m_GroupMembers[i];
                    Transform groupMemberTransform = groupMember.transform;

					if (!m_BibbitsToDrop.Contains (groupMemberTransform)) {
						poi.RemoveTransformFromPOI (groupMemberTransform);
						newPOI.AddTransformToPOI (groupMemberTransform);
					}
                }
            }
        }
        else
        {
            Debug.Assert(shouldRemoveFromPOI);
            if (PathLogic != null)
            {
                int nbGroupMembers = m_GroupMembers.Count;
                for (int i = 0; i < nbGroupMembers; ++i)
                {
                    GameObject groupMember = m_GroupMembers[i];
                    Transform groupMemberTransform = groupMember.transform;

					if (!m_BibbitsToDrop.Contains (groupMemberTransform)) {
						poi.RemoveTransformFromPOI (groupMemberTransform);
						PathLogic.AddTransformToMove (groupMemberTransform);
					}
                }
            }
            else
            {
                int nbGroupMembers = m_GroupMembers.Count;
                for (int i = 0; i < nbGroupMembers; ++i)
                {
                    GameObject groupMember = m_GroupMembers[i];
                    Transform groupMemberTransform = groupMember.transform;

					if (!m_BibbitsToDrop.Contains (groupMemberTransform)) {
						poi.RemoveTransformFromPOI (groupMemberTransform);
					}
                }
            }
        }
    }

    // ADDS BIBBIT TO CONTAINED BIBBITS LIST
    public void AddBibbit(GameObject _bibbit)
    {
        m_GroupMembers.Add(_bibbit);
        if (m_BibbitsToDrop.Count > 0)
        {
            m_BibbitsToDrop.Add(_bibbit.transform);
        }
        else
        {
            m_BibbitsToDrop.Add(_bibbit.transform);
            StartCoroutine(DropBibbits());
        }
    }

    // REMOVES BIBBIT TO CONTAINED BIBBITS LIST
    public void RemoveBibbit(GameObject _bibbit)
    {
        Debug.Assert(m_GroupMembers.Contains(_bibbit));
        m_GroupMembers.Remove(_bibbit);

        Transform foundDropped = m_BibbitsToDrop.Find(b => b == _bibbit.transform);
        if (foundDropped != null)
        {
            // Note: Removing a dropping bibbit. Teleport it to the ground before removing it.
            m_BibbitsToDrop.Remove(foundDropped);
        }
        else
        {
            if (m_ActivePOIs.Count > 0)
            {
                m_ActivePOIs[0].RemoveTransformFromPOI(_bibbit.transform);
            }
            else
            {
                if (PathLogic != null)
                {
                    PathLogic.RemoveTransformToMove(_bibbit.transform);
                }
            }
        }

        if (Spawner != null)
        {
            if (m_GroupMembers.Count < MinGroupSize)
            {
                Debug.Log("Adding additional bibbit...");
                Spawner.SpawnAdditionalBibbits(1);
            }
        }
    }

    private IEnumerator DropBibbits()
    {
        while (m_BibbitsToDrop.Count > 0)
        {
            int nbBibbitsToDrop = m_BibbitsToDrop.Count;
            for (int i = nbBibbitsToDrop - 1; i >= 0; --i)
            {
                Transform bibbitToDrop = m_BibbitsToDrop[i];

                if (bibbitToDrop.GetComponent<IsGroundedLogic>().IsGrounded)
                {
                    m_BibbitsToDrop.RemoveAt(i);
                    bibbitToDrop.GetComponent<Rigidbody>().isKinematic = true;
                    bibbitToDrop.GetComponentInChildren<Animation>().Play();
                    AssignBibbit(bibbitToDrop);
                }
            }

            yield return null;
        }
    }

    private void AssignBibbit(Transform bibbitTransform)
    {
        if (m_ActivePOIs.Count > 0)
        {
            m_ActivePOIs[0].AddTransformToPOI(bibbitTransform);
        }
        else if (PathLogic != null)
        {
            PathLogic.AddTransformToMove(bibbitTransform);
        }
    }

    public void GetNeighbouringBibbits(Transform referenceBibbit, ref List<Transform> neighbours, int maxNbNeighbours)
    {
        if (m_ActivePOIs.Count > 0)
        {
            m_ActivePOIs[0].GetNeighbouringMembers(referenceBibbit, ref neighbours, maxNbNeighbours);
        }
        else if (PathLogic != null)
        {
            PathLogic.GetNeighbouringMembers(referenceBibbit, ref neighbours, maxNbNeighbours);
        }
    }
}
