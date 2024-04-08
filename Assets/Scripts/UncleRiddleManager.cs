using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UncleRiddleManager : MonoBehaviour
{
    public static UncleRiddleManager instance;
    public GameObject[] UI; // ����UI

    public GameObject checkUI;
    public CinemachineFreeLook followCinema;
    public int riddleNumber;
    private void Awake()
    {
        instance = this;
    }
   
    public void OnButtonClicked(int index)
    {
        // �p�G���a���U�O
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
