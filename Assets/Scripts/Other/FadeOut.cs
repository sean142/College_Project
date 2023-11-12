using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public static FadeOut instance;
    public float restartTimer;
    public bool canTran;
    private CanvasGroup fadeCanvasGroup;
    public GameObject[] UI;    //FadeOut 運行時 場景的UI關閉

    void Awake()
    {
        instance = this;
        fadeCanvasGroup = GetComponent<CanvasGroup>();

        UI[0] = GameObject.FindGameObjectWithTag("PlayerHPUI");
        UI[1] = GameObject.FindGameObjectWithTag("BagUI");
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
            yield return null;
        }
        canTran = true;
        fadeCanvasGroup.alpha = 1;
    }
}
