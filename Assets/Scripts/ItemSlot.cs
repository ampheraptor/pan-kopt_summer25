using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ItemSlot
{
    public bool isFull; // does this slot have an item in it already?
    public Item thisItem; // the item in this slot

    public Button slotButton; // for clicking behavior
    public TextMeshProUGUI itemText; // the UI text for the item name



}
