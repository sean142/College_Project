using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    Image healthSlider;

    private void Awake()
    {
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        UpdateHealth();
    }

    void UpdateHealth()
    {    
        float sliderPercent = (float)GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    }
}
