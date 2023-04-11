using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreAbuility : MonoBehaviour
{
    public static void SpeedActivateAndNightVision()
    {
        // 以某種方式實現速度提升能力
        PlayerController.instance.currentSpeed = PlayerController.instance.runSpeed;
        //使用時間12秒，冷卻時間8秒
        // 使用Shader Graph在黑暗中實現夜視能力
        Debug.Log("speed");
    }  
    
    public static void StrengthBoost()
    {
        PlayerController.instance.currentSpeed = PlayerController.instance.normalSpeed;

        // 力量提升，能用手擊飛敵人與破壞牆壁。
        Debug.Log("power");
    }

    public static void ImproveDefense()
    {
        PlayerController.instance.currentSpeed = PlayerController.instance.normalSpeed;

        // 提升防禦，受到的傷害大幅減輕。
        Debug.Log("defense");
    }
}
