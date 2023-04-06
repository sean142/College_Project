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
        // TODO: �ھګ��s�������֤߰����O
        Debug.Log("�ϥή֤߯�O: " + button.name);
    }
}
