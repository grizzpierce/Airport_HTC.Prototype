using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroupPathFollowing : MonoBehaviour
{
    public delegate void OnTransformAtDestinationDelegate(Transform transform);
    public event OnTransformAtDestinationDelegate OnTransformAtDestination;

    public float MinDistanceOnPath = 0.5f;
    public float GroupSpeed = 1.0f;
    public PathNode PathToFollow;

    class GroupMember
    {
        public Transform Transform;
        public PathNode TargetNode;
        public float Ratio;
        public Vector3 movementOrigin;
        public float SpeedModifier;
        public float DistanceOnPathToLastNode;

        public GroupMember(Transform transform, PathNode targetNode)
        {
            Transform = transform;
            TargetNode = targetNode;
            Ratio = 0.0f;
            movementOrigin = transform.position;
            SpeedModifier = 1.0f;
            DistanceOnPathToLastNode = 0.0f;
        }
    }

    // Note: Group members are sorted from their distance to the last node (first members are closest to the last node). This way it's easier to find the closest neighbours on the path.
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

        // Adjust speed to keep minimal distance between members
        {
            if (m_GroupMembers.Count == 1)
            {
                m_GroupMembers.First.Value.SpeedModifier = 1.0f;
            }
            if (m_GroupMembers.Count > 1)
            {
                    m_GroupMembers.First.Value.SpeedModifier = 1.0f;
                GroupMember previousGroupMember = m_GroupMembers.First.Value;
                LinkedListNode<GroupMember> currentMemberIterator = m_GroupMembers.First.Next;
                while (currentMemberIterator != null)
                {
                    GroupMember currentMember = currentMemberIterator.Value;

                    if (currentMember.TargetNode != null && currentMember.TargetNode.NextNode != null)
                    {
                        float distanceOnPathToPreviousMember = currentMember.DistanceOnPathToLastNode - previousGroupMember.DistanceOnPathToLastNode;
                        if (distanceOnPathToPreviousMember < MinDistanceOnPath)
                        {
                            currentMember.SpeedModifier = MinDistanceOnPath > Mathf.Epsilon ? distanceOnPathToPreviousMember / MinDistanceOnPath : 0.0f;
                        }
                        else
                        {
                            currentMember.SpeedModifier = 1.0f;
                        }
                    }
                    else
                    {
                        currentMember.SpeedModifier = 1.0f;
                    }

                    previousGroupMember = currentMember;
                    currentMemberIterator = currentMemberIterator.Next;
                }
            }
        }

        // Move group members
        List<GroupMember> membersToResetPosition = new List<GroupMember>();
        List<GroupMember> membersAtDestination = new List<GroupMember>();
        {
            LinkedListNode<GroupMember> currentMemberIterator = m_GroupMembers.First;
            while (currentMemberIterator != null)
            {
                GroupMember currentMember = currentMemberIterator.Value;

                if (currentMember.TargetNode != null)
                {
                    Vector3 origin = currentMember.movementOrigin;
                    Vector3 destination = currentMember.TargetNode.transform.position;

                    float currentSpeed = GroupSpeed * currentMember.SpeedModifier;
                    if (currentSpeed > Mathf.Epsilon)
                    {
                        float duration = Vector3.Distance(origin, destination) / currentSpeed;
                        currentMember.Ratio += Time.deltaTime / duration;
                    }

                    currentMember.Transform.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(origin, destination, currentMember.Ratio));
//                    currentMember.Transform.position = Vector3.Lerp(origin, destination, currentMember.Ratio);

                    if (currentMember.Ratio >= 1.0f)
                    {
                        if (currentMember.TargetNode == lastNodeOnPath && currentMember.TargetNode.NextNode != null)
                        {
                            membersToResetPosition.Add(currentMember);
                        }
                        currentMember.TargetNode = currentMember.TargetNode.NextNode;
                        currentMember.movementOrigin = currentMember.Transform.position;
                        currentMember.Ratio = 0.0f;

                        if (currentMember.TargetNode == null)
                        {
                            membersAtDestination.Add(currentMember);
                        }
                    }
                }
                currentMemberIterator = currentMemberIterator.Next;
            }
        }

        // Members at destination.
        {
            int nbMembersAtDestination = membersAtDestination.Count;
            for (int i = 0; i < nbMembersAtDestination; ++i)
            {
                if (OnTransformAtDestination != null)
                {
                    OnTransformAtDestination(membersAtDestination[i].Transform);
                }
            }
        }

        // Update distance to last node on path
        {
            LinkedListNode<GroupMember> currentMemberIterator = m_GroupMembers.First;
            while (currentMemberIterator != null)
            {
                GroupMember currentMember = currentMemberIterator.Value;

                if (currentMember.TargetNode != null)
                {
                    float distanceOnPathToLastNode = Vector3.Distance(currentMember.Transform.position, currentMember.TargetNode.transform.position);
                    distanceOnPathToLastNode += DistanceOnPathToNode(currentMember.TargetNode, lastNodeOnPath);
                    currentMember.DistanceOnPathToLastNode = distanceOnPathToLastNode;
                }
                else
                {
                    currentMember.DistanceOnPathToLastNode = 0.0f;
                }
                currentMemberIterator = currentMemberIterator.Next;
            }
        }

        // Reset positons in the group.
        {
            int nbMembersToReset = membersToResetPosition.Count;
            for (int i = 0; i < nbMembersToReset; ++i)
            {
                GroupMember memberToReset = membersToResetPosition[i];
                m_GroupMembers.Remove(memberToReset);
            }
            for (int i = 0; i < nbMembersToReset; ++i)
            {
                GroupMember memberToReset = membersToResetPosition[i];
                InsertInGroup(memberToReset);
            }
        }

        // Debug
        {
            LinkedListNode<GroupMember> currentMemberIterator = m_GroupMembers.First;
            int count = 0;
            while (currentMemberIterator != null)
            {
                GroupMember currentMember = currentMemberIterator.Value;
                if (currentMember.TargetNode == m_CachedPath[m_CachedPath.Count - 1])
                {
                    currentMember.Transform.GetComponent<TextMesh>().text = "X" + count.ToString() + " " + currentMember.DistanceOnPathToLastNode.ToString("F2");//count.ToString();
                }
                else
                {
                    currentMember.Transform.GetComponent<TextMesh>().text = count.ToString() + " " + currentMember.DistanceOnPathToLastNode.ToString("F2");//count.ToString();
                }
                ++count;
                currentMemberIterator = currentMemberIterator.Next;
            }
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
            float backwardMemberDistanceOnPathToReference = Mathf.Abs(backwardGroupMember.DistanceOnPathToLastNode - referenceMemberDistanceToLastNode);

            GroupMember forwardGroupMember = forwardNextMemberIterator.Value;
            float forwardMemberDistanceOnPathToReference = Mathf.Abs(forwardGroupMember.DistanceOnPathToLastNode - referenceMemberDistanceToLastNode);

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

        PathNode lastNodeOnPath = m_CachedPath[m_CachedPath.Count - 1];
        float newMemberDistanceToLastNode = Vector3.Distance(newGroupMember.Transform.position, newGroupMember.TargetNode.transform.position);
        newMemberDistanceToLastNode += DistanceOnPathToNode(newGroupMember.TargetNode, lastNodeOnPath);
        newGroupMember.DistanceOnPathToLastNode = newMemberDistanceToLastNode;

        InsertInGroup(newGroupMember);
    }

    /// Inserts at the correct location in the group. Group being sorted with the first member being the closest from the last node.
    private void InsertInGroup(GroupMember newGroupMember)
    {
        LinkedListNode<GroupMember> currentMemberIterator = m_GroupMembers.First;

        while (currentMemberIterator != null)
        {
            GroupMember currentMember = currentMemberIterator.Value;

            if (newGroupMember.DistanceOnPathToLastNode < currentMember.DistanceOnPathToLastNode)
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
            m_GroupMembers.AddLast(newGroupMember);
        }
    }

    private float DistanceOnPathToNode(PathNode currentNode, PathNode destinationNode)
    {
        float result = 0.0f;

        if (currentNode != destinationNode)
        {
            PathNode currentNodeInitialValue = currentNode;

            do
            {
                if (currentNode.NextNode != null)
                {
                    result += Vector3.Distance(currentNode.transform.position, currentNode.NextNode.transform.position);
                }
                currentNode = currentNode.NextNode;

            } while (currentNode != null && currentNode != destinationNode && currentNode != currentNodeInitialValue);
        }
        return result;
    }

    public void RemoveTransformToMove(Transform transformToMove)
    {
        LinkedListNode<GroupMember> member = FindGroupMember(transformToMove);
        Debug.Assert(member != null);
        m_GroupMembers.Remove(member);
    }
}
