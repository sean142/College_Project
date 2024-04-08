using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class TeachingUI : MonoBehaviour
{
    public static TeachingUI instance;
    public GameObject UI;
    public CinemachineFreeLook followCinema;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (UI.activeSelf)
        {
            followCinema.enabled = false;
            CheckToMainMenu.instance.checkUI.SetActive(false);
            PlayerController.instance.canMove = false;
        }
    }
    public void OnButtonOnClicked()
    {
        UI.SetActive(false);
        followCinema.enabled = true;
        PlayerController.instance.canMove = true;

    }
}
