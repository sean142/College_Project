using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CheckToMainMenu : MonoBehaviour
{
    public static CheckToMainMenu instance;
    public GameObject checkUI;
    public CinemachineFreeLook followCinema;
    private void Awake()
    {
        instance = this;
        followCinema = FindObjectOfType<CinemachineFreeLook>();
    }
    public void ButtonOnClicked(int index)
    {
        //當玩家按下是時
        if (index == 0)
        {
            SceneController.Instance.TransitionToMain();
        }
        if (index == 1)
        {
            checkUI.SetActive(false);
            followCinema.enabled = true;
            PlayerController.instance.canMove = true;
        }
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            checkUI.SetActive(true);          
        }

        if (checkUI.activeSelf)
        {
            followCinema.enabled = false;
            PlayerController.instance.canMove = false;
        }
    }
}
