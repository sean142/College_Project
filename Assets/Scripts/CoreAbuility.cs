using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreAbuility : MonoBehaviour
{
    public static void SpeedActivateAndNightVision()
    {
        // 以某種方式實現速度提升能力
        PlayerController.instance.currentSpeed = PlayerController.instance.runSpeed;
        Debug.Log("speed");

        // 使用Shader Graph在黑暗中實現夜視能力
        float fogStartDistance = RenderSettings.fogStartDistance;
        float fogEndDistance = 100f; // 宣告一個float變數
        RenderSettings.fogEndDistance = fogEndDistance; // 將變數值設定給Fog的結束距離
        Debug.Log("當前Fog起始距離為: " + RenderSettings.fogStartDistance);
        Debug.Log("當前Fog結束距離為: " + RenderSettings.fogEndDistance);

        //使用時間12秒，冷卻時間8秒
    }  
    
    public static void StrengthBoost()
    {
        PlayerController.instance.currentSpeed = PlayerController.instance.normalSpeed;
        Debug.Log("normalspeed");      

        float fogEndDistance = 15f; // 宣告一個float變數
        RenderSettings.fogEndDistance = fogEndDistance; // 將變數值設定給Fog的結束距離
        Debug.Log("當前Fog結束距離為: " + RenderSettings.fogEndDistance);

        // 力量提升，能用手擊飛敵人與破壞牆壁。
        Debug.Log("power");
    }

    public static void ImproveDefense()
    {
        PlayerController.instance.currentSpeed = PlayerController.instance.normalSpeed;
        Debug.Log("normalspeed");     

        float fogEndDistance = 15f; // 宣告一個float變數
        RenderSettings.fogEndDistance = fogEndDistance; // 將變數值設定給Fog的結束距離
        Debug.Log("當前Fog結束距離為: " + RenderSettings.fogEndDistance);

        // 提升防禦，受到的傷害大幅減輕。
        Debug.Log("defense");
    }
}
