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
    private ProtoMSCell currentCell;

    [SerializeField]private Transform body;

   
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
        if (SetCurrentCell(currentCell.x + (int)dir.x, currentCell.y + (int)dir.y))
        {
            //Prevent diagonal movement
            dir.Normalize();
            float newX = (dir.y != 0) ? 0 : dir.x;
            float newY = (dir.x != 0) ? 0 : dir.y;
            dir.x = newX;
            dir.y = newY;

            Debug.Log("Applying this vector: " + dir);
            Vector3 moveDir = new Vector3(dir.x, dir.y, 0) * mstepSize;
            transform.position += moveDir;
            //Rotation
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            body.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
        
        
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

    public bool SetCurrentCell(int x, int y)
    {
        if (currentCell != null)
        {
            if (RoundHandler.instance.GetGrid().IsValidCoord(x, y))
            {
                currentCell = RoundHandler.instance.GetGrid().GetCell(x, y);
                return true;
            }
            else
            {
                Debug.Log("That cell does not exist");
                return false;
            }
        }
        else
        {
           
            return false;
        }

    }
}
