using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum CellType { Default, Mine }
    public enum Action { Reveal, Flag }

    [Header("Cell Range")]
    public int rangeFromClick = 0;
    public List<ProtoMSCell> cellsInRange;
    [SerializeField] ProtoMSGrid grid;

    [Header("Cell Type")]
    public CellType[] affectedTypes;
    public List<ProtoMSCell> affectedCells;

    [Header("Action")]
    public Action action;
    public bool overrideFailure = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            UseItem(5, 5);
        }
    }


    public void SelectThisItem()
    {
        ItemManager.instance.SelectItem(this);
    }

    public void UseItem(int x, int y)
    {
        GenerateRangeList(x,y);
        GenerateAffectedList(cellsInRange);
        foreach(ProtoMSCell cell in affectedCells)
        {
            DoActionOnCell(cell);
        }  
    }


    void GenerateRangeList(int x, int y) //get a list of cells within range of click position
    {
        cellsInRange = new List<ProtoMSCell>();
        int leftX = Mathf.Max(x - rangeFromClick, 0);
        int rightX = Mathf.Min(x + rangeFromClick, grid.GetGridCols() - 1);
        int bottomY = Mathf.Max(y - rangeFromClick, 0);
        int topY= Mathf.Min(y + rangeFromClick, grid.GetGridRows() - 1);
        for(int yIndex = bottomY; yIndex <= topY; yIndex++)
        {
            for (int xIndex = leftX; xIndex <= rightX; xIndex++)
            {
                cellsInRange.Add(grid.GetCell(xIndex,yIndex));
            }
        }
    }
    
    void GenerateAffectedList(List<ProtoMSCell> inputList) //from the cells within range, make a new list of the ones that match the type that this item affects
    {
        affectedCells = new List<ProtoMSCell>();
        for(int i = 0; i < inputList.Count; i++)
        {
            if(affectsAllCellTypes() || isCellAffected(inputList[i]))
            {
                affectedCells.Add(inputList[i]);
            }
        }
    }

    void DoActionOnCell(ProtoMSCell cell) //do the actual item action on the cell
    {
        switch (action)
        {
            case Action.Flag:
                cell.tileData.FlagPlace();
                break;
            case Action.Reveal:
                cell.tileData.RevealSingle(this);
                break;
        }
    }

    bool isCellAffected(ProtoMSCell cell)
    {
        for (int j = 0; j < affectedTypes.Length; j++)
        {
            switch (affectedTypes[j])
            {
                case CellType.Mine:
                    if (cell.GetMine())
                    {
                        return true;
                    }
                    break;
                case CellType.Default:
                    if (!cell.GetMine())
                    {
                        return true;
                    }
                    break;
            }
        }
        return false;
    }

    bool affectsAllCellTypes()
    {
        return affectedTypes.Length == 0;
    }
}
