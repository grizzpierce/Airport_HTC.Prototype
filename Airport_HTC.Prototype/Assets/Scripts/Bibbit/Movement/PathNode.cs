using UnityEngine;

public class PathNode : MonoBehaviour
{
	public PathNode NextNode;

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		if (NextNode != null)
		{
			Gizmos.DrawLine(transform.position, NextNode.transform.position);
		}
		else
		{
			Gizmos.DrawCube(transform.position, Vector3.one);
		}
	}
}
