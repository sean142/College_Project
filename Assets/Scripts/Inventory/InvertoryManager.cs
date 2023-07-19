using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InvertoryManager :Singleton<InvertoryManager>
{
    [Header("Core")]
    public GameObject coreUI;     // 核心背包UI
    public GameObject coreButton;
    public GameObject coreBag;

    [Header("Material")]
    public GameObject materialUI; // 素材背包UI
    public GameObject materialButton;
    public GameObject materialBag;

    public bool isBagOpen;
    public int openBagIndex = -1;     //紀錄當前打開背包

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    
    public void OnControlBagButtonClick()
    {
        isBagOpen = !isBagOpen;
        materialButton.SetActive(isBagOpen);
        coreButton.SetActive(isBagOpen);

        if (openBagIndex != -1)
        {
            CloseBag(openBagIndex);
            openBagIndex = -1;
        }
        else
        {
            openBagIndex = 0; // 打開第一個背包(核心背包)
            OpenBag(openBagIndex);
        }

        CoreInventory.instance.ToggleInventory();
    }

    // 管理背包UI
    public void OnBagUIClick(int index)
    {
        // 關閉當前打開的背包(如果有)，然後打開新背包
        if (openBagIndex != -1)
        {
            CloseBag(openBagIndex);
        }

        openBagIndex = index;
        OpenBag(openBagIndex);
    }

    private void OpenBag(int index)
    {
        if (index == 0)
        {
            coreUI.SetActive(true);
            coreBag.SetActive(true);
        }
        else if (index == 1)
        {
            materialUI.SetActive(true);
            materialBag.SetActive(true);
        }
    }

    private void CloseBag(int index)
    {
        if (index == 0)
        {
            coreUI.SetActive(false);
            coreBag.SetActive(false);
        }
        else if (index == 1)
        {
            materialUI.SetActive(false);
            materialBag.SetActive(false);
        }
    }


    /*
    public void OnControlBagButtonClick()
    {
        isBagOpen = !isBagOpen;
        materialButton.SetActive(isBagOpen);
        coreButton.SetActive(isBagOpen);
        coreBag.SetActive(isBagOpen);
        materialBag.SetActive(isBagOpen);
    }

    //管理背包UI
    public void OnBagUIClick(int index)
    {
        if (index == 0)
        {
            coreUI.SetActive(true);
            coreBag.SetActive(true);
            materialUI.SetActive(false);
            materialBag.SetActive(false);
        }

        if (index == 1)
        {
            materialUI.SetActive(true);
            materialBag.SetActive(true);

            coreUI.SetActive(false);
            coreBag.SetActive(false); 
        }
    }*/
}
