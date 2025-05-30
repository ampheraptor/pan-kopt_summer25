using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProtoMSGrid : Singleton<ProtoMSGrid>
{
    [SerializeField] public static readonly int ROWS = 10;
    [SerializeField] public static readonly int COLS = 10;

    private float cellWidth;
    private float cellHeight;

    private ProtoMSCell[,] grid = new ProtoMSCell[ROWS, COLS];
    [SerializeField] private GameObject cellPrefab;


    public int minesRandomMin;
    public int minesRandomMax;
    private int totalMines;
    private int totalSafeCells;
    private int cellsRevealed = 0;

    public void StartGrid()
    {
        cellWidth = cellPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        cellHeight = cellPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        CreateGrid();
        CenterMyself();
        Debug.Log("started grid");
    }

    public void RestartGrid()
    {
        EraseGrid();
        CreateGrid();
    }
    private void CreateGrid()
    {
        for (int y = 0; y < COLS; y++)
        {
            for (int x = 0; x < ROWS; x++)
            {
                //Make a new cell 
                ProtoMSCell cell = Instantiate(cellPrefab, transform).GetComponent<ProtoMSCell>();
                //Put it in the 2D Array
                grid[x, y] = cell;
                //Give cell it's 2D coords
                grid[x, y].SetXY(x, y, this);
                //Place it correctly
                float newX = x * cellWidth;
                float newY = y * cellHeight;
                cell.transform.localPosition = new Vector3(newX, newY, 0);
            }
        }

        //place mines
        totalMines = Random.Range(minesRandomMin, minesRandomMax);
        if (totalMines <= ROWS * COLS)
        {
            for (int i = 0; i < totalMines; i++)
            {
                RandomEmptyCell().SetMine(true);
            }
        } else
        {
            Debug.Log("ERROR in ProtoMSGrid.cs: mine count exceeds cell count");
        }
        //set numbers
        for (int y = 0; y < COLS; y++)
        {
            for (int x = 0; x < ROWS; x++)
            {
                grid[x, y].CountNeighborMines(); // needs to be in its own loop because otherwise it will try to count cells that dont exist yet
            }
        }
        totalSafeCells = COLS * ROWS - totalMines;
        cellsRevealed = 0;
    }

    private ProtoMSCell RandomEmptyCell()
    {
        ProtoMSCell pms = grid[Random.Range(0, ROWS), Random.Range(0, COLS)];
        if (pms.GetMine())
        {
            return RandomEmptyCell(); //keeps iterating through random cells until it finds one that isn't a mine
            //for this reason, this function may cause an overflow if the # of mines placed exceeds the number of total cells
            //so run a check first!
        }
        return pms;
    }

    public void ReportRevealed()
    {
        cellsRevealed++;
        if (cellsRevealed == totalSafeCells)
        {
            FindFirstObjectByType<RoundHandler>().ReportVictory();
        }
    }

    private void CenterMyself()
    {
        //Request page help on this, it's not exactly right
        float newX = transform.position.x;
        float newY = transform.position.y;
        float lengthOfGrid = cellWidth * COLS;
        float heightOfGrid = cellHeight * ROWS;
        newX = newX - (lengthOfGrid / 2) + (cellWidth / 2);
        newY = newY - (heightOfGrid / 2) + (cellHeight / 2); // ok literally don't know why this works. whatever
        transform.position = new Vector3(newX, newY, 0);

    }

    public ProtoMSCell GetCell(int x, int y)
    {
        //Debug.Log("I'm getting Cell: " + x + ", " + y + ". ");
        return grid[x, y];
    }

    private void EraseGrid()
    {
        for (int y = 0; y < COLS; y++)
        {
            for (int x = 0; x < ROWS; x++)
            {
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }
    }

}
