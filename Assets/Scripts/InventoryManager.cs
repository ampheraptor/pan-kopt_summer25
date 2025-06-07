using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InventoryManager : MonoBehaviour
{
    [SerializeField] public int currentMoney; // how much money you have
    [SerializeField] public List<ItemSlot> itemSlots;

    [Header("Text Fields")]
    public TextMeshProUGUI moneyText;
    // Start is called before the first frame update
    void Start()
    {
        ResetInventory();
        UpdateMoneyTxt();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetInventory() // empties all items, sets button behavior. 
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].thisItem = null; // empties item
            itemSlots[i].isFull = false; // no full anymore!!
        }


        // this is a bit janky because onClick behavior is, if we can transfer it to another function feel free
        foreach (ItemSlot thisSlot in itemSlots)
        {
            thisSlot.slotButton.onClick.AddListener(() =>
            {
                if (thisSlot.isFull)
                {
                    FindFirstObjectByType<ItemManager>().SelectItem(thisSlot.thisItem);
                    thisSlot.isFull = false;
                    thisSlot.itemText.text = "Empty";
                    Debug.Log("Used item!");
                }
                else
                {
                    Debug.Log("Slot is empty! No item to use!");
                }
            }

            ); // why is the syntax for addlistener like this. 
        }

    }




    public void UpdateMoneyTxt() // honestly this is only its own function so it can be called from other scripts 
    {
        moneyText.text = currentMoney.ToString();
    }



    public void UpdateInventory(string thisItemName, Item thisSelectedItem)
    {
        int indexToFill = -1;

        // Find the first empty slot
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (!itemSlots[i].isFull)
            {
                indexToFill = i;
                break; // stop at the first available slot
            }
        }

        if (indexToFill != -1)
        {
            Debug.Log("Wahoo! Space for a new item!");

            ItemSlot slotToFill = itemSlots[indexToFill];  //

            slotToFill.thisItem = thisSelectedItem;
            slotToFill.isFull = true;

            if (slotToFill.itemText != null)
                slotToFill.itemText.text = thisItemName;

            else
                Debug.LogWarning("itemText is null on this slot!");



            // update the list to have the item
            itemSlots[indexToFill] = slotToFill;
        }
        else
        {
            Debug.Log("Whoops! All inventory slots used!");
        }
    }
}


