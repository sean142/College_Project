using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InvertoryManager : MonoBehaviour
{
    public static InvertoryManager instance;
    public static event Action<bool> OnInventoryChanged;

    public Inventory myBag;
    public GameObject slotGrid;
    public Slot slotPrefab;
    public Text itemInformation;
    [Space(10)]
    public GameObject bag;
    public GameObject[] bagButton;
    public bool isOpenBag;
    public Button currentButton;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        instance = this;

        if (bag == null)
            bag = GameObject.Find("BagCanvas/Bag");

      
    }

    private void OnEnable()
    {
        RefreshItem();
        instance.itemInformation.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
     
    }

    public static void UpdateItemInfo(string intemDescription)
    {
        instance.itemInformation.text = intemDescription;
    }

    public static void CreatNewItem(Item item)
    {
        Slot newItem = Instantiate(instance.slotPrefab, instance.slotGrid.transform.position, Quaternion.identity);
        newItem.gameObject.transform.SetParent(instance.slotGrid.transform);
        newItem.slotItem = item;
        newItem.itemImage.sprite = item.itemImage;
        newItem.slotNum.text = item.itemHeld.ToString();
    }

    public static void RefreshItem()
    {
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            if (instance.slotGrid.transform.childCount == 0)
                break;
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < instance.myBag.itemList.Count; i++)
        {
            CreatNewItem(instance.myBag.itemList[i]);           
        }
    }

    private void ToggleInventory()
    {
        instance.isOpenBag = !instance.isOpenBag;
        OnInventoryChanged?.Invoke(instance.isOpenBag);
        instance.bag.SetActive(isOpenBag);

        for (int i = 0; i < instance.bagButton.Length; i++)
        {
            instance.bagButton[i].SetActive(instance.isOpenBag);            
        }

        // �����I�]�ɡA�p�G���襤���֤߫��s�A�����襤
        if (instance.currentButton != null)
        {
            instance.currentButton.GetComponent<Image>().color = Color.white;
            instance.currentButton = null;
        }
    }
    
    public void OnSelectButton(Button button)
    {
        // �����襤���A
        if (button == instance.currentButton)
        {
            button.GetComponent<Image>().color = Color.white;
            instance.currentButton = null;
        }
        else
        {
            // �����쥻�襤�����s
            if (instance.currentButton != null)
            {
                instance.currentButton.GetComponent<Image>().color = Color.white;
            }

            // ��s�襤���s����e���s
            button.GetComponent<Image>().color = Color.green;
            instance.currentButton = button;
        }
    }
  
}
