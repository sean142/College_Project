using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item slotItem;
    public Image itemImage;
    public Text slotNum;

    public void ItemOnClicked()
    {
        InvertoryManager.UpdateItemInfo(slotItem.itemInfo);
    }
}
