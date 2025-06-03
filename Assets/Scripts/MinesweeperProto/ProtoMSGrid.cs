using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProtoMSGrid : MonoBehaviour
{
    //These are constants but need to just be private so they can be exposed in the editor
    [SerializeField] protected int ROWS = 10;
    [SerializeField] protected int COLS = 10;

    protected float cellWidth;
    protected float cellHeight;

    protected ProtoMSCell[,] grid;
    [SerializeField] private GameObject cellPrefab;

    [SerializeField] private bool clickable;


    public int minesRandomMin;
    public int minesRandomMax;


    private int totalMines;
    private int totalSafeCells = 0;
    private int cellsRevealed;

    public virtual void StartGrid() //Virtual is so child Traverser grid can override
    {
        grid = new ProtoMSCell[ROWS, COLS];
        cellWidth = cellPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        cellHeight = cellPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        CreateGrid();
        CenterMyself();
    }

    public void RestartGrid()
    {
        EraseGrid();
        CreateGrid();
    }
    private void Start()
    {
       
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
        }
        else
        {
            Debug.Log("ERROR in ProtoMSGrid.cs: mine count exceeds cell count");
        }

        //assign tiles. it's weird that we're using data stored in PMSCell to determine what data-storing class it should have but this is temporary rest assured
        for (int y = 0; y < COLS; y++)
        {
            for (int x = 0; x < ROWS; x++)
            {
                ProtoMSCell cell = grid[x, y];
                Tile td;
                if (cell.GetMine())
                {
                    td = new MineTile();
                }
                else
                {
                    td = new NumberTile();
                }
                cell.SetTileData(td);
                td.StartTileData(cell, this, x, y); //important - must "StartTileData" when you create a new Tile

            }
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

    protected ProtoMSCell RandomEmptyCell()
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

    public int GetGridRows()
    {
        return ROWS;
    }

    public int GetGridCols()
    {
        return COLS;
    }



    //Called from Input system
    public void ToggleDebugNumbers(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShowNumbers();
        }
        else if (context.canceled)
        {
            HideNumbers();
        }
    }

    private void ShowNumbers()
    {
        foreach (ProtoMSCell c in grid)
        {
            c.ShowNumber(true);
        }
    }

    private void HideNumbers()
    {
        foreach (ProtoMSCell c in grid)
        {
            c.ShowNumber(false);
        }
    }

    public void ReportMineTriggered() //for now should probably be simple, but later we may make this more complicated - e.g. if you have extra lives or a shield or w/e
    {
        FindFirstObjectByType<RoundHandler>().ReportMineTriggered();
    }
    public void ReportRevealed()
    {
        cellsRevealed++;
        if (cellsRevealed == totalSafeCells)
        {
            FindFirstObjectByType<RoundHandler>().ReportVictory();
        }
    }

    public bool IsValidCoord(int x, int y)
    {
        if (x >= 0 && x < GetGridRows() && y >= 0 && y < GetGridCols())
        {
            return true;
        }
        else
        {
            return false;
        }
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

    public bool GetClickable()
    {
        return clickable;
    }

}
