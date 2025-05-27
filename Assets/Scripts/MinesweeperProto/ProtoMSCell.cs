using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProtoMSCell : MonoBehaviour, IPointerClickHandler
{
    private bool mine = false;
    private bool revealed = false;

    private float width, height;
    private int x, y;

    private int neighborMineCount = 0;

    [SerializeField] private TextMeshPro mText;

    void Awake()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
        mText = GetComponentInChildren<TextMeshPro>(); // Only one so OK

        //mText.enabled = false;
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

    public int getNeighborMineCount()
    {
        return neighborMineCount;
    }

    //Should only really be used for setting mines to true
    public void setMine(bool mine)
    {
        this.mine = mine;
        mText.text = (mine == true) ? "M" : "";
    }

    public void GetNeighborMineCount()
    {
        if (!mine)
        {
            //for (int y = 0; y < )
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
            Debug.Log("Clicked " + name);
        }
    }
    
}
