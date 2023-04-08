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
        if (Input.GetKeyDown(KeyCode.F) && InvertoryManager.instance.currentInt != 0)
        {
            UseCoreAbility(InvertoryManager.instance.currentInt);
        }
    }   

    public void UseCoreAbility(int Int)
    {       
        if (Int==1)
        {
            CoreAbuility.SpeedActivateAndNightVision();
            Debug.Log("使用核心能力: "+1);
        }
        else if (Int == 2)
        {
            CoreAbuility.StrengthBoost();
            Debug.Log("使用核心能力: "+2);
        }
        else if (Int == 3)
        {
            CoreAbuility.ImproveDefense();
            Debug.Log("使用核心能力: "+3);
        }
    }
}
