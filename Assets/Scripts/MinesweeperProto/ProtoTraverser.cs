using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class ProtoTraverser : MonoBehaviour
{
    //One stepsize is fine if cell width and height are the same, but can change
    private float mstepSize;
    private Coroutine movementCoroutine;
    private Vector2 lastDir = Vector2.zero;

    [SerializeField]private Transform body;
    private ProtoMSCell currentCell;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (movementCoroutine == null)
        {
            Vector2 dir = context.ReadValue<Vector2>();
            if (dir != Vector2.zero) {
                Vector2 currentPos = transform.position;

                movementCoroutine = StartCoroutine(Movement(dir));

            }
            
        }
    }

    private IEnumerator Movement(Vector2 dir)
    {
        //Prevent diagonal movement
        dir.Normalize();
        float newX = (dir.y != 0) ? 0 : dir.x;
        float newY = (dir.x != 0) ? 0 : dir.y;
        dir.x = newX;
        dir.y = newY;
        //In actual game if applicable will need a conversion from Grid coords to Transform coords in GridManager, this is easy its just also easy to not do it because step size is really close to 1
        if (SetCurrentCell(currentCell.x + (int)dir.x, currentCell.y + (int)dir.y))
        {
            //Debug.Log("Applying this vector: " + dir);
            Vector3 moveDir = new Vector3(dir.x, dir.y, 0) * mstepSize;
            transform.position += moveDir;
            
        }
        //Rotation
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        body.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        lastDir = dir;

        yield return new WaitForEndOfFrame();
        movementCoroutine = null;
    }

    public void SetStepSize(float stepSize)
    {
        mstepSize = stepSize;
    }

    public void SetCurrentCell(ProtoMSCell cell)
    {
        currentCell = cell;
    }

    public bool SetCurrentCell(int x, int y) // Handles collisions and not going out of bounds
    {

        ProtoMSGrid g = RoundHandler.instance.GetGrid();
        if (currentCell != null)
        {
            if (g.IsValidCoord(x, y)) // am I going out of bounds? (Outisde of the grid?)
            {
                if (g.GetCell(x, y).tileData.GetRevealed())// Is the cell in front of me covered?
                {
                    currentCell = g.GetCell(x, y); // The cell I want to travel to is within bounds and is revealed
                    return true;
                }
                
            }
           
        }
        
        return false;

    }

    public void OnTryRevealTile(InputAction.CallbackContext context)
    {
        Debug.Log("E pressed");
        //In actual game if applicable will need a conversion from Grid coords to Transform coords in GridManager, this is easy its just also easy to not do it because step size is really close to 1
        int x = currentCell.x + (int)lastDir.x;
        int y = currentCell.y + (int)lastDir.y;
        if (CanInteract(x,y))
        {
            ProtoMSCell cell = RoundHandler.instance.GetGrid().GetCell(x, y);
            cell.tileData.TryReveal();
        }
       
        
    }

    public bool CanInteract(int x, int y)
    {
        ProtoMSGrid g = RoundHandler.instance.GetGrid();
        if (g.IsValidCoord(x, y)) //Not interacting with the void
        {
            if (!g.GetCell(x, y).tileData.GetRevealed()) //Can't interact with revealed tiles
            {
                return true;
            }
        }
        return false;
    }


}
