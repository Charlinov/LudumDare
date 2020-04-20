using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RG_Node
{

    public int x;
    public int y;

    public int gCost, hCost, fCost;

    public bool isWalkable;
    public RG_Node prevNode;
    
    public RG_Node(int x, int y)
    {
        this.x = x;
        this.y = y;
        isWalkable = true;
    }


    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}
