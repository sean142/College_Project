using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NoviceTeaching : MonoBehaviour
{
    public static NoviceTeaching instance;

    public GameObject playerHealthUI;
    public GameObject playerBagUI;
    public GameObject missionUI;
    public GameObject[] noviceTeachingUI;
    public CinemachineFreeLook followCinema;

    private void Awake()
    {
        instance = this;
    }
    public void Update()
    {
        if (noviceTeachingUI[0].activeSelf || noviceTeachingUI[1].activeSelf)
        {
            CheckToMainMenu.instance.checkUI.SetActive(false);
            PlayerController.instance.canMove = false;
            followCinema.enabled = true;
            GameManager.Instance.followCinema.m_YAxis.m_MaxSpeed = 0;
            GameManager.Instance.followCinema.m_XAxis.m_MaxSpeed = 0;
        }

        if (SceneController.Instance.isStandingUp && SceneController.Instance.isButtonPressed)
        {
            noviceTeachingUI[0].SetActive(false);
            noviceTeachingUI[1].SetActive(false);
            playerHealthUI.SetActive(true);
            playerBagUI.SetActive(true);
            missionUI.SetActive(true);
        }
        if (SceneController.Instance.isStandingUp && SceneController.Instance.hasExtraBool)
        {
            noviceTeachingUI[0].SetActive(false);
            noviceTeachingUI[1].SetActive(false);
            playerHealthUI.SetActive(true);
            playerBagUI.SetActive(true);
            missionUI.SetActive(true);
            GameManager.Instance.followCinema.m_YAxis.m_MaxSpeed = 2;
            GameManager.Instance.followCinema.m_XAxis.m_MaxSpeed = 400;
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
            SceneController.Instance.isButtonPressed = true;       
        }
    }  
}
