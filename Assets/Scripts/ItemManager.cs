using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemManager : Singleton<ItemManager> 
{
    public Item selectedItem;
    [SerializeField] TextMeshProUGUI currentItemText;
    // Start is called before the first frame update
    void Start()
    {
        UpdateCurrentItemText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectItem(Item i)
    {
        selectedItem = i;
        UpdateCurrentItemText();

    }

    public void DeselectItem()
    {
        selectedItem = null;
        UpdateCurrentItemText();
    }

    public void UseSelectedItem(int x, int y)
    {
        if (selectedItem)
        {
            selectedItem.UseItem(x, y);
            DeselectItem();
        }
    }

    private void UpdateCurrentItemText()
    {
        string itemName = "None";
        if (selectedItem)
        {
            itemName = selectedItem.gameObject.name;
        }
        currentItemText.text = "Current Item:\n" + itemName;
    }
}
