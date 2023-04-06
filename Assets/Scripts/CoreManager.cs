using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreManager :Singleton<CoreManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {  
        if (Input.GetKeyDown(KeyCode.F) && InvertoryManager.instance.currentButton != null)
        {
            UseCoreAbility(InvertoryManager.instance.currentButton);
        }
    }
   

    public static void UseCoreAbility(Button button)
    {
        // TODO: 根據按鈕對應的核心執行能力
        Debug.Log("使用核心能力: " + button.name);
    }
}
