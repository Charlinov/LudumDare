using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RG_Node
{

    private int x;
    private int y;

    public int gCost, hCost, fCost;

    public RG_Node prevNode;
    
    public RG_Node(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

}
