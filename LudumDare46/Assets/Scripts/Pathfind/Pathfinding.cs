using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private RG_Node[,] pathFindMap;
    public int mapWidth;
    public int mapHeight;


    public Pathfinding(int width, int height)
    {
        pathFindMap = new RG_Node[mapWidth, mapHeight];
    }

    private List<RG_Node> FindPath(int xStart, int yStart, int xEnd, int yEnd)
    {
        RG_Node startNode = pathFindMap[xStart, yStart];

        return null;
    }
}
