using UnityEngine;
using System.Collections;

public class Node{

	public Vector3 Position { get; private set; }
    public Vector2 Index { get; private set; }
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
    public Node(Vector3 pos, Vector2 index, bool walkable)
    {
        Position = pos;
        Index = index;
        Walkable = walkable;
        G = 0.0f;
        H = 0.0f;
        F = 0.0F;
        State = NodeState.Untested;
    }

    public void SetH(Node target)
    {
        H = (target.Position - Position).magnitude;
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
