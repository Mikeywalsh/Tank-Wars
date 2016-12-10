using UnityEngine;
using System.Collections.Generic;

public class Pathfinder {
    private static bool[,] Map;
    private static Vector3 MapStart;

    private Point StartIndex;
    private Point EndIndex;    
    private Node[,] NodeMap;
    private List<Node> OpenNodes;

    public Pathfinder()
    {
        NodeMap = new Node[Map.GetLength(0), Map.GetLength(1)];

        for (int y = 0; y < Map.GetLength(1); y++)
        {
            for (int x = 0; x < Map.GetLength(0); x++)
            {
                NodeMap[x, y] = new Node(new Vector3(MapStart.x + x, MapStart.y, MapStart.z - y), new Point(x, y), Map[x, y]);
            }
        }
    }

    public List<Vector3> FindPath(Vector3 start, Vector3 end)
    {
        List<Vector3> path = new List<Vector3>();
        OpenNodes = new List<Node>();

        StartIndex = new Point(Mathf.RoundToInt(start.x - MapStart.x), Mathf.RoundToInt(MapStart.z - start.z));
        EndIndex = new Point(Mathf.RoundToInt(end.x - MapStart.x), Mathf.RoundToInt(MapStart.z - end.z));

        if (StartIndex == EndIndex || OutOfBounds(StartIndex) || OutOfBounds(EndIndex))
            return path;

        foreach (Node node in NodeMap)
        {
            node.Reset();
            node.SetH(NodeMap[EndIndex.X, EndIndex.Y]);
        }

        //Setup start node
        NodeMap[StartIndex.X, StartIndex.Y].SetG(0);
        NodeMap[StartIndex.X, StartIndex.Y].SetF();
        NodeMap[StartIndex.X, StartIndex.Y].State = Node.NodeState.Open;
        OpenNodes.Add(NodeMap[StartIndex.X, StartIndex.Y]);

        bool success = Search();
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

    private void Discover(Node fromNode)
    {
        List<Point> adjacentIndexs = GetAdjacentIndexs(fromNode.Index);

        foreach(Point index in adjacentIndexs)
        {
            int x = index.X;
            int y = index.Y;

            //Stay within level
            if (OutOfBounds(index))
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
                }
            }
            else
            {
                //If untested, set parent, flag as open and set G and F values
                float tempG = fromNode.G + fromNode.GetTraversalCost(node);
                node.SetG(tempG);
                node.SetF();
                node.ParentNode = fromNode;
                node.State = Node.NodeState.Open;

                if (!OpenNodes.Contains(node))
                    OpenNodes.Add(node);
            }
        }
    }

    private bool Search()
    {
        while(OpenNodes.Count != 0)
        {
            Node currentNode = OpenNodes[0];
            OpenNodes.RemoveAt(0);

            currentNode.State = Node.NodeState.Closed;

            Discover(currentNode);
            OpenNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));

            if (currentNode.Index == EndIndex)
                return true;

            Debug.Log(OpenNodes.Count.ToString());
        }
        return false;
    }

    private List<Point> GetAdjacentIndexs(Point index)
    {
        List<Point> adjacentIndexs = new List<Point>();
        adjacentIndexs.Add(index + new Point(1, 0));
        adjacentIndexs.Add(index + new Point(0, 1));
        adjacentIndexs.Add(index + new Point(-1, 0));
        adjacentIndexs.Add(index + new Point(0, -1));

        //temp corner cutting 
        if (!OutOfBounds(new Point(index.X + 1, index.Y + 1)))
        {
            if (NodeMap[index.X + 1, index.Y].Walkable && NodeMap[index.X, index.Y + 1].Walkable)
                adjacentIndexs.Add(index + new Point(1, 1));
        }
        if (!OutOfBounds(new Point(index.X - 1, index.Y + 1)))
        {
            if (NodeMap[index.X - 1, index.Y].Walkable && NodeMap[index.X, index.Y + 1].Walkable)
                adjacentIndexs.Add(index + new Point(-1, 1));
        }
        if (!OutOfBounds(new Point(index.X + 1, index.Y - 1)))
        {
            if (NodeMap[index.X + 1, index.Y].Walkable && NodeMap[index.X, index.Y - 1].Walkable)
                adjacentIndexs.Add(index + new Point(1, -1));
        }
        if (!OutOfBounds(new Point(index.X - 1, index.Y - 1)))
        {
            if (NodeMap[index.X - 1, index.Y].Walkable && NodeMap[index.X, index.Y - 1].Walkable)
                adjacentIndexs.Add(index + new Point(-1, -1));
        }

        return adjacentIndexs;
    }

    private bool OutOfBounds(Point p)
    {
        return (p.X < 0 || p.X >= NodeMap.GetLength(0) || p.Y < 0 || p.Y >= NodeMap.GetLength(1));
    }

    public static void SetMap(bool[,] map, Vector3 mapStart)
    {
        MapStart = mapStart;
        Map = map;
    }

    //void OnDrawGizmos()
    //{
    //    if (!ShowInEditor)
    //        return;

    //    for (int y = 0; y < Map.GetLength(1); y++)
    //    {
    //        for (int x = 0; x < Map.GetLength(0); x++)
    //        {
    //            Gizmos.color = Color.magenta;
    //            if (path.Count > 0)
    //                Gizmos.DrawLine(GameObject.Find("AI").transform.position, path[0]);
    //            for (int i = 0; i < path.Count - 1; i++)
    //            {
    //                Gizmos.DrawLine(path[i], path[i + 1]);
    //            }

    //            switch (NodeMap[x, y].State)
    //            {
    //                case Node.NodeState.Open:
    //                    Gizmos.color = Color.magenta;
    //                    break;
    //                case Node.NodeState.Closed:
    //                    Gizmos.color = Color.green;
    //                    break;
    //                case Node.NodeState.Untested:
    //                    Gizmos.color = Color.red;
    //                    break;
    //            }
    //            Gizmos.DrawCube(new Vector3(MapStart.x + x, 0.125f + (Map[x, y] ? 0 : 1), MapStart.z - y), new Vector3(1, 0.25f, 1));
    //        }
    //    }
    //}
}
