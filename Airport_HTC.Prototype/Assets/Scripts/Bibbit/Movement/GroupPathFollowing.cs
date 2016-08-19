using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroupPathFollowing : MonoBehaviour
{
    public float GroupSpeed = 1.0f;
    public PathNode PathToFollow;

    class GroupMember
    {
        public Transform Transform;
        public PathNode TargetNode;
        public float Ratio;
        public Vector3 movementOrigin;

        public GroupMember(Transform transform, PathNode targetNode)
        {
            Transform = transform;
            TargetNode = targetNode;
            Ratio = 0.0f;
            movementOrigin = transform.position;
        }
    }

    // Note: Group members are sorted from their distance to the last node. This way it's easier to find the closest neighbours on the path.
    // TODO: Space out members so they always keep a min distance between each other. clinel 2016-08-19.
    private LinkedList<GroupMember> m_GroupMembers = new LinkedList<GroupMember>();

    private List<PathNode> m_CachedPath = new List<PathNode>();

    void Awake()
    {
        Debug.Assert(PathToFollow != null);

        PathNode pathIterator = PathToFollow;
        while (pathIterator != null && !m_CachedPath.Contains(pathIterator))
        {
            m_CachedPath.Add(pathIterator);
            pathIterator = pathIterator.NextNode;
        }
    }

    void Update()
    {
        PathNode lastNodeOnPath = m_CachedPath[m_CachedPath.Count - 1];

        List<GroupMember> membersToResetPosition = new List<GroupMember>();

        LinkedListNode<GroupMember> currentMemberIterator = m_GroupMembers.First;
        while (currentMemberIterator != null)
        {
            GroupMember currentMember = currentMemberIterator.Value;

            if (currentMember.TargetNode != null)
            {
                Vector3 origin = currentMember.movementOrigin;
                Vector3 destination = currentMember.TargetNode.transform.position;

                float duration = Vector3.Distance(origin, destination) / GroupSpeed;

                currentMember.Ratio += Time.deltaTime / duration;
                currentMember.Transform.position = Vector3.Lerp(origin, destination, currentMember.Ratio);

                if (currentMember.Ratio >= 1.0f)
                {
                    if (currentMember.TargetNode == lastNodeOnPath && currentMember.TargetNode.NextNode != null)
                    {
                        membersToResetPosition.Add(currentMember);
                    }
                    currentMember.TargetNode = currentMember.TargetNode.NextNode;
                    currentMember.movementOrigin = currentMember.Transform.position;
                    currentMember.Ratio = 0.0f;
                }
            }
            currentMemberIterator = currentMemberIterator.Next;
        }

        // Reset positons in the group.
        int nbMembersToReset = membersToResetPosition.Count;
        for (int i = 0; i < nbMembersToReset; ++i)
        {
            GroupMember memberToReset = membersToResetPosition[i];
            m_GroupMembers.Remove(memberToReset);
            InsertInGroup(memberToReset);
        }

        currentMemberIterator = m_GroupMembers.First;
        int count = 0;
        while (currentMemberIterator != null)
        {
            GroupMember currentMember = currentMemberIterator.Value;
            currentMember.Transform.GetComponent<TextMesh>().text = count.ToString();
            ++count;
            currentMemberIterator = currentMemberIterator.Next;
        }
    }

    LinkedListNode<GroupMember> FindGroupMember(Transform transformToFind)
    {
        LinkedListNode<GroupMember> currentNode = m_GroupMembers.First;
        while (currentNode != null && currentNode.Value.Transform != transformToFind)
        {
            currentNode = currentNode.Next;
        }
        return currentNode;
    }

    public void GetNeighbouringMembers(Transform referenceTransform, ref List<Transform> neighbours, int maxNbNeighbours)
    {
        PathNode lastNodeOnPath = m_CachedPath[m_CachedPath.Count - 1];

        LinkedListNode<GroupMember> referenceMemberIterator = FindGroupMember(referenceTransform);
        GroupMember referenceMember = referenceMemberIterator.Value;
        float referenceMemberDistanceToLastNode = DistanceOnPathToNode(referenceMember.TargetNode, lastNodeOnPath);

        LinkedListNode<GroupMember> backwardNextMemberIterator = referenceMemberIterator.Previous;
        if (backwardNextMemberIterator == null)
        {
            backwardNextMemberIterator = m_GroupMembers.Last;
        }

        LinkedListNode<GroupMember> forwardNextMemberIterator = referenceMemberIterator.Next;
        if (forwardNextMemberIterator == null)
        {
            forwardNextMemberIterator = m_GroupMembers.First;
        }

        // Only 2 members in the group
        if (forwardNextMemberIterator == backwardNextMemberIterator &&
        forwardNextMemberIterator != null &&
        forwardNextMemberIterator.Value != referenceMember)
        {
            Debug.Assert(m_GroupMembers.Count == 2);
            neighbours.Add(backwardNextMemberIterator.Value.Transform);
            maxNbNeighbours--;
        }

        // Spread along group from reference member and take closest neighbours.
        while (backwardNextMemberIterator != forwardNextMemberIterator &&
                (
                    (backwardNextMemberIterator.Next != null && backwardNextMemberIterator.Next != forwardNextMemberIterator) ||
                    (forwardNextMemberIterator.Previous != null && forwardNextMemberIterator.Previous != backwardNextMemberIterator)
                ) &&
                maxNbNeighbours > 0)
        {
            GroupMember backwardGroupMember = backwardNextMemberIterator.Value;
            float backwardMemberDistanceOnPathToReference = Vector3.Distance(backwardGroupMember.Transform.position, backwardGroupMember.TargetNode.transform.position);
            backwardMemberDistanceOnPathToReference += DistanceOnPathToNode(backwardGroupMember.TargetNode, lastNodeOnPath);
            backwardMemberDistanceOnPathToReference = Mathf.Abs(backwardMemberDistanceOnPathToReference - referenceMemberDistanceToLastNode);

            GroupMember forwardGroupMember = forwardNextMemberIterator.Value;
            float forwardMemberDistanceOnPathToReference = Vector3.Distance(forwardGroupMember.Transform.position, forwardGroupMember.TargetNode.transform.position);
            forwardMemberDistanceOnPathToReference += DistanceOnPathToNode(forwardGroupMember.TargetNode, lastNodeOnPath);
            forwardMemberDistanceOnPathToReference = Mathf.Abs(forwardMemberDistanceOnPathToReference - referenceMemberDistanceToLastNode);

            if (backwardMemberDistanceOnPathToReference < forwardMemberDistanceOnPathToReference)
            {
                neighbours.Add(backwardGroupMember.Transform);
                --maxNbNeighbours;

                if (backwardNextMemberIterator.Previous == null)
                {
                    backwardNextMemberIterator = m_GroupMembers.Last;
                }
                else
                {
                    backwardNextMemberIterator = backwardNextMemberIterator.Previous;
                }
            }
            else
            {
                neighbours.Add(forwardGroupMember.Transform);
                --maxNbNeighbours;

                if (forwardNextMemberIterator.Next == null)
                {
                    forwardNextMemberIterator = m_GroupMembers.First;
                }
                else
                {
                    forwardNextMemberIterator = forwardNextMemberIterator.Next;
                }
            }
        }
    }

    public void AddTransformToMove(Transform transformToMove)
    {
        Debug.Assert(FindGroupMember(transformToMove) == null);

        // Find closest path node
        Vector3 startPosition = transformToMove.position;
        float closestDistance = float.MaxValue;
        PathNode closestPathNode = null;

        int nbNodes = m_CachedPath.Count;
        for (int i = 0; i < nbNodes; ++i)
        {
            PathNode currentPathNode = m_CachedPath[i];
            float distanceToStart = Vector3.Distance(startPosition, currentPathNode.transform.position);

            if (distanceToStart < closestDistance)
            {
                closestDistance = distanceToStart;
                closestPathNode = currentPathNode;
            }
        }

        Debug.Assert(closestPathNode != null);
        GroupMember newGroupMember = new GroupMember(transformToMove, closestPathNode);
        InsertInGroup(newGroupMember);
    }

    /// Inserts at the correct location in the group. Group being sorted with the first member being the furthest from the last node.
    private void InsertInGroup(GroupMember newGroupMember)
    {
        PathNode lastNodeOnPath = m_CachedPath[m_CachedPath.Count - 1];

        LinkedListNode<GroupMember> currentMemberIterator = m_GroupMembers.First;
        float newMemberDistanceToLastNode = Vector3.Distance(newGroupMember.Transform.position, newGroupMember.TargetNode.transform.position);
        newMemberDistanceToLastNode += DistanceOnPathToNode(newGroupMember.TargetNode, lastNodeOnPath);

        while (currentMemberIterator != null)
        {
            GroupMember currentMember = currentMemberIterator.Value;

            float distanceToLastNode = 0f;
            if (currentMember.TargetNode != null)
            {
                distanceToLastNode = Vector3.Distance(currentMember.Transform.position, currentMember.TargetNode.transform.position);
                distanceToLastNode += DistanceOnPathToNode(currentMember.TargetNode, lastNodeOnPath);
            }

            if (newMemberDistanceToLastNode > distanceToLastNode)
            {
                break;
            }

            currentMemberIterator = currentMemberIterator.Next;
        }

        if (currentMemberIterator != null)
        {
            m_GroupMembers.AddBefore(currentMemberIterator, newGroupMember);
        }
        else
        {
            m_GroupMembers.AddFirst(newGroupMember);
        }
    }

    private float DistanceOnPathToNode(PathNode currentNode, PathNode destinationNode)
    {
        float result = 0.0f;

        PathNode currentNodeInitialValue = currentNode;

        do
        {
            if (currentNode.NextNode != null)
            {
                result += Vector3.Distance(currentNode.transform.position, currentNode.NextNode.transform.position);
            }
            currentNode = currentNode.NextNode;

        } while (currentNode != null && currentNode != destinationNode && currentNode != currentNodeInitialValue);

        return result;
    }

    public void RemoveTransformToMove(Transform transformToMove)
    {
        LinkedListNode<GroupMember> member = FindGroupMember(transformToMove);
        Debug.Assert(member != null);
        m_GroupMembers.Remove(member);
    }
}
