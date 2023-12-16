using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button newGameBtn;
    public Button continueBtn;
    public Button exitBtn;

    void Awake()
    {
        newGameBtn = transform.GetChild(0).GetComponent<Button>();
        newGameBtn.onClick.AddListener(NewGame);

        continueBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn.onClick.AddListener(ContinueGame);

        exitBtn = transform.GetChild(2).GetComponent<Button>();
        exitBtn.onClick.AddListener(ExitGame);
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();

        SceneController.Instance.TransitionToFirstLevel();

        SceneController.Instance.isFirstTimeInGame = true;
        SceneController.Instance.isStandingUp = false;
    }

    public void ContinueGame()
    {
        SceneController.Instance.TransitionToLoadGame();
        SceneController.Instance.isStandingUp = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
