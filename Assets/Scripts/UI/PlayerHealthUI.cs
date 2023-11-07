using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public static PlayerHealthUI instance;

    public  Image frountHealthBar;
    public  Image backHealthBar;
    public float chipSpeed = 20f;
    public float lerpTimer = 1f;
    public float sliderPercent;

    private void Awake()
    {
        instance = this;

        backHealthBar = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        frountHealthBar = transform.GetChild(0).GetChild(1).GetComponent<Image>();
    }
 
    private void Update()
    {
        UpdateHealth();
        //Debug.Log("" + GameManager.Instance.playerStats.CurrentHealth);
       
    }

    void UpdateHealth()
    {
        //如果要做存讀檔 要使用下面代碼
        //ConutnueButton按下去後 會觸發fillB > sliderPercent 但我不要跑延遲扣寫 讓血量條直接到當前血量 
        //當前血量要存著，當移動至其他場景中，才不會再觸發延遲扣寫代碼
        //寫一個sliderPercent的腳本來記錄
        //使用 dontdestroyonload 做UI 但回到Menu時 我不想讓ui出現
        float fillB = backHealthBar.fillAmount;
        sliderPercent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;

        frountHealthBar.fillAmount = sliderPercent;
        backHealthBar.fillAmount = sliderPercent;

        if (fillB > sliderPercent)
        {
            frountHealthBar.fillAmount = sliderPercent;
            backHealthBar.color = Color.red;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, sliderPercent, percentComplete);
            Debug.LogError("ContinueTEST");
        }

        //float fillB = backHealthBar.fillAmount;

        //frountHealthBar.fillAmount = RecordCurrentHPValue.instance.sliderPercent;
        //backHealthBar.fillAmount = RecordCurrentHPValue.instance.sliderPercent;

        //if (fillB > RecordCurrentHPValue.instance.sliderPercent)
        //{
        //    frountHealthBar.fillAmount = RecordCurrentHPValue.instance.sliderPercent;
        //    backHealthBar.color = Color.red;
        //    float percentComplete = lerpTimer / chipSpeed;
        //    backHealthBar.fillAmount = Mathf.Lerp(fillB, RecordCurrentHPValue.instance.sliderPercent, percentComplete);
        //    Debug.LogError("ContinueTEST");
        //}

        //float sliderPercent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;
        //frountHealthBar.fillAmount = sliderPercent;
    }
}
