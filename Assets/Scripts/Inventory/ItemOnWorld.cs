using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;
    public Inventory playerInventory;
    public static ItemOnWorld instance;
    private void Awake()
    {
        instance = this;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //AddNewItem();
            //Destroy(gameObject);
        }
    }

    public void AddNewItem()
    {
        if (!playerInventory.itemList.Contains(thisItem))
        {
            playerInventory.itemList.Add(thisItem);
            //InvertoryManager.CreatNewItem(thisItem);
        }
        else
        {
            thisItem.itemHeld++;
        }

        MaterialInventory.RefreshItem();
    }

    
}
