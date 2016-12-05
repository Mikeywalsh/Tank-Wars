using UnityEngine;
using System.Collections.Generic;

public class Pathfind : MonoBehaviour {

    public bool ShowInEditor;
    public Vector3 MapStart;

    public Point StartIndex;
    public Point EndIndex;

    private bool[,] Map = new bool[,] { { true, true, true, true, true, true, true, true }, { true, true, true, true, true, false, true, true }, { true, true, true, true, true, false, true, true }, { false, false, false, false, false, false, true, true }, { true, true, true, true, true, false, true, true }, { true, true, true, true, true, false, true, true }, { true, true, true, true, true, true, true, true }, { true, true, true, true, true, false, true, true } };
    private Node[,] NodeMap;
    public List<Vector3> path = new List<Vector3>();

    private List<Node> OpenNodes = new List<Node>();

    void Start()
    {
        NodeMap = new Node[Map.GetLength(0), Map.GetLength(1)];

        for (int y = 0; y < Map.GetLength(1); y++)
        {
            for (int x = 0; x < Map.GetLength(0); x++)
            {
                NodeMap[x, y] = new Node(new Vector3(MapStart.x + x, MapStart.y, MapStart.z - y), new Point(x, y), Map[x, y]);
            }
        }

        foreach (Node node in NodeMap)
        {
            node.SetH(NodeMap[EndIndex.X, EndIndex.Y]);
        }

        path = FindPath();
        Debug.Log("Optimal path: " + path.Count.ToString() + " moves!");
    }

    private void Discover(Node fromNode)
    {
        List<Node> walkableNodes = new List<Node>();
        List<Point> adjacentIndexs = GetAdjacentIndexs(fromNode.Index);

        foreach(Point index in adjacentIndexs)
        {
            int x = index.X;
            int y = index.Y;

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

                if (!OpenNodes.Contains(node))
                    OpenNodes.Add(node);


            }
        }
    }

    private bool Search(Node currentNode)
    {
        currentNode.State = Node.NodeState.Closed;
        OpenNodes.RemoveAt(0);
        Discover(currentNode);
        Debug.Log(OpenNodes.Count.ToString());
        OpenNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));

        //foreach (Node nextNode in adjacentNodes)
        //{
            //Debug.Log(currentNode.Position.ToString());
            if (currentNode.Index == EndIndex)
            {
                return true;
            }
            else
            {
                if (Search(OpenNodes[0]))
                    return true;
            }
        //}
        return false;
    }

    public List<Vector3> FindPath()
    {
        List<Vector3> path = new List<Vector3>();

        //Setup start node
        NodeMap[StartIndex.X, StartIndex.Y].SetG(0);
        NodeMap[StartIndex.X, StartIndex.Y].SetF();
        NodeMap[StartIndex.X, StartIndex.Y].State = Node.NodeState.Open;
        OpenNodes.Add(NodeMap[StartIndex.X, StartIndex.Y]);

        //Debug.Log(Mathf.RoundToInt(StartPos.x - MapStart.x).ToString());
        //Debug.Log(Mathf.RoundToInt(StartPos.z - MapStart.z).ToString());
        //bool success = Search(NodeMap[Mathf.RoundToInt(StartPos.x - MapStart.x), -1 *Mathf.RoundToInt(StartPos.z - MapStart.z)]);
        bool success = Search(NodeMap[StartIndex.X, StartIndex.Y]);
        if (success)
        {
            Node node = NodeMap[EndIndex.X, EndIndex.Y];
            while (node.ParentNode != null)
            {
                path.Add(node.Position);
                node = node.ParentNode;
            }
            path.Reverse();
        }
        return path;
    }

    private List<Point> GetAdjacentIndexs(Point index)
    {
        List<Point> adjacentIndexs = new List<Point>();
        adjacentIndexs.Add(index + new Point(1, 0));
        adjacentIndexs.Add(index + new Point(0, 1));
        adjacentIndexs.Add(index + new Point(-1, 0));
        adjacentIndexs.Add(index + new Point(0, -1));

        //temp corner cutting 
        if (index.X + 1 < NodeMap.GetLength(0) && index.Y + 1 < NodeMap.GetLength(1))
        {
            if (NodeMap[index.X + 1, index.Y].Walkable && NodeMap[index.X, index.Y + 1].Walkable)
                adjacentIndexs.Add(index + new Point(1, 1));
        }
        if (index.X - 1 >= 0 && index.Y + 1 < NodeMap.GetLength(1))
        {
            if (NodeMap[index.X - 1, index.Y].Walkable && NodeMap[index.X, index.Y + 1].Walkable)
                adjacentIndexs.Add(index + new Point(-1, 1));
        }
        if (index.X + 1 < NodeMap.GetLength(0) && index.Y - 1 >= 0)
        {
            if (NodeMap[index.X + 1, index.Y].Walkable && NodeMap[index.X, index.Y - 1].Walkable)
                adjacentIndexs.Add(index + new Point(1, -1));
        }
        if (index.X - 1 >= 0 && index.Y - 1 >= 0)
        {
            if (NodeMap[index.X - 1, index.Y].Walkable && NodeMap[index.X, index.Y - 1].Walkable)
                adjacentIndexs.Add(index + new Point(-1, -1));
        }

        return adjacentIndexs;
    }

    void OnDrawGizmos()
    {
        if (!ShowInEditor)
            return;

        for (int y = 0; y < Map.GetLength(1); y++)
        {
            for (int x = 0; x < Map.GetLength(0); x++)
            {
                Gizmos.color = Color.magenta;
                if (path.Count > 0)
                    Gizmos.DrawLine(transform.position, path[0]);
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
