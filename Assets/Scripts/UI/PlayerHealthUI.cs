using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    Image frountHealthBar;
    Image backHealthBar;

    public float chipSpeed = 20f;
    public float lerpTimer = 1f;
    public bool isReturningFromMenu;
    public float sliderPercent;

    private void Awake()
    {
        backHealthBar = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        frountHealthBar = transform.GetChild(0).GetChild(1).GetComponent<Image>();
    }
 
    private void Update()
    {
        UpdateHealth();
    }

    void UpdateHealth()
    {
        //在玩家從主選單返回遊戲時，直接顯示當前的血量，而不需要透過延遲扣寫的方式逐漸減少到當前血量。
        //同時，當玩家的血量變化時，前景血條和背景血條的填充量也能夠透過延遲扣寫的方式逐漸變化。

        float fillB = backHealthBar.fillAmount;
        sliderPercent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;

        frountHealthBar.fillAmount = sliderPercent;
        backHealthBar.fillAmount = sliderPercent;       

        if (isReturningFromMenu)
        {
            fillB = sliderPercent;
            isReturningFromMenu = false;
        }

        if (fillB > sliderPercent)
        {
            frountHealthBar.fillAmount = sliderPercent;
            backHealthBar.color = Color.red;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, sliderPercent, percentComplete);
        }

        //float fillB = backHealthBar.fillAmount;
        //sliderPercent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;

        //frountHealthBar.fillAmount = sliderPercent;
        //backHealthBar.fillAmount = sliderPercent;

        //if (fillB > sliderPercent)
        //{
        //    frountHealthBar.fillAmount = sliderPercent;
        //    backHealthBar.color = Color.red;
        //    float percentComplete = lerpTimer / chipSpeed;
        //    backHealthBar.fillAmount = Mathf.Lerp(fillB, sliderPercent, percentComplete);
        //    Debug.LogError("ContinueTEST");
        //}



        //float sliderPercent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;
        //frountHealthBar.fillAmount = sliderPercent;
    }
}
