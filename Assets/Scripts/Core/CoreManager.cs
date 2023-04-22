using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreManager :Singleton<CoreManager> 
{
    public float useTime;
    public float useTimer;
    public float coolTime;
    public float coolTimer;
    public bool isUseTime;
    public bool isCoolTime;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (!isUseTime)
        {
            if (Input.GetKeyDown(KeyCode.F) && InvertoryManager.instance.currentInt != 0)
            {
                UseCoreAbility(InvertoryManager.instance.currentInt);
            }
        }       


        
        if (isUseTime)
        {
            useTimer += Time.deltaTime;
         
            if (useTimer >= useTime) // usage period ends
            {
                isUseTime = false;
                useTimer = 0;
                coolTimer = coolTime;
                isCoolTime = true;

            }
        }
        else if (coolTimer > 0)
        {
            coolTimer -= Time.deltaTime;
            CoreAbuility.InitialState();

            if (coolTimer <= 0) // cooldown ends
            {
                isUseTime = false;
                coolTimer = 0;
                useTimer = useTime;
                isCoolTime = false;

            }
        }
    }

    public void UseCoreAbility(int Int)
    {
        if (isCoolTime) // 在冷卻中，不執行任何動作
        {
            Debug.Log("核心能力冷卻中");
            return;
        }
        if (Int==1)
        {         
            useTimer = 0; // 重置 useTimer 以便启动冷却计时器
            CoreAbuility.SpeedActivateAndNightVision();
            Debug.Log("使用核心能力: " + 1);
            coolTimer = coolTime;
        }
        else if (Int == 2)
        {
            useTimer = 0; // 重置 useTimer 以便启动冷却计时器
            CoreAbuility.StrengthBoost();
            Debug.Log("使用核心能力: "+2);
            coolTimer = coolTime;

        }
        else if (Int == 3)
        {
            useTimer = 0; // 重置 useTimer 以便启动冷却计时器
            CoreAbuility.ImproveDefense();
            Debug.Log("使用核心能力: "+3);
            coolTimer = coolTime;
        }
        isUseTime = true;
        isCoolTime = true;

    }

}
