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
    private void Awake()
    {
        instance = this;
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        Shake();
    }
  
    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                for (int i = 0; i < 3; i++)
                {
                    noise = freeLookCamera.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                    noise.m_AmplitudeGain = 0;
                }
            }
        }
      
    }
    public void ShakeCamera(float intensity, float time)
    {
        // Iterate over each rig and access the Perlin component
        for (int i = 0; i < 3; i++)
        {
            noise = freeLookCamera.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (noise != null)
            {
                noise.m_AmplitudeGain = intensity;
                shakeTimer = time;
            }
        }
        
    }

    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        // Shake each rig of the FreeLook Camera
        for (int i = 0; i < 3; i++) // Three rigs: 0 for bottom, 1 for mid, 2 for top
        {
            CinemachineVirtualCamera vCam = freeLookCamera.GetRig(i);
            if (vCam != null)
            {
                CinemachineBasicMultiChannelPerlin perlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                if (perlin != null)
                {
                    perlin.m_AmplitudeGain = shakeIntensity;
                    yield return new WaitForSeconds(shakeDuration);
                    perlin.m_AmplitudeGain = 0f;
                }
            }
        }
    }
}

