using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProtoMSGrid : Singleton<ProtoMSGrid>
{
    private static readonly int ROWS = 7;
    private static readonly int COLS = 5;

    private ProtoMSCell[,] grid = new ProtoMSCell[ROWS, COLS];
    [SerializeField] private GameObject cellPrefab;


    private void CreateGrid() 
    {
        for (int y = 0; y < COLS; y++)
        {
            for (int x = 0; x < ROWS; x++)
            {
                //Make a new cell
                GameObject cell = Instantiate(cellPrefab, transform);
                //Put it in the 2D Array
                grid[x, y] = cell.GetComponent<ProtoMSCell>();
                //Give cell it's 2D coords
                grid[x,y].setXY(x, y);
                //Place it correctly
                float newX = x * grid[x,y].getWidth();
                float newY = y * grid[x,y].getHeight();
                cell.transform.localPosition = new Vector3(newX,newY,0);

                //Temporary Mine Testing
                if (Random.Range(0,10) > 5)
                {
                    grid[x, y].setMine(true);
                }
            }
        }
    }

    private void Start()
    {
        CreateGrid();
    }
}
