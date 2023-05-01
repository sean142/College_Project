using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreManager :Singleton<CoreManager> 
{
    [Header("UseTime&CoolTime")]
    public float useTime;
    public float useTimer;
    public float coolTime;
    public float coolTimer;
    public bool isUseTime;
    public bool isCoolTime;

    [Header("Absorp")]
    public float absorptionTime; // 吸收所需時間
    public float absorptionTimer; // 吸收計時器    
    public bool isCoreAbsorbed; // 是否吸收完了
    public bool isBeingAbsorbed; // 是否正在被吸收中
    public bool isTrigger; //是否碰撞到核心

    protected override void Awake()
    {
       // Application.targetFrameRate = 60;
      //  Time.timeScale = 1f;

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

        if (isTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E) && isBeingAbsorbed == false)
            {
                // 開始吸收計時
                isBeingAbsorbed = true;
                isTrigger = false;
                absorptionTimer = 0.0f;
            }
        }
       
        // 檢測是否正在被吸收中
        if (isBeingAbsorbed)
        {
            // 更新吸收進度條
            absorptionTimer += Time.deltaTime;
        }
        // 檢測吸收是否完成
        if (absorptionTimer >= absorptionTime)
        {
            // 吸收完成，核心消失
            absorptionTimer = 0.0f;
            isBeingAbsorbed = false;
            isCoreAbsorbed = true;
            Destroy(CoreItemOnWorld.instance.core1); // 或者設置為非活動狀態等待重生
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
            useTimer = 0; // 重置 useTimer 
            CoreAbuility.SpeedActivateAndNightVision();
            Debug.Log("使用核心能力: " + 1);
            coolTimer = coolTime;
        }
        else if (Int == 2)
        {
            useTimer = 0; 
            CoreAbuility.StrengthBoost();
            Debug.Log("使用核心能力: "+2);
            coolTimer = coolTime;

        }
        else if (Int == 3)
        {
            useTimer = 0; 
            CoreAbuility.ImproveDefense();
            Debug.Log("使用核心能力: "+3);
            coolTimer = coolTime;
        }
        isUseTime = true;
        isCoolTime = true;
    }
}
