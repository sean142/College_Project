using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory",menuName = "Inventory/New Inventory")]
public class Inventory : ScriptableObject
{
    public static Inventory instance;

    public List<Item> itemList = new List<Item>();

    private void Awake()
    {
        instance = this;
    }
}
