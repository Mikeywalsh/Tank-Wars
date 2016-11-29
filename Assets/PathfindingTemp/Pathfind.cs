using UnityEngine;
using System.Collections.Generic;

public class Pathfind : MonoBehaviour {

    public bool ShowInEditor;
    public Vector3 MapStart;
    public Vector3 StartPos;
    public Vector3 EndPos;

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
                NodeMap[x, y] = new Node(new Vector3(x, MapStart.y, y), Map[x,y]);

                NodeMap[x, y].SetH(EndPos, MapStart);
            }
        }

        path = FindPath();
        Debug.Log("OH MY" + path.Count.ToString());
    }

    private List<Node> GetAdjacentWalkableNodes(Node fromNode)
    {
        List<Node> walkableNodes = new List<Node>();
        List<Vector3> adjacentLocations = GetAdjacentLocations(fromNode.Position);

        foreach(Vector3 pos in adjacentLocations)
        {
            int x = Mathf.RoundToInt(pos.x);
            int y = Mathf.RoundToInt(pos.z);
            //Debug.Log(x.ToString());
            //Debug.Log(y.ToString());

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
                float tempG = fromNode.G + fromNode.GetTraversalCost(fromNode, node);
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
                float tempG = fromNode.G + fromNode.GetTraversalCost(fromNode, node);
                node.SetG(tempG);
                node.SetF();
                node.ParentNode = fromNode;
                node.State = Node.NodeState.Open;
                walkableNodes.Add(node);
            }
        }

        return walkableNodes;
    }

    private bool Search(Node currentNode)
    {
        currentNode.State = Node.NodeState.Closed;

        List<Node> adjacentNodes = GetAdjacentWalkableNodes(currentNode);
        adjacentNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));

        foreach (Node nextNode in adjacentNodes)
        {
            Debug.Log(nextNode.Position.ToString());
            if (nextNode.Position == new Vector3(4,0.5f,1))
            {
                Debug.Log("JESUS FUCK");
                endNode = nextNode;
                return true;
            }
            else
            {
                if (Search(nextNode))
                    return true;
            }
        }
        return false;
    }

    public List<Vector3> FindPath()
    {
        List<Vector3> path = new List<Vector3>();
        //Debug.Log(Mathf.RoundToInt(StartPos.x - MapStart.x).ToString());
        //Debug.Log(Mathf.RoundToInt(StartPos.z - MapStart.z).ToString());
        //bool success = Search(NodeMap[Mathf.RoundToInt(StartPos.x - MapStart.x), -1 *Mathf.RoundToInt(StartPos.z - MapStart.z)]);
        bool success = Search(NodeMap[1,1]);
        if (success)
        {
            Node node = endNode;
            while (node.ParentNode != null)
            {
                Vector3 temp = node.Position;
                temp.z *= -1;
                path.Add(temp);
                node = node.ParentNode;
            }
            path.Reverse();
        }
        return path;
    }

    private List<Vector3> GetAdjacentLocations(Vector3 pos)
    {
        List<Vector3> adjacentLocations = new List<Vector3>();
        adjacentLocations.Add(pos + new Vector3(1, 0, 0));
        adjacentLocations.Add(pos + new Vector3(0, 0, 1));
        adjacentLocations.Add(pos + new Vector3(-1, 0, 0));
        adjacentLocations.Add(pos + new Vector3(0, 0, -1));

        adjacentLocations.Add(pos + new Vector3(-1, 0, -1));
        adjacentLocations.Add(pos + new Vector3(1, 0, -1));
        adjacentLocations.Add(pos + new Vector3(-1, 0, -1));
        adjacentLocations.Add(pos + new Vector3(1, 0, 1));
        return adjacentLocations;
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
                //Gizmos.DrawCube(new Vector3(MapStart.x + x, 0.125f + (Map[x, y] ? 0 : 1), MapStart.z - y), new Vector3(1, 0.25f, 1));
            }
        }

    }
}
