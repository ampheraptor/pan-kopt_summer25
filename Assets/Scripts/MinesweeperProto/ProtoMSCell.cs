using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProtoMSCell : MonoBehaviour, IPointerClickHandler
{
    private bool mine = false;
    private bool revealed = false;
    private bool flagged = false;
  
    private int x, y;

    private int neighborMineCount = 0;

    [SerializeField] private TextMeshPro mText;
    [SerializeField] private SpriteRenderer mCover;
    [SerializeField] private GameObject mFlag;

    void Awake()
    {
        
        mCover.enabled = true;
       

        //mText.enabled = false;
    }

    

    public void SetXY(int x, int y)
    {
        this.x = x; this.y = y;
    }

    public int GetNeighborMineCount()
    {
        return neighborMineCount;
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

    public void CountNeighborMines()
    {
        if (!mine)
        {
            int total = 0;
            //Check every neighbor
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    //Check if valid, this line should make sure we don't check outside of the 2d grid's index
                    if (IsValidCoord(this.x + x, this.y + y))
                    {
                        if (ProtoMSGrid.instance.GetCell(x + this.x, y + this.y).GetMine())
                        {
                            //We don't need to skip ourselves because we are not a mine
                            total++;
                        }
                        
                    }
                }
            }

            neighborMineCount = total; //It feels better to not update the referencable variable during a loop. IDK why
            if (neighborMineCount > 0)
            {
                mText.text = neighborMineCount.ToString();
            }


        }
        else
        {
            neighborMineCount = -1;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!flagged) //flagged mines are not allowed to be left-clicked - nothing will happen.
            {
                if (mine)
                {
                    RevealSingle();
                }
                else
                {
                    RevealRecursive();
                }
            }
        } else if(eventData.button == PointerEventData.InputButton.Right) 
        {
            ToggleFlag();
        }
    } 

    void ToggleFlag()
    {
        if (!revealed) //revealed tiles cannot be flagged manually
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
    }

    void FlagPlace()
    {

        flagged = true;
        mFlag.SetActive(true);
    }

    void FlagRemove()
    {
        flagged = false;
        mFlag.SetActive(false);
    }

    public void RevealRecursive() //this is "Chording"
    {
        // base case
        if (!mine && !revealed)
        {
            RevealSingle();
            if (neighborMineCount == 0)
            {
                for (int y = -1; y < 2; y++)
                {
                    for (int x = -1; x < 2; x++)
                    {
                        //Check if valid, this line should make sure we don't check outside of the 2d grid's index
                        if (IsValidCoord(this.x + x, this.y + y))
                        {
                            //Don't need to check if it's not me because I am already revealed
                            ProtoMSGrid.instance.GetCell(this.x + x, this.y + y).RevealRecursive();

                        }
                    }
                }
            }
        }

        
    }

    public void RevealSingle() // For public use, reveals self
    {
        if (!revealed) //If not already revealed, might be redudnant check but just in case
        {
            //Reveal
            revealed = true;
            mCover.enabled = false;
            if (flagged)
            {
                FlagRemove();
            }
        }
    }

    public bool IsValidCoord(int x, int y)
    {
        if (x >= 0 && x < ProtoMSGrid.ROWS && y >= 0 && y < ProtoMSGrid.COLS)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
}
