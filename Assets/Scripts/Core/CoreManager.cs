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

    [Header("CoreObjects")]
    public GameObject coreObject; //儲存核心物件
    public int coreQuantity; //物件池中核心物件的總數量是多少
    public CoreItemOnWorld[] corePool; //物件池 
    public int corePoolCount; //當前的核心編號 
    public Transform corePoint; //儲存生成核心物件的位置

    protected override void Awake()
    {
       // Application.targetFrameRate = 60;
      //  Time.timeScale = 1f;

        base.Awake();
        DontDestroyOnLoad(this);
    }
    public void Start()
    {
        // 建立物件池並從中獲取 BasicPoolObject 的 CoreItemOnWorld 腳本
        corePool =  new CoreItemOnWorld[coreQuantity];

        for (int i = 0; i < corePool.Length; i++)
        {
            corePool[i] = Instantiate(coreObject).GetComponent<CoreItemOnWorld>(); 
        }
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
            // 吸收完成，隱藏該核心物件
            absorptionTimer = 0.0f;
            isBeingAbsorbed = false;
            isCoreAbsorbed = true;

            // 如果 corePoolCount 大於等於 corePool 長度，則重新從頭開始計算編號
            //  if (corePoolCount >= corePool.Length)
            //  corePoolCount = 0;
            //執行下面程式

            // 隱藏物件池中的最後一個核心物件 取得最後一個核心物件，要扣1是因為array是從0開始編號
            int i = corePoolCount - 1;
            // 如果索引是負數，表示所有核心都已經使用，因此使用物件池中最後一個核心物件
            if (i == -1)  
            {
                i = corePool.Length - 1; //這裡的 i 就是物件池的最後一個 //要同等於array 所以池子的長度要扣1 因為array是0開始數   
            }
            corePool[i].TurnOff();
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
    }

    //抓取corePoint位置 與當前數量從新計算
    public void TureOnCore()
    {
        corePool[corePoolCount].transform.position = corePoint.position;
        corePool[corePoolCount].transform.rotation = corePoint.rotation;
        corePool[corePoolCount].TurnOn();
        corePoolCount += 1;
        if (corePoolCount >= corePool.Length)
            corePoolCount = 0;      
    }
}
