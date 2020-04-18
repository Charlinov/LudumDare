using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBlackBoard : MonoBehaviour
{
    private float [,] heatMap;

    public int heatMapSize;
    [SerializeField]
    private float heatMapFalloff;

    private int score;
    private int scoreDelta;
    // Start is called before the first frame update
    void Start()
    {
        heatMap = new float[heatMapSize, heatMapSize];
        for (int i = 0; i < heatMapSize; i++)
        {
            for (int j = 0; j < heatMapSize; j++)
            {
                heatMap[i,j] = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreDelta = 0;

        for (int i = 0; i < heatMapSize; i++)
        {
            for (int j = 0; j < heatMapSize; j++)
            {
                if (heatMap[i, j] == 1)
                    scoreDelta++;

                heatMap[i, j] = Mathf.Max(0.0f, heatMap[i,j] - (Time.deltaTime * heatMapFalloff));
            }
        }
        score += scoreDelta;
    }

    public void AddHeat(Vector3 position)
    {
        Vector3 gridSpace = worldToGridSpace(position);
        heatMap[(int)gridSpace.x,(int)gridSpace.y] = 1; // middle

        heatMap[(int)gridSpace.x - 1, (int)gridSpace.y + 1] = 1;    // top left
        heatMap[(int)gridSpace.x, (int)gridSpace.y + 1] = 1;        // top middle
        heatMap[(int)gridSpace.x + 1, (int)gridSpace.y + 1] = 1;    // top right

        heatMap[(int)gridSpace.x - 1, (int)gridSpace.y - 1] = 1;    // bottom left
        heatMap[(int)gridSpace.x, (int)gridSpace.y - 1] = 1;        // bottom middle
        heatMap[(int)gridSpace.x + 1, (int)gridSpace.y - 1] = 1;    // bottom right

        heatMap[(int)gridSpace.x + 1, (int)gridSpace.y] = 1;        // middle right
        heatMap[(int)gridSpace.x - 1, (int)gridSpace.y] = 1;        // middle left
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < heatMapSize; i++)
        {
            for (int j = 0; j < heatMapSize; j++)
            {
                

                if (heatMap != null)
                {
                    Color color = new Color();
                    color.r = heatMap[i, j];
                    color.g = Map(heatMap[i,j], 0, 1, 1, 0);
                    color.b = 0;
                    color.a = 1;

                    Gizmos.color = color;
                }
                else
                    Gizmos.color = Color.grey;

                Gizmos.DrawWireCube(gridToWorldSpace(new Vector3(i, j)), new Vector3(1, 1));
            }
        }
    }

    private Vector3 worldToGridSpace(Vector3 worldSpace)
    {
        Vector3 gridSpace;
        gridSpace.x = (int)(worldSpace.x + heatMapSize / 2);
        gridSpace.y = (int)(worldSpace.y + heatMapSize / 2);
        gridSpace.z = 0;

        return gridSpace;
    }

    private Vector3 gridToWorldSpace(Vector3 gridSpace)
    {
        Vector3 worldSpace;
        worldSpace.x = gridSpace.x - heatMapSize / 2;
        worldSpace.y = gridSpace.y - heatMapSize / 2;
        worldSpace.z = 0;

        return worldSpace;
    }


    private float Map(float s, float a1, float a2, float b1, float b2)
    {
        float t;
        t = b1 + (((s - a1) * (b2 - b1)) / (a2 - a1));
        return t;
    }
}