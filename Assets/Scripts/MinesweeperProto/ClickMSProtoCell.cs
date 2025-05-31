using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickMSProtoCell : ProtoMSCell, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
       
        if (eventData.button == PointerEventData.InputButton.Left) //left click - reveal
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
        }
        else if (eventData.button == PointerEventData.InputButton.Right) //right click - for unrevealed, toggle flag. for revealed, if right # of flags, reveal adjacent.
        {
            if (!revealed) //revealed tiles cannot be flagged manually
            {
                ToggleFlag();
            }
            else if (neighborFlagCount == neighborMineCount) //right click revealed reveals adjacents
            { //only does this if flag count == mine count; i.e. you (probably) got the flags "correct"
                RevealAdjacent(false);
            }

        }
    }
}
