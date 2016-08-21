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

    class DroppedBibbit
    {
        public Transform Transform;
        public Vector3 Origin;
        public Vector3 Ground;
        public float Ratio = 0f;
    }
    private List<DroppedBibbit> m_BibbitsToDrop = new List<DroppedBibbit>();

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

                    poi.RemoveTransformFromPOI(groupMemberTransform);
                    newPOI.AddTransformToPOI(groupMemberTransform);
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

                    poi.RemoveTransformFromPOI(groupMemberTransform);
                    PathLogic.AddTransformToMove(groupMemberTransform);
                }
            }
            else
            {
                int nbGroupMembers = m_GroupMembers.Count;
                for (int i = 0; i < nbGroupMembers; ++i)
                {
                    GameObject groupMember = m_GroupMembers[i];
                    Transform groupMemberTransform = groupMember.transform;

                    poi.RemoveTransformFromPOI(groupMemberTransform);
                }
            }
        }
    }

    // ADDS BIBBIT TO CONTAINED BIBBITS LIST
    public void AddBibbit(GameObject _bibbit)
    {
        m_GroupMembers.Add(_bibbit);

        // HACK: We simulate dropping the bibbits to the ground. Should use physics. clinel 2016-08-19.
        bool isGrounded = false;
        RaycastHit hit;
        if (Physics.Raycast(_bibbit.transform.position, Vector3.down, out hit, LayerMask.GetMask("Floor")))
        {
            isGrounded = hit.distance < DistToGround;
        }

        if (isGrounded)
        {
            AssignBibbit(_bibbit.transform);
        }
        else
        {
            DroppedBibbit droppedBibbit = new DroppedBibbit();
            droppedBibbit.Transform = _bibbit.transform;
            droppedBibbit.Ratio = 0.0f;
            droppedBibbit.Origin = _bibbit.transform.position;
            droppedBibbit.Ground = hit.point;

            if (m_BibbitsToDrop.Count > 0)
            {
                m_BibbitsToDrop.Add(droppedBibbit);
            }
            else
            {
                m_BibbitsToDrop.Add(droppedBibbit);
                StartCoroutine(DropBibbits());
            }
        }
    }

    // REMOVES BIBBIT TO CONTAINED BIBBITS LIST
    public void RemoveBibbit(GameObject _bibbit)
    {
        Debug.Assert(m_GroupMembers.Contains(_bibbit));
        m_GroupMembers.Remove(_bibbit);

        DroppedBibbit foundDropped = m_BibbitsToDrop.Find(b => b.Transform == _bibbit.transform);
        if (foundDropped != null)
        {
            // Note: Removing a dropping bibbit. Teleport it to the ground before removing it.
            m_BibbitsToDrop.Remove(foundDropped);
            foundDropped.Transform.position = foundDropped.Ground;
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
                DroppedBibbit bibbitToDrop = m_BibbitsToDrop[i];

                float duration = Vector3.Distance(bibbitToDrop.Origin, bibbitToDrop.Ground) / DropSpeed;
                bibbitToDrop.Ratio += Time.deltaTime / duration;
                // TODO: Use better than Lerp. clinel 2016-08-19.
                bibbitToDrop.Transform.position = Vector3.Lerp(bibbitToDrop.Origin, bibbitToDrop.Ground, bibbitToDrop.Ratio);
                if (bibbitToDrop.Ratio >= 1f)
                {
                    m_BibbitsToDrop.RemoveAt(i);

                    AssignBibbit(bibbitToDrop.Transform);
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
        else
        {
            PathLogic.GetNeighbouringMembers(referenceBibbit, ref neighbours, maxNbNeighbours);
        }
    }
}
