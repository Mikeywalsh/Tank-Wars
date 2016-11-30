using UnityEngine;
using System.Collections.Generic;

public class Pathfind : MonoBehaviour {

    public bool ShowInEditor;
    public Vector3 MapStart;
    public Vector2 StartIndex;
    public Vector2 EndIndex;

    private Node endNode;
    private bool[,] Map = new bool[,] { { true, true, true, true, true, true, true, true }, { true, true, true, true, true, false, true, true }, { true, true, true, true, true, false, true, true }, { false, false, false, false, false, false, true, true }, { true, true, true, true, true, false, true, true }, { true, true, true, true, true, false, true, true }, { true, true, true, true, true, true, true, true }, { true, true, true, true, true, false, true, true } };
    private Node[,] NodeMap;
    private List<Vector3> path = new List<Vector3>();

    void Start()
    {
        NodeMap = new Node[Map.GetLength(0), Map.GetLength(1)];

        for(int y = 0; y < Map.GetLength(1); y++)
        {
            for(int x = 0; x < Map.GetLength(0); x++)
            {
                NodeMap[x, y] = new Node(new Vector3(MapStart.x + x, MapStart.y, MapStart.z - y),new Vector2(x,y), Map[x,y]);
            }
        }

        for (int y = 0; y < Map.GetLength(1); y++)
        {
            for (int x = 0; x < Map.GetLength(0); x++)
            {
                NodeMap[x, y].SetH(NodeMap[(int)EndIndex.x, (int)EndIndex.y]);
            }
        }

        path = FindPath();
        Debug.Log("Optimal path: " + path.Count.ToString() + " moves!");
    }

    private List<Node> GetAdjacentWalkableNodes(Node fromNode)
    {
        List<Node> walkableNodes = new List<Node>();
        List<Vector2> adjacentIndexs = GetAdjacentIndexs(fromNode.Index);

        foreach(Vector2 index in adjacentIndexs)
        {
            int x = (int)index.x;
            int y = (int)index.y;

            //Stay within level
            if (x < 0 || x >= NodeMap.GetLength(0) || y < 0 || y >= NodeMap.GetLength(1))
                continue;

            Node node = NodeMap[x, y];
            //Only consider walkable Nodes
            if (!node.Walkable)
                continue;

            //Ignore closed nodes
            if (node.State == Node.NodeState.Closed)
                continue;

            //Already-open nodes are only added to the list if their G-value is lower via this route
            if(node.State == Node.NodeState.Open)
            {
                float tempG = fromNode.G + fromNode.GetTraversalCost(node);
                if (tempG < node.G)
                {
                    node.SetG(tempG);
                    node.SetF();
                    node.ParentNode = fromNode;
                    walkableNodes.Add(node);
                }
            }
            else
            {
                //If untested, set parent and flag as open and set G and F values
                float tempG = fromNode.G + fromNode.GetTraversalCost(node);
                node.SetG(tempG);
                node.SetF();
                node.ParentNode = fromNode;
                node.State = Node.NodeState.Open;
                walkableNodes.Add(node);
            }
        }

        return walkableNodes;
    }

    private Node LowestF()
    {
        float min = float.MaxValue;
        Node minNode = NodeMap[0, 0];

        foreach(Node n in NodeMap)
        {
            if (n.State != Node.NodeState.Closed && n.F != 0 && n.F < min)
            {
                min = n.F;
                minNode = n;
            }
        }
        return minNode;
    }

    private bool Search(Node currentNode)
    {
        currentNode.State = Node.NodeState.Closed;

        List<Node> adjacentNodes = GetAdjacentWalkableNodes(currentNode);
        //adjacentNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));

        //foreach (Node nextNode in adjacentNodes)
        //{
            //Debug.Log(currentNode.Position.ToString());
            if (currentNode.Index == EndIndex)
            {
                Debug.Log("JESUS FUCK");
                endNode = currentNode;
                return true;
            }
            else
            {
                if (Search(LowestF()))
                    return true;
            }
        //}
        return false;
    }

