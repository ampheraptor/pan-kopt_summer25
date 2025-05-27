using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProtoMSCell : MonoBehaviour, IPointerClickHandler
{
    private bool mine = false;
    private bool revealed = false;

    private float width, height;
    private int x, y;

    void Awake()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    public float getWidth()
    {
        return width;
    }
    public float getHeight()
    {
        return height;
    }

    public void setXY(int x, int y)
    {
        this.x = x; this.y = y;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Clicked " + name);
        }
    }
    
}
