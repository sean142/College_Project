using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreItemOnWorld : BacePoolObject
{
    public GameObject core1;

     void Awake()
     {
        TurnOff();
     }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CoreManager.instance.isTrigger = true;

            Debug.Log("開始吸收");
        }
    }

    void OnTriggerStay(Collider other)
    {     
        // 檢測是否正在被吸收中
        if (CoreManager.instance.isBeingAbsorbed)
        {
            // 檢測是否被攻擊中斷吸收
            if (other.gameObject.CompareTag("Enemy"))
            {
                CoreManager.instance.isBeingAbsorbed = false;
                CoreManager.instance.absorptionTimer = 0.0f;
            }
            else
            {
                // 更新吸收計時器
                CoreManager.instance.absorptionTimer += Time.deltaTime;
                // 更新吸收進度條

                // 如果吸收完成，將核心加入背包，並且隱藏提示和進度條
                if (CoreManager.instance.absorptionTimer >= CoreManager.instance.absorptionTime)
                {
                    CoreManager.instance.isBeingAbsorbed = false;
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public override void TurnOn()
    {
        base.TurnOn();
        this.gameObject.SetActive(true);
    }

    public override void TurnOff()
    {
        base.TurnOff();
        this.gameObject.SetActive(false);
    } 
}
