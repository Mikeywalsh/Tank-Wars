using UnityEngine;
using System.Collections;

public class Node{

	public Vector3 Position { get; private set; }
    public bool Walkable { get; set; }
    public float G { get; private set; }
    public float H { get; private set; }
    public float F { get; private set; }
    public NodeState State { get; set; }
    public Node ParentNode { get; set; }

    /// <summary>
    /// Create a walkable node with given position
    /// </summary>
    /// <param name="pos">Position of the node</param>
    public Node(Vector3 pos, bool walkable)
    {
        Position = pos;
        Walkable = walkable;
        G = 0.0f;
        H = 0.0f;
        F = 0.0F;
        State = NodeState.Untested;
    }

    public void SetH(Vector3 target, Vector3 mapOffset)
    {
        H = (target - new Vector3(mapOffset.x + Position.x, mapOffset.y, mapOffset.z - Position.z)).magnitude;
    }

    public void SetG(float g)
    {
        G = g;
    }

    public void SetF()
    {
        F = G + H;
    }

    public float GetTraversalCost(Node from, Node to)
    {
        return (to.Position - from.Position).magnitude;
    }

    public enum NodeState
    {
        Untested = 0,
        Open,
        Closed
    }
}
