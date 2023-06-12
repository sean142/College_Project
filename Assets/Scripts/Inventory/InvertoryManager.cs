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
    }
}
