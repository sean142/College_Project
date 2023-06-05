using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreManager_TEST : Singleton<CoreManager_TEST>
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
    public GameObject[] coreObjects; //不同敌人对应的核心物件
    public int coreQuantity; //每个敌人核心物件的数量
    public CoreItemOnWorld[,] corePools; //核心物件池，使用二维数组根据敌人类型和位置储存核心物件
    public int[,] corePoolCounts; //当前核心物件的编号
    public Transform[,] corePoints; //储存生成核心物件的位置
  
    public void Start()
    {
        // 初始化核心物件池
        corePools = new CoreItemOnWorld[coreObjects.Length, coreQuantity];
        corePoolCounts = new int[coreObjects.Length, corePoints.Length];
        corePoints = new Transform[coreObjects.Length, corePoints.Length];

        for (int i = 0; i < coreObjects.Length; i++)
        {
            for (int j = 0; j < coreQuantity; j++)
            {
                corePools[i, j] = Instantiate(coreObjects[i]).GetComponent<CoreItemOnWorld>();
            }
        }
    }

    private void FixedUpdate()
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
            if (Input.GetKeyDown(KeyCode.E) && !isBeingAbsorbed)
            {
                // 开始吸收计时
                isBeingAbsorbed = true;
                isTrigger = false;
                absorptionTimer = 0.0f;
            }
        }

        // 检测是否正在被吸收中
        if (isBeingAbsorbed)
        {
            // 更新吸收进度条
            absorptionTimer += Time.deltaTime;
        }

        // 检测吸收是否完成
        if (absorptionTimer >= absorptionTime)
        {
            // 吸收完成，隐藏该核心物件
            absorptionTimer = 0.0f;
            isBeingAbsorbed = false;
            isCoreAbsorbed = true;

            int enemyType = GetEnemyType(); // 获取敌人类型
            int corePointCount = GetCorePointCount(enemyType); // 获取生成核心位置的编号

            // 隐藏对应敌人类型和位置的核心物件
            corePools[enemyType, corePoolCounts[enemyType, corePointCount]].TurnOff();

            // 递增核心物件的编号和位置的编号
            corePoolCounts[enemyType, corePointCount]++;
            corePoolCounts[enemyType, corePointCount] %= coreQuantity;
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
        if (isCoolTime) // 在冷却中，不执行任何动作
        {
            Debug.Log("核心能力冷却中");
            return;
        }

        if (Int > 0 && Int <= coreObjects.Length)
        {
            int enemyType = Int - 1; // 敌人类型与核心物件数组索引对应
            int corePointCount = GetCorePointCount(enemyType); // 获取生成核心位置的编号

            useTimer = 0; // 重置 useTimer

            // 根据敌人类型和位置生成核心物件
            corePools[enemyType, corePoolCounts[enemyType, corePointCount]].transform.position = corePoints[enemyType, corePointCount].position;
            corePools[enemyType, corePoolCounts[enemyType, corePointCount]].transform.rotation = corePoints[enemyType, corePointCount].rotation;
            corePools[enemyType, corePoolCounts[enemyType, corePointCount]].TurnOn();

            // 递增核心物件的编号和位置的编号
            corePoolCounts[enemyType, corePointCount]++;
            corePoolCounts[enemyType, corePointCount] %= coreQuantity;

            // 执行核心能力
            switch (Int)
            {
                case 1:
                    CoreAbuility.SpeedActivateAndNightVision();
                    break;
                case 2:
                    CoreAbuility.StrengthBoost();
                    break;
                case 3:
                    CoreAbuility.ImproveDefense();
                    break;
            }

            Debug.Log("使用核心能力: " + Int);
            coolTimer = coolTime;
            isUseTime = true;
        }
    }

    // 获取敌人类型
    private int GetEnemyType()
    {
        // 根据敌人的不同条件来判断敌人类型，返回相应的编号
        return 0;
    }

    // 获取生成核心位置的编号
    private int GetCorePointCount(int enemyType)
    {
        // 根据敌人类型来判断生成核心位置的编号，返回相应的编号
        return 0;
    }
}

