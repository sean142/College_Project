using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InvertoryManager :Singleton<InvertoryManager>
{
    public static event Action<bool> OnInventoryChanged;

    public Inventory myBag;
    public GameObject slotGrid;
    public Slot slotPrefab;
    public Text itemInformation;
    [Space(10)]
    public GameObject bag;
    public Button[] coreButton;
    public bool isOpenBag;
    public Button currentButton;
    public int currentInt;       
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    //void Start()
    //{
    //    // ��l�ɤ���ܭI�]���s
    //    for (int i = 0; i < instance.bagButton.Length; i++)
    //    {
    //        instance.bagButton[i].gameObject.SetActive(false);
    //    }
    //}

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
        isOpenBag = !isOpenBag;
        OnInventoryChanged?.Invoke(isOpenBag);
        bag.SetActive(isOpenBag);

        if (CoreManager.instance.isCoreAbsorbed)
        {            
            // �p�G�w�g�l���F�֤ߡA��ܭI�]���s
            for (int i = 0; i < instance.coreButton.Length; i++)
            {
                instance.coreButton[i].gameObject.SetActive(isOpenBag);
            }
        }
        else if (!CoreManager.instance.isBeingAbsorbed && CoreManager.instance.absorptionTimer <= CoreManager.instance.absorptionTime)
        {
            // �p�G���b�l���A����ܭI�]���s
            for (int i = 0; i < instance.coreButton.Length; i++)
            {
                instance.coreButton[i].gameObject.SetActive(false);
            }
        }
        else
        {
            // �p�G�S���b�l���A�]�S���l�����A����ܭI�]���s
            for (int i = 0; i < instance.coreButton.Length; i++)
            {
                instance.coreButton[i].gameObject.SetActive(false);
            }
        }


        // �����I�]�ɡA�p�G���襤���֤߫��s�A�����襤
        if (currentButton != null)
        {
            currentButton.GetComponent<Image>().color = Color.white;
            currentButton = null;
        }
    }
    
    public void OnSelectButton(Button button)
    {
        // �����襤���A
        if (button == currentButton)
        {
            button.GetComponent<Image>().color = Color.white;
            currentButton = null;
        }
        else
        {
            // �����쥻�襤�����s
            if (currentButton != null)
            {
                currentButton.GetComponent<Image>().color = Color.white;
            }

            // ��s�襤���s����e���s
            button.GetComponent<Image>().color = Color.green;
            currentButton = button;
        }
    }

    public void OnSelectCurrentCore(int Int)
    {
        if (Int==1)
        {
            currentInt = 1;
        }
        else if (Int == 2)
        {
            currentInt = 2;
        }
        else if (Int == 3)
        {
            currentInt = 3;
        }
    }  
}
