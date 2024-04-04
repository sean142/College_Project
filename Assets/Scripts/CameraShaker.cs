using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker instance;
    public CinemachineFreeLook freeLookCamera;
    public float shakeTimer;
    public  CinemachineBasicMultiChannelPerlin[] perlinChannels;
    public GameObject[] textUI; 

    private void Awake()
    {
        instance = this;
        freeLookCamera = GetComponent<CinemachineFreeLook>();

        // 初始化 perlinChannels 数组
        perlinChannels = new CinemachineBasicMultiChannelPerlin[3];
        for (int i = 0; i < 3; i++)
        {
            perlinChannels[i] = freeLookCamera.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }
    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                {
                    for (int i = 0; i < 3; i++)
                    {
                        perlinChannels[i].m_AmplitudeGain = 0;
                    }
                }
            }
        }
    }
    public void ShakeCamera(float amplitude, float time)
    {      
        for (int i = 0; i < 3; i++)
        {
            perlinChannels[i].m_AmplitudeGain = amplitude;
        }
        shakeTimer = time;
    }
   
}

