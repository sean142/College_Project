using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreAbuility : MonoBehaviour
{
    public static void SpeedActivateAndNightVision()
    {
        // �H�Y�ؤ覡��{�t�״��ɯ�O
        PlayerController.instance.currentSpeed = PlayerController.instance.runSpeed;
        //�ϥήɶ�12��A�N�o�ɶ�8��
        // �ϥ�Shader Graph�b�·t����{�]����O
        Debug.Log("speed");
    }  
    
    public static void StrengthBoost()
    {
        PlayerController.instance.currentSpeed = PlayerController.instance.normalSpeed;

        // �O�q���ɡA��Τ������ĤH�P�}�a����C
        Debug.Log("power");
    }

    public static void ImproveDefense()
    {
        PlayerController.instance.currentSpeed = PlayerController.instance.normalSpeed;

        // ���ɨ��m�A���쪺�ˮ`�j�T��C
        Debug.Log("defense");
    }
}
