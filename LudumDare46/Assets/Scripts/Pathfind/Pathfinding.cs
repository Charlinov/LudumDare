using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private RG_Node[,] pathFindMap;
    public int mapWidth;
    public int mapHeight;

    private List<RG_Node> openList;
    private List<RG_Node> closedList;

    private const int MOVE_COST = 10;


    public static Pathfinding sInstance { get; private set; }

    public Pathfinding(int width, int height)
    {
        sInstance = this;
        mapWidth = width;
        mapHeight = height;
        pathFindMap = new RG_Node[mapWidth, mapHeight];
        Debug.Log(mapHeight + "  " + mapHeight);
        // initalize me plz
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                pathFindMap[x, y] = new RG_Node(x, y);
            }
        }
    }

    public List<Vector3> FindPath(Vector3 start, Vector3 end)
    {
        Vector2 gridStart = worldToGridSpace(start);
        Vector2 gridEnd = worldToGridSpace(end);

        List<RG_Node> path = FindPath((int)gridStart.x, (int)gridStart.y, (int)gridEnd.x, (int)gridEnd.y);
        if(path == null)
        {
            return null;
        }
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (RG_Node node in path)
            {
                vectorPath.Add(gridToWorldSpace(new Vector2(node.x, node.y)));
            }
            return vectorPath;
        }

    }

    private List<RG_Node> FindPath(int xStart, int yStart, int xEnd, int yEnd)
    {
        RG_Node startNode = pathFindMap[xStart, yStart];
        RG_Node endNode = pathFindMap[xEnd, yEnd];

        openList = new List<RG_Node> { startNode };
        closedList = new List<RG_Node>();

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                RG_Node node = pathFindMap[x, y];
                node.gCost = int.MaxValue;
                node.CalculateFCost();
                node.prevNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = calculateDistance(startNode, endNode);
        startNode.CalculateFCost();


        while(openList.Count > 0)
        {
            RG_Node currNode = GetLowestFCostNode(openList);
            if(currNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currNode);
            closedList.Add(currNode);

            foreach (RG_Node node in getNodeNeighbours(currNode))
            {
                if (closedList.Contains(node)) continue;
                if(!node.isWalkable)
                {
                    closedList.Add(node);
                    continue;
                }

                int tentativeGCost = currNode.gCost + calculateDistance(currNode, node);

                if(tentativeGCost < node.gCost)
                {
                    node.prevNode = currNode;
                    node.gCost = tentativeGCost;
                    node.hCost = calculateDistance(node, endNode);
                    node.CalculateFCost();

                    if(!openList.Contains(node))
                    {
                        openList.Add(node);
                    }
                }
            }
        }

        // out of nodes on open list!
        return null;
    }

    private List<RG_Node> getNodeNeighbours(RG_Node node)
    {
        List<RG_Node> neighbourList = new List<RG_Node>();
        //left
        if (node.x - 1 >= 0)
            neighbourList.Add(pathFindMap[node.x - 1, node.y]);
        //right
        if(node.x + 1 < mapWidth)
            neighbourList.Add(pathFindMap[node.x + 1, node.y]);
        //up
        if (node.y - 1 >= 0)
            neighbourList.Add(pathFindMap[node.x, node.y - 1]);
        //down
        if (node.y + 1 < mapHeight)
            neighbourList.Add(pathFindMap[node.x, node.y + 1]);


        return neighbourList;
    }

    private List<RG_Node> CalculatePath(RG_Node endNode)
    {
        List<RG_Node> path = new List<RG_Node>();
        path.Add(endNode);

        RG_Node currNode = endNode;
        while(currNode.prevNode != null)
        {
            path.Add(currNode.prevNode);
            currNode = currNode.prevNode;
        }

        path.Reverse();
        return path;
    }

    private int calculateDistance(RG_Node a, RG_Node b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);

        return xDistance + yDistance;
    }

    private RG_Node GetLowestFCostNode(List<RG_Node> nodeList)
    {
        RG_Node lowestFCostNode = nodeList[0];
        for (int i = 0; i < nodeList.Count; i++)
        {
            if (nodeList[i].fCost < lowestFCostNode.fCost)
                lowestFCostNode = nodeList[i];
        }

        return lowestFCostNode;
    }


    private Vector2 worldToGridSpace(Vector3 worldSpace)
    {
        Vector2 gridSpace;
        gridSpace.x = (int)(worldSpace.x + mapWidth / 2);
        gridSpace.y = (int)(worldSpace.y + mapHeight / 2);

        return gridSpace;
    }

    private Vector3 gridToWorldSpace(Vector2 gridSpace)
    {
        Vector3 worldSpace;
        worldSpace.x = gridSpace.x - mapWidth / 2;
        worldSpace.y = gridSpace.y - mapHeight / 2;
        worldSpace.z = 0;

        return worldSpace;
    }

    public void SetUnwalkable(int x, int y)
    {
        pathFindMap[x, y].isWalkable = false;
    }

    public bool isWalkable(int x, int y)
    {
        return pathFindMap[x, y].isWalkable;
    }
}
