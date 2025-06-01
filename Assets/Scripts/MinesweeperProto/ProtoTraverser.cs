using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProtoTraverser : MonoBehaviour
{
    //One stepsize is fine if cell width and height are the same, but can change
    private float mstepSize;

   
    public void OnMove(InputAction.CallbackContext context)
    {
        
        Vector2 dir = context.ReadValue<Vector2>();
        //Prevent diagonal movement
        dir.Normalize();
        dir.x = (dir.y != 0) ? 0 : dir.x;
        dir.y = (dir.x != 0) ? 0 : dir.y;
        
        Debug.Log("Applying this vector: " + dir);
        Vector3 moveDir = new Vector3(dir.x, dir.y, 0) * mstepSize;
        transform.position += moveDir;

    }


    
    public void SetStepSize(float stepSize)
    {
        mstepSize = stepSize / 2;
    }
}
