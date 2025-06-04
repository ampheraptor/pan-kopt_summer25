using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tile;
using UnityEngine.EventSystems;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class Tile
{
    RoundHandler rh;
    protected ProtoMSCell clickableObject; //the clickableObject basically handles all the unity stuff and like, gameobjects and sprites and all of that. tiles are all about pure math and data.
    protected ProtoMSGrid parentGrid;
    protected bool flagged = false;
    protected int mine = 0; //mine is now a number rather than a bool
    protected bool revealed = false; 
    protected int x, y;


    public virtual void StartTileData(ProtoMSCell clickableObject, ProtoMSGrid grid, int x, int y)
    {
        parentGrid = grid;
        this.clickableObject = clickableObject;
        this.x = x;
        this.y = y;
    }

    public virtual void LateStartTileData() //basically, more startup functions but "later", e.g. for number tiles to count their neighbors once the mines have already been placed.
    {
    }
     
    /*public Tile(ProtoMSCell clickableObject, ProtoMSGrid grid)
    {
        parentGrid = grid;
        this.clickableObject = clickableObject;
    } */

    public virtual void TryReveal(bool forceRevealFlags=false)
    {
        RevealSingle();
    }

    public virtual void LeftClick()
    {
        if (!flagged) //flagged mines are not allowed to be left-clicked - nothing will happen.
        {
            TryReveal();
        }
    }

    public virtual void RightClick()
    {
        if (!revealed)
        {
            ToggleFlag();
        }
    }

    public virtual void RevealSingle() // For public use, reveals self
    {
        if (!revealed) //If not already revealed, might be redudnant check but just in case
        {
            //Reveal
            revealed = true;
            clickableObject.SetCellRevealed(false);
            if (flagged)
            {
                flagged = false;
                clickableObject.SetFlagVisible(false);
            }
            if (mine > 0)
            {
                parentGrid.ReportMineTriggered();
            }
            if (parentGrid != null)
            {
                parentGrid.ReportRevealed(); //Q: what happens if the last unrevealed tile on the map is a mine? so we win and lose simultaneously?
            }
            else
            {
                parentGrid.ReportRevealed();
            }

        }
    }

    public virtual int GetMine()
    { 
        return mine;
    }

    public virtual bool GetFlagged()
    {
        return flagged;
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
        clickableObject.SetFlagVisible(true);
    }

    protected void FlagRemove()
    {
        flagged = false;
        clickableObject.SetFlagVisible(false);
    }

}

public class MineTile : Tile
{
    public override void StartTileData(ProtoMSCell clickableObject, ProtoMSGrid grid, int x, int y)
    {
        this.mine = 1;
        base.StartTileData(clickableObject, grid,x ,y);
    } 
}

public class NumberTile : Tile
{
    public override void TryReveal(bool forceIncludeFlags)
    { 
        RevealRecursive(forceIncludeFlags);
    }

    public override void RightClick()
    {
        base.RightClick();
        if(revealed && GetNeighborFlagCount() == CountNeighborMines())
        {
            RevealAdjacent(false);
        }

    }

    public int CountNeighborMines()
    {
        int total = 0;
        //Check every neighbor
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                //Check if valid, this line should make sure we don't check outside of the 2d grid's index
                if (parentGrid.IsValidCoord(this.x + x, this.y + y))
                {
                    int m = parentGrid.GetCell(x + this.x, y + this.y).tileData.GetMine();
                    if (m > 0)
                    {
                        //We don't need to skip ourselves because we are not a mine
                        //ummm acktually we do, because we're leaving it open to interpretation how tiles work. maybe the number-mine tile will be a thing later who knows.
                        total += m;
                    }

                }
            }
        }
        return total;
    }

    public override void LateStartTileData()
    {
        base.LateStartTileData();
        string s = CountNeighborMines() + "";
        if(s != "0")
        {
            clickableObject.SetTileText(s);
        }
    }

    public void RevealAdjacent(bool includeFlagged = false) //reveals all adjacent tiles regardless of contents. non-recursive.
    { //"includeFlagged" means whether to reveal flagged tiles as well
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                //Check if valid, this line should make sure we don't check outside of the 2d grid's index
                if (parentGrid.IsValidCoord(this.x + x, this.y + y))
                {
                    Tile td = parentGrid.GetCell(this.x + x, this.y + y).tileData;
                    if (includeFlagged || td.GetFlagged() == false)
                    {
                        td.TryReveal(true);
                    }
                }
            }
        }
    }

    public void RevealRecursive(bool forceIncludeFlagged = true, bool ignoreSelf =false) //this is "Chording"... //forceIncludeFlags is relevant with chording bc we basically never want a "zero tile" next to an unrevealed tile, we always want a border.
    { //checkSelf is whether we care if i am already revealed
        // base case 
        if ((!revealed || ignoreSelf) && (forceIncludeFlagged || !flagged))
        {
            RevealSingle();
            if (CountNeighborMines() == 0)
            {
                //Debug.Log("NM count was 0 at " + x + ", " + y);
                for (int y = -1; y < 2; y++)
                {
                    for (int x = -1; x < 2; x++)
                    {
                        //Check if valid, this line should make sure we don't check outside of the 2d grid's index
                        if (parentGrid.IsValidCoord(this.x + x, this.y + y))
                        {
                            //Don't need to check if it's not me because I am already revealed
                            parentGrid.GetCell(this.x + x, this.y + y).tileData.TryReveal();

                        }
                    }
                }
            }
        }
    }

    public int GetNeighborFlagCount()
    {
        int total = 0;
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                //Check if valid, this line should make sure we don't check outside of the 2d grid's index
                if (parentGrid.IsValidCoord(this.x + x, this.y + y))
                {
                    if (parentGrid.GetCell(x + this.x, y + this.y).tileData.GetFlagged())
                    {
                        total++;
                    }
                }
            }
        }
        return total;
    }
}

public class MultiMine : MineTile
{
    public override void StartTileData(ProtoMSCell clickableObject, ProtoMSGrid grid, int x, int y)
    {
        base.StartTileData(clickableObject, grid, x, y);
        this.mine = 2;
    }
}
