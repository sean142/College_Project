using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker instance;
    public CinemachineFreeLook freeLookCamera;
    public float shakeTimer;
    public CinemachineBasicMultiChannelPerlin noise;
    public float shakeIntensity = 1f; // Intensity of the shake
    public float shakeDuration = 0.3f; // Duration of the shake
    public float amplitudeGain;
    public float frequemcyGain;
    private void Awake()
    {
        instance = this;
        freeLookCamera = GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                {
                    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = 
                        freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                    cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
                }
            }
        }

    }
    


    public void ShakeCamera(float intensity, float time)
    {
        //freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        //freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        //freeLookCamera.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;

        //freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
        //freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
        //freeLookCamera.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }
}

