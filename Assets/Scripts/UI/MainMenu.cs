using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public  Button newGameBtn;
    public Button continueBtn;
    void Awake()
    {
        newGameBtn = transform.GetChild(0).GetComponent<Button>();
        newGameBtn.onClick.AddListener(NewGame);

        continueBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn.onClick.AddListener(ContinueGame);
    }
    public  void NewGame()
    {
        PlayerPrefs.DeleteAll();

        SceneController.Instance.TransitionToFirstLevel();        
    }

    public void ContinueGame()
    {
        SceneController.Instance.TransitionToLoadGame();
    }
}
