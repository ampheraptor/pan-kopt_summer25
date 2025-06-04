using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class ProtoTraverser : MonoBehaviour
{
    //One stepsize is fine if cell width and height are the same, but can change
    private float mstepSize;
    private Coroutine movementCoroutine;

   
    public void OnMove(InputAction.CallbackContext context)
    {
        if (movementCoroutine == null)
        {
            movementCoroutine = StartCoroutine(Movement(context.ReadValue<Vector2>()));
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

        Debug.Log("Applying this vector: " + dir);
        Vector3 moveDir = new Vector3(dir.x, dir.y, 0) * mstepSize;
        transform.position += moveDir;
        yield return new WaitForEndOfFrame();
        movementCoroutine = null;
    }


    
    public void SetStepSize(float stepSize)
    {
        mstepSize = stepSize;// / 2;
    }
}
