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
        float fillB = backHealthBar.fillAmount;       
        float sliderPercent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;
        if (fillB > sliderPercent)
        {
            frountHealthBar.fillAmount = sliderPercent;
            backHealthBar.color = Color.red;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, sliderPercent, percentComplete);
        }
    }
}
