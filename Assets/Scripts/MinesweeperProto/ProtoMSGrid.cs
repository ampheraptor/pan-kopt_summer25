using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProtoMSGrid : Singleton<ProtoMSGrid>
{
    public static readonly int ROWS = 7;
    public static readonly int COLS = 5;

    private float cellWidth;
    private float cellHeight;

    private ProtoMSCell[,] grid = new ProtoMSCell[ROWS, COLS];
    [SerializeField] private GameObject cellPrefab;



    private void Start()
    {
        cellWidth = cellPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        cellHeight = cellPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        CreateGrid();
        CenterMyself();
    }
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
                float newX = x * cellWidth;
                float newY = y * cellHeight;
                cell.transform.localPosition = new Vector3(newX,newY,0);

                //Temporary Mine Testing
                if (Random.Range(0,10) > 7)
                {
                    grid[x, y].setMine(true);
                }
            }
        }

        for (int y = 0; y < COLS; y++)
        {
            for (int x = 0; x < ROWS; x++)
            {
                grid[x, y].CountNeighborMines(); // needs to be in its own loop because otherwise it will try to count cells that dont exist yet
            }
        }
    }

    

    private void CenterMyself()
    {
        //Request page help on this, it's not exactly right
        float newX = transform.position.x;
        float newY = transform.position.y;
        float lengthOfGrid = cellWidth * COLS;
        float heightOfGrid = cellHeight * ROWS;
        newX = newX - (lengthOfGrid / 2);
        newY = newY - (heightOfGrid / 2);
        transform.position = new Vector3(newX, newY, 0);

    }

    public ProtoMSCell GetCell(int x, int y)
    {
        //Debug.Log("I'm getting Cell: " + x + ", " + y + ". ");
        return grid[x,y];
    }
}
