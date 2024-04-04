using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckToMainMenu : MonoBehaviour
{
    //public static CheckToMainMenu instance;
    public GameObject checkUI;
    private void Awake()
    {
        //instance = this;
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
        }
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            checkUI.SetActive(true);
        }
    }
}
