using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemManager : Singleton<ItemManager>
{
    public Item selectedItem;
    public Item purchasedItem;
    [SerializeField] TextMeshProUGUI currentItemText;
    // Start is called before the first frame update
    void Start()
    {
        // UpdateCurrentItemText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuyItem(Item i) // purchase babyyyy
    {
        purchasedItem = i;
        string itemName = "None";
        if (purchasedItem)
        {
            itemName = purchasedItem.gameObject.name;
        }
        FindFirstObjectByType<InventoryManager>().currentMoney -= i.itemCost;
        FindFirstObjectByType<InventoryManager>().UpdateMoneyTxt();
        UpdateCurrentItemText();

        PutItemInInventory(itemName, purchasedItem); // this is a bit janky but it puts the item in the inventory

    }

    public void SelectItem(Item i)
    {
        selectedItem = i;
        string itemName = selectedItem.gameObject.name;
        currentItemText.text = "Selected Item:\n" + itemName;

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

    public void PutItemInInventory(string thisItemName, Item thisSelectedItem) // calls inventoryManager to populate the item in the inventory
    {
        FindFirstObjectByType<InventoryManager>().UpdateInventory(thisItemName, thisSelectedItem);
    }
}
