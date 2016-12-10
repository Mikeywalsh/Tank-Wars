using UnityEngine;
using System.Collections;

/// <summary>
/// A node used for A* pathfinding.
/// </summary>
public class Node{

    /// <summary>
    /// World position of this node instance
    /// </summary>
	public Vector3 Position { get; private set; }
    /// <summary>
    /// Index of this node instance in relation to other node instances in the current map.
    /// </summary>
    public Point Index { get; private set; }
    /// <summary>
    /// Flags whether or not this node is able to be used
    /// </summary>
    public bool Walkable { get; set; }
    /// <summary>
    /// Distance travelled from the start node to this node
    /// </summary>
    public float G { get; private set; }
    /// <summary>
    /// Straight-line distance between this node and the destination node
    /// </summary>
    public float H { get; private set; }
    /// <summary>
    /// Sum of G and H
    /// </summary>
    public float F { get; private set; }
    /// <summary>
    /// Current state of the node
    /// </summary>
    public NodeState State { get; set; }
    /// <summary>
    /// Parent of this node, goes back to start node and symbolises shortest path
    /// </summary>
    public Node ParentNode { get; set; }

    /// <summary>
    /// Create a walkable node with given position
    /// </summary>
    /// <param name="pos">Position of the node</param>
    public Node(Vector3 pos, Point index, bool walkable)
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

    public void Reset()
    {
        G = 0.0f;
        H = 0.0f;
        F = 0.0F;
        State = NodeState.Untested;
        ParentNode = null;
    }

    /// <summary>
    /// Get the travel cost between this node instance and the provided node
    /// </summary>
    /// <param name="to">The node to get the distance to</param>
    /// <returns>The distance between the two nodes</returns>
    public float GetTraversalCost(Node to)
    {
        return (to.Position - Position).magnitude;
    }

    public override bool Equals(object obj)
    {
        return obj is Node && Index == ((Node)obj).Index;
    }

    public override int GetHashCode()
    {
        return Index.GetHashCode();
    }

    public enum NodeState
    {
        Untested = 0,
        Open,
        Closed
    }
}
