using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProtoMSCell : MonoBehaviour, IPointerClickHandler
{
    public Tile tileData;
    protected bool mine = false;
    protected bool revealed = false;
    protected bool flagged = false;
  
    public int x, y;

    protected int neighborMineCount = 0;
    protected int neighborFlagCount => GetNeighborFlagCount();

    [SerializeField] protected TextMeshPro mText;
    [SerializeField] protected SpriteRenderer mCover;
    [SerializeField] protected GameObject mFlag;

    private ProtoMSGrid parentGrid;

    void Awake()
    {
        
        mCover.enabled = true;
       

        //mText.enabled = false;
    }

    public void SetTileData(Tile td)
    {
        tileData = td;

        //debug
        //mFlag.GetComponent<TextMeshPro>().text = td.GetMine() + "";
        //SetFlagVisible(true);
    }

    public void SetCellRevealed(bool visible)
    {
        mCover.enabled = visible;
    }


    public void SetFlagVisible(bool visible)
    {
        mFlag.SetActive(visible);
    }

    public void SetXY(int x, int y)
    {
        this.x = x; this.y = y;
        
    }

    public void SetXY(int x, int y, ProtoMSGrid parentGrid)
    {
        this.x = x; this.y = y;
        this.parentGrid = parentGrid;
    }

    public int GetNeighborMineCount()
    {
        return neighborMineCount;
    }

    public int GetNeighborFlagCount()
    {
        int total = 0;
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                //Check if valid, this line should make sure we don't check outside of the 2d grid's index
                if (IsValidCoord(this.x + x, this.y + y))
                {
                    if (parentGrid.GetCell(x + this.x, y + this.y).flagged)
                    {
                        total++;
                    }
                }
            }
        }
        return total;
    }

    //Should only really be used for setting mines to true
    //if we want to make framework for setting mines to false, will need to refresh the neighbor numbers as well
    public void SetMine(bool mine)
    {
        this.mine = mine;
        mText.text = (mine == true) ? "M" : "";
    }

    public bool GetMine()
    {
        return mine;
    }

    public void SetTileText(string text)
    {
        mText.text = text;
    }


    protected void ToggleFlag()
    { 
        if (!flagged)
        {
            FlagPlace();
        }
        else
        {
            FlagRemove();
        }
}

    protected void FlagPlace()
    {

        flagged = true;
        mFlag.SetActive(true);
    }

    protected void FlagRemove()
    {
        flagged = false;
        mFlag.SetActive(false);
    }

    public bool IsValidCoord(int x, int y)
    {
        if (x >= 0 && x < parentGrid.GetGridRows() && y >= 0 && y < parentGrid.GetGridCols())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ShowNumber(bool reveal)
    {
        mCover.sortingOrder = (reveal) ? 0 : 1;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //If this is a clickable grid
        
        if (parentGrid.GetClickable())
        {
            if (eventData.button == PointerEventData.InputButton.Left) //left click - reveal
            {
                tileData.LeftClick();
                /*if (!flagged) //flagged mines are not allowed to be left-clicked - nothing will happen.
                {
                    if (mine)
                    {
                        RevealSingle();
                    }
                    else
                    {
                        RevealRecursive();
                    }
                }*/
            }
            else if (eventData.button == PointerEventData.InputButton.Right) //right click - for unrevealed, toggle flag. for revealed, if right # of flags, reveal adjacent.
            {
                tileData.RightClick();
                /*if (!revealed) //revealed tiles cannot be flagged manually
                {
                    ToggleFlag();
                }
                else if (neighborFlagCount == neighborMineCount) //right click revealed reveals adjacents
                { //only does this if flag count == mine count; i.e. you (probably) got the flags "correct"
                    RevealAdjacent(false);
                }*/

            }
        }
        else
        {
            Debug.Log("Grid is not set to clickable, you cannot click the tile!!");
        }
    }
        

}
