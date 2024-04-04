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
        //���a���U�O��
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
