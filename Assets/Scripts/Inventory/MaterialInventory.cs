using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialInventory : Singleton<MaterialInventory>
{
    public Inventory myBag;
    public GameObject slotGrid;
    public Slot slotPrefab;
    public Text itemInformation;
    public GameObject bag;
    public bool isOpenBag;

    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        RefreshItem();
        Instance.itemInformation.text = "";
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Tab))
    //    {
    //        ToggleInventory();
    //    }
    //}

    public static void UpdateItemInfo(string intemDescription)
    {
        Instance.itemInformation.text = intemDescription;
    }

    public static void CreatNewItem(Item item)
    {
        Slot newItem = Instantiate(Instance.slotPrefab, Instance.slotGrid.transform.position, Quaternion.identity);
        newItem.gameObject.transform.SetParent(Instance.slotGrid.transform);
        newItem.slotItem = item;
        newItem.itemImage.sprite = item.itemImage;
        newItem.slotNum.text = item.itemHeld.ToString();
    }

    public static void RefreshItem()
    {
        for (int i = 0; i < Instance.slotGrid.transform.childCount; i++)
        {
            if (Instance.slotGrid.transform.childCount == 0)
                break;
            Destroy(Instance.slotGrid.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < Instance.myBag.itemList.Count; i++)
        {
            CreatNewItem(Instance.myBag.itemList[i]);
        }
    }

    private void ToggleInventory()
    {
        isOpenBag = !isOpenBag;
        bag.SetActive(isOpenBag);
    }
}