    public List<Vector3> FindPath()
    {
        List<Vector3> path = new List<Vector3>();
        NodeMap[(int)StartIndex.x, (int)StartIndex.y].SetG(0);
        NodeMap[(int)StartIndex.x, (int)StartIndex.y].SetF();
        //Debug.Log(Mathf.RoundToInt(StartPos.x - MapStart.x).ToString());
        //Debug.Log(Mathf.RoundToInt(StartPos.z - MapStart.z).ToString());
        //bool success = Search(NodeMap[Mathf.RoundToInt(StartPos.x - MapStart.x), -1 *Mathf.RoundToInt(StartPos.z - MapStart.z)]);
        bool success = Search(NodeMap[(int)StartIndex.x, (int)StartIndex.y]);
        if (success)
        {
            Node node = endNode;
            while (node.ParentNode != null)
            {
                //Vector3 temp = node.Position;
                //temp.z *= -1;
                path.Add(node.Position);
                node = node.ParentNode;
            }
            path.Reverse();
        }
        return path;
    }

    private List<Vector2> GetAdjacentIndexs(Vector2 index)
    {
        List<Vector2> adjacentIndexs = new List<Vector2>();
        adjacentIndexs.Add(index + new Vector2(1, 0));
        adjacentIndexs.Add(index + new Vector2(0, 1));
        adjacentIndexs.Add(index + new Vector2(-1, 0));
        adjacentIndexs.Add(index + new Vector2(0, -1));

        //temp corner cutting 
        if (index.x + 1 < NodeMap.GetLength(0) && index.y + 1 < NodeMap.GetLength(1))
        {
            if(NodeMap[(int)(index.x + 1), (int)(index.y)].Walkable && NodeMap[(int)(index.x), (int)(index.y + 1)].Walkable)
                adjacentIndexs.Add(index + new Vector2(1, 1));
        }
        if (index.x - 1 >= 0 && index.y + 1 < NodeMap.GetLength(1))
        {
            if (NodeMap[(int)(index.x - 1), (int)(index.y)].Walkable && NodeMap[(int)(index.x), (int)(index.y + 1)].Walkable)
                adjacentIndexs.Add(index + new Vector2(-1, 1));
        }
        if (index.x + 1 < NodeMap.GetLength(0) && index.y - 1 >= 0)
        {
            if (NodeMap[(int)(index.x + 1), (int)(index.y)].Walkable && NodeMap[(int)(index.x), (int)(index.y - 1)].Walkable)
                adjacentIndexs.Add(index + new Vector2(1, -1));
        }
        if (index.x - 1 >= 0 && index.y - 1 >= 0)
        {
            if (NodeMap[(int)(index.x - 1), (int)(index.y)].Walkable && NodeMap[(int)(index.x), (int)(index.y - 1)].Walkable)
                adjacentIndexs.Add(index + new Vector2(-1, -1));
        }

        return adjacentIndexs;
    }

    void OnDrawGizmos()
    {
        if (!ShowInEditor)
            return;

        for(int y = 0; y < Map.GetLength(1); y++)
        {
            for(int x = 0; x < Map.GetLength(0); x++)
            {
                Gizmos.color = Color.magenta;
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Gizmos.DrawLine(path[i], path[i + 1]);
                }
                //Gizmos.color = Map[x, y] ? Color.green : Color.red;
                
                switch (NodeMap[x, y].State)
                {
                    case Node.NodeState.Open:
                        Gizmos.color = Color.magenta;
                        break;
                    case Node.NodeState.Closed:
                        Gizmos.color = Color.green;
                        break;
                    case Node.NodeState.Untested:
                        Gizmos.color = Color.red;
                        break;
                }
                Gizmos.DrawCube(new Vector3(MapStart.x + x, 0.125f + (Map[x, y] ? 0 : 1), MapStart.z - y), new Vector3(1, 0.25f, 1));
            }
        }

    }
}
