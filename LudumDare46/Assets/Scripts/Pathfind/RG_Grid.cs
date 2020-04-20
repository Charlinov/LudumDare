using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RG_Grid
{
    private int width;
    private int height;
    private int cellSize;
    private int[,] gridArray;

    public RG_Grid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;

        gridArray = new int[width, height];
    }
}
