using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public static FadeOut instance;
    public float restartTimer;
    private CanvasGroup fadeCanvasGroup;
    public GameObject[] UI;    //FadeOut 運行時 場景的UI關閉

    void Awake()
    {
        instance = this;
        fadeCanvasGroup = GetComponent<CanvasGroup>();

        UI[0] = GameObject.FindGameObjectWithTag("PlayerHPUI");
        UI[1] = GameObject.FindGameObjectWithTag("BagUI");
        UI[2] = GameObject.FindGameObjectWithTag("MinimapUI");
        UI[3] = GameObject.FindGameObjectWithTag("BackGroundUI");

        
    }

    public void TurnOnFadeOut()
    {
        StartCoroutine(InstallFadeOutScene());
    }

    IEnumerator InstallFadeOutScene()
    {
        float currentTime = 0f;
        while (currentTime < restartTimer)
        {
            currentTime += Time.deltaTime;
            fadeCanvasGroup.alpha = 0 + (currentTime / restartTimer);
            PlayerController.instance.canMove = false;
            UI[0].SetActive(false);
            UI[1].SetActive(false);
            UI[2].SetActive(false);
            UI[3].SetActive(false);
            yield return null;
        }
        fadeCanvasGroup.alpha = 1;
    }
}
