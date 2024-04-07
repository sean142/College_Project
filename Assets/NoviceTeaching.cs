using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoviceTeaching : MonoBehaviour
{
    public static NoviceTeaching instance;

    public GameObject playerHealthUI;
    public GameObject playerBagUI;
    public GameObject missionUI;
    public GameObject[] noviceTeachingUI;
    private void Awake()
    {
        instance = this;
    }
    public void Update()
    {
        if (noviceTeachingUI[0].activeSelf || noviceTeachingUI[1].activeSelf)
        {
            CheckToMainMenu.instance.checkUI.SetActive(false);
        }

        if (SceneController.Instance.isStandingUp == true)
        {
            noviceTeachingUI[0].SetActive(false);
            noviceTeachingUI[1].SetActive(false);
            playerHealthUI.SetActive(true);
            playerBagUI.SetActive(true);
            missionUI.SetActive(true);
        }
    }

    public void OnButtonOnClicked(int index)
    {
        if (index == 0)
        {
            noviceTeachingUI[0].SetActive(false);
            noviceTeachingUI[1].SetActive(true);
        }
        if (index == 1)
        {
            noviceTeachingUI[1].SetActive(false);
            playerHealthUI.SetActive(true);
            playerBagUI.SetActive(true);
            missionUI.SetActive(true);
            //SceneController.Instance.isStandingUp = true;
            //PlayerController.instance.canMove = true;
        }
    }
}
