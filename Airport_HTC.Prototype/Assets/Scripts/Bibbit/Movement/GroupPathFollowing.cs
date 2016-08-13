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

    // Note: LinkedList because I would like members to be in the same order as they are on the path to do some logic like evenly spacing each other and dropping a member anywhere on the path. clinel 2016-08-13.
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
                    currentMember.TargetNode = currentMember.TargetNode.NextNode;
                    currentMember.movementOrigin = currentMember.Transform.position;
                    currentMember.Ratio = 0.0f;
                }
            }
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
        m_GroupMembers.AddFirst(new GroupMember(transformToMove, closestPathNode));
    }

    public void RemoveTransformToMove(Transform transformToMove)
    {
        LinkedListNode<GroupMember> member = FindGroupMember(transformToMove);
        Debug.Assert(member != null);
        m_GroupMembers.Remove(member);
    }


}
