using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CoreManager : Singleton<CoreManager>
{
    public bool isCoreTurnOn;
    public bool isBeingAbsorbed; // 是否正在被吸收中
    public bool isCoreGenerating; //判斷核心是否正在生成

    [Header("UseTime&CoolTime")]
    public float useTime;
    public float useTimer;
    public float coolTime;
    public float coolTimer;
    public bool isUseTime;
    public bool isCoolTime;

    [Header("Absorp")]
    //public float absorptionTime; // 吸收所需時間
    //public float absorptionTimer; // 吸收計時器    
    //public bool isCoreAbsorbed; // 是否吸收完了
    //public bool isTrigger; //是否碰撞到核心

    [Header("CoreObjects")]
    public GameObject[] coreObject; //儲存核心物件
    public CoreItemOnWorld[] corePool; //物件池 
    public int[] currentAbsorbCore;   // 當前核心編號  

    [Header("TrailPool")]
    public GameObject[] trailsObject;
    public TrailsItemOnWorld[] trailsPool;
    public Transform trailTarget;

    [Header("PoolParent")]
    public GameObject poolParent;

    [Header("BezierCalculator")]
    public int Duration = 5;
    public Transform[] bezierHandle;
    public GameObject bezierHandleParent;
    private float _duration;  

    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(this);
    }

    public void Start()
    {
        // 建立物件池並從中獲取 BasicPoolObject 的 CoreItemOnWorld 腳本
        corePool = new CoreItemOnWorld[coreObject.Length];
        trailsPool = new TrailsItemOnWorld[trailsObject.Length];
        trailTarget = GameObject.FindGameObjectWithTag("TrailTarget").transform;
        poolParent = GameObject.FindGameObjectWithTag("PoolParent");
        bezierHandleParent = GameObject.FindGameObjectWithTag("BezierHandleParent");

        for (int i = 0; i < corePool.Length; i++)
        {
            corePool[i] = Instantiate(coreObject[i]).GetComponent<CoreItemOnWorld>();
            corePool[i].transform.parent = poolParent.transform;            
        }
        for (int i = 0; i < trailsPool.Length; i++)
        {
            trailsPool[i] = Instantiate(trailsObject[i]).GetComponent<TrailsItemOnWorld>();
            trailsPool[i].transform.parent = poolParent.transform;
        }
       
        for (int i = 0; i < bezierHandle.Length; i++)
        {
            bezierHandle[i] = bezierHandleParent.gameObject.transform.GetChild(i);
        }

    }
    private void FixedUpdate()
    {
        TrailMoveToTarget();
    }
    private void Update()
    {
        //if (isBool)
        //{
        //    if (_duration > Duration)
        //    {
        //        _duration = 0;
        //    }
        //    var t = _duration / Duration;
        //    int activeCount = 0;
        //    for (int i = 0; i < corePool.Length; i++)
        //    {
        //        if (corePool[i].isActive && activeCount < trailsPool.Length)
        //        {
        //            // 檢查軌跡是否已經到達 trailTarget
        //            if (Vector3.Distance(trailsPool[activeCount].transform.position, trailTarget.position) > 0.01f)
        //            {
        //                // 如果還沒有到達，則更新軌跡的位置
        //                trailsPool[activeCount].transform.position = Mathf.Pow(1 - t, 2) * P0[i].position + 2 * t * (1 - t) * P1[i].position + Mathf.Pow(t, 2) * trailTarget.position;
        //                activeCount++;
        //            }
        //        }
        //    }
        //    _duration += Time.deltaTime;
        //}
        //TrailMoveToTarget();


        HandleCooldown();
    }

    void HandleAbsorption()
    {
        for (int i = 0; i < corePool.Length; i++)
        {
            if (corePool[i].isActive)
            {
                int currentCoreIndex = currentAbsorbCore[i];
                corePool[currentAbsorbCore[currentCoreIndex]].TurnOff();
                CoreInventory.Instance.coreBool[currentCoreIndex] = true;
            }              
        }
        // 檢測是否正在被吸收中
        //if (isBeingAbsorbed)
        //{
        //    // 更新吸收進度條
        //    absorptionTimer += Time.deltaTime;
        //}
        // 檢測吸收是否完成
        //if (absorptionTimer >= absorptionTime)
        //{
            // 吸收完成，隱藏該核心物件
            //absorptionTimer = 0.0f;
            //isBeingAbsorbed = false;
            //isCoreAbsorbed = true;

            //int activeCount = 0;
            //for (int i = 0; i < corePool.Length; i++)
            //{
            //    if (corePool[i].isActive && activeCount < trailsPool.Length)
            //    {
            //        //trailsPool[activeCount].transform.position = Vector3.Lerp(trailTarget.position, trailsPool[activeCount].transform.position, absorptionTimer * Time.deltaTime);
            //        activeCount++;

            //        int currentCoreIndex = currentAbsorbCore[i];
            //        corePool[currentAbsorbCore[currentCoreIndex]].TurnOff();
            //        CoreInventory.instance.coreBool[currentCoreIndex] = true;
            //    }
            //}

            //for (int i = 0; i < corePool.Length; i++)
            //{
            //    if (corePool[i].isActive)
            //    {

            //        int currentCoreIndex = currentAbsorbCore[i];
            //        corePool[currentAbsorbCore[currentCoreIndex]].TurnOff();
            //        CoreInventory.instance.coreBool[currentCoreIndex] = true;
            //    }
            //}
        //}
    }

    void HandleCooldown()
    {
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

            if (SceneController.Instance.outDoor)
                CoreAbuility.OutDoorInitialState();
            else
                CoreAbuility.InDoorInitialState();

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
        if (Int == 1)
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
            Debug.Log("使用核心能力: " + 2);
            coolTimer = coolTime;

        }
        else if (Int == 3)
        {
            useTimer = 0;
            CoreAbuility.ImproveDefense();
            Debug.Log("使用核心能力: " + 3);
            coolTimer = coolTime;
        }
        isUseTime = true;

    }

    //抓取corePoint位置 與從新計算當前編號
    public void TureOnCore(Transform enemypoint, int enemyType)
    {
        corePool[enemyType].transform.position = enemypoint.position;
        corePool[enemyType].transform.rotation = enemypoint.rotation;
        corePool[enemyType].TurnOn();
        isCoreTurnOn = true;
    }

    public void TurnOnTrail()
    {
        int activeCount = 0;
        for (int i = 0; i < corePool.Length; i++)
        {
            if (corePool[i].isActive && activeCount < trailsPool.Length)
            {
                trailsPool[activeCount].transform.position = corePool[i].transform.position;
                trailsPool[activeCount].transform.rotation = corePool[i].transform.rotation;
                trailsPool[activeCount].TurnOn();
                activeCount++;

                //P0[i].transform.position = corePool[i].transform.position;
                bezierHandle[i].transform.position = new Vector3(corePool[i].transform.position.x, corePool[i].transform.position.y + 5, corePool[i].transform.position.z);
            }
        }
    }

    public void TurnOffTrail()
    {
        for (int i = 0; i < trailsPool.Length; i++)
        {
            trailsPool[i].TurnOff();
        }
    }

    void TrailMoveToTarget()
    {
        if (isBeingAbsorbed)
        {
            if (_duration > Duration)
            {
                _duration = 0;
            }
            var t = _duration / Duration;
            int activeCount = 0;
            float arrivalThreshold = 0.01f;
            for (int i = 0; i < corePool.Length; i++)
            {
                if (corePool[i].isActive && activeCount < trailsPool.Length)
                {
                    float distanceToTarget = Vector3.Distance(trailsPool[activeCount].transform.position, trailTarget.position);

                    // 檢查軌跡是否已經到達 trailTarget
                    if (distanceToTarget <= arrivalThreshold)
                    {
                        HandleAbsorption();
                        isCoreTurnOn = false;
                    }
                    else
                    {
                        // 如果還沒有到達，則更新軌跡的位置
                        trailsPool[activeCount].transform.position = Mathf.Pow(1 - t, 2) * corePool[i].transform.position + 2 * t * (1 - t) * bezierHandle[i].position + Mathf.Pow(t, 2) * trailTarget.position;
                        activeCount++;
                    }
                }
            }
            _duration += Time.deltaTime;
        }

       
    }
}
