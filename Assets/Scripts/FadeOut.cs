using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public static FadeOut instance;
    public float restartTimer;
    public bool canTran;
    private CanvasGroup fadeCanvasGroup;

    void Awake()
    {
        instance = this;
        fadeCanvasGroup = GetComponent<CanvasGroup>();
    }

    public IEnumerator InstallFadeOutScene()
    {
        float currentTime = 0f;
        while (currentTime < restartTimer)
        {
            currentTime += Time.deltaTime;
            fadeCanvasGroup.alpha = 0 + (currentTime / restartTimer);
            PlayerController.instance.canMove = false;
            yield return null;
        }
        canTran = true;
        fadeCanvasGroup.alpha = 1;
    }
}
