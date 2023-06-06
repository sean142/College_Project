using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InvertoryManager :MonoBehaviour
{
    public GameObject coreUI;     // 核心背包UI
    public GameObject materialUI; // 素材背包UI
   

    //管理背包UI
    public void OnBagUIClick(int index)
    {
        if(index==0)
        {
            coreUI.SetActive(true);
            materialUI.SetActive(false);
            coreUI.transform.SetParent(null);
        }

        if (index == 1)
        {
            coreUI.SetActive(false);
            materialUI.SetActive(true);
            materialUI.transform.SetParent(null);

        }
    }
}
