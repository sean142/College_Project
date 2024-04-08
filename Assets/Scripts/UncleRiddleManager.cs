using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UncleRiddleManager : MonoBehaviour
{
    public static UncleRiddleManager instance;
    public GameObject[] UI; // 解謎UI

    public GameObject checkUI;
    public CinemachineFreeLook followCinema;
    public int riddleNumber;
    private void Awake()
    {
        instance = this;
    }
   
    public void OnButtonClicked(int index)
    {
        // 如果玩家按下是
        if (index == 0)
        {
            checkUI.SetActive(false);
            //UI.SetActive(true);
            UI[riddleNumber].SetActive(true);
        }
        if (index == 1)
        {
            checkUI.SetActive(false);
            followCinema.enabled = true;
        }
    }  
}
