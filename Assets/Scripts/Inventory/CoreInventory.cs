using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreInventory : Singleton<CoreInventory>
{
    public Inventory myBag;

    [Space(10)]
    public GameObject bag;
    public Button[] coreButton;
    public bool isOpenBag;
    public Button currentButton;
    public int currentInt;
    public bool[] coreBool; 

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Tab))
    //    {
    //        ToggleInventory();
    //    }
    //}

    public void ToggleInventory()
    {
        //isOpenBag = !isOpenBag;
        //bag.SetActive(isOpenBag);

        for (int i = 0; i < coreBool.Length; i++)
        {
            coreButton[i].gameObject.SetActive(coreBool[i]);
        }

        //if (CoreManager.instance.isCoreAbsorbed)
        //{
        //    //TODO 如果已經吸收了核心，顯示當前吸收的核心按鈕，以吸收至背包的核心就不會消失
        //    for (int i = 0; i < instance.coreButton.Length; i++)
        //    {
        //        coreButton[CoreManager.instance.currentAbsorbCore].gameObject.SetActive(true);
        //    }
        //}
        //else if (!CoreManager.instance.isBeingAbsorbed && CoreManager.instance.absorptionTimer <= CoreManager.instance.absorptionTime)
        //{
        //    // 如果正在吸收，不顯示背包按鈕
        //    for (int i = 0; i < instance.coreButton.Length; i++)
        //    {
        //        instance.coreButton[CoreManager.instance.currentAbsorbCore].gameObject.SetActive(false);
        //    }
        //}
        //else
        //{
        //    // 如果沒有在吸收，也沒有吸收完，不顯示背包按鈕
        //    for (int i = 0; i < instance.coreButton.Length; i++)
        //    {
        //        instance.coreButton[CoreManager.instance.currentAbsorbCore].gameObject.SetActive(false);
        //    }
        //}

        // 切換背包時，如果有選中的核心按鈕，取消選中
        if (currentButton != null)
        {
            currentButton.GetComponent<Image>().color = Color.white;
            currentButton = null;
        }
    }

    public void OnSelectButton(Button button)
    {
        // 切換選中狀態
        if (button == currentButton)
        {
            button.GetComponent<Image>().color = Color.white;
            currentButton = null;
        }
        else
        {
            // 取消原本選中的按鈕
            if (currentButton != null)
            {
                currentButton.GetComponent<Image>().color = Color.white;
            }

            // 更新選中按鈕為當前按鈕
            button.GetComponent<Image>().color = Color.green;
            currentButton = button;
        }
    }

    public void OnSelectCurrentCore(int Int)
    {
        if (Int == 1)
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
