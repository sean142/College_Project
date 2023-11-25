using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthUIPrefab;
    public Transform barPoint;
    public bool alwaysVisible;
    public float visibleTime;

    public Image healthSlider;
    public Transform UIBar;
    public Transform cam;
    public float timeLeft;

    CharacterStats currentStats;

    public  Image frountHealthBar;
    public  Image backHealthBar;

    public float chipSpeed = 20f;
    public float lerpTimer = 1f;
    public bool isReturningFromMenu;
    public float sliderPercent;

    private void Awake()
    {
        currentStats = GetComponent<CharacterStats>();

        currentStats.UpdataHealthBarOnAttack += UpdateHealthBar;

        //backHealthBar = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        //frountHealthBar = transform.GetChild(0).GetChild(1).GetComponent<Image>();
    }

    private void OnEnable()
    {
        cam = Camera.main.transform;

        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                UIBar = Instantiate(healthUIPrefab, canvas.transform).transform;
                //backHealthBar = UIBar.GetChild(0).GetComponent<Image>();
                //frountHealthBar = UIBar.GetChild(1).GetComponent<Image>();
                healthSlider = UIBar.GetChild(0).GetComponent<Image>();
                UIBar.gameObject.SetActive(alwaysVisible);
            }
        }
    }
    public void Update()
    {
        //UpdateHealth();
        //currentStats.UpdataHealthBarOnAttack += UpdateHealthBar;

    }

    private void LateUpdate()
    {
        if (UIBar != null)
        {
            UIBar.position = barPoint.position;
            UIBar.forward = -cam.forward;

            if (timeLeft <= 0 && !alwaysVisible)
                UIBar.gameObject.SetActive(false);
            else
                timeLeft -= Time.deltaTime;
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
            Destroy(UIBar.gameObject);

        //UIBar.gameObject.SetActive(true);
        //timeLeft = visibleTime;

        //float fillB = backHealthBar.fillAmount;

        //sliderPercent = (float)currentHealth / maxHealth;
        //frountHealthBar.fillAmount = sliderPercent;
        //backHealthBar.fillAmount = sliderPercent;

        //if (isReturningFromMenu)
        //{
        //    fillB = sliderPercent;
        //    isReturningFromMenu = false;
        //}

        //if (fillB > sliderPercent)
        //{
        //    frountHealthBar.fillAmount = sliderPercent;
        //    backHealthBar.color = Color.red;
        //    float percentComplete = lerpTimer / chipSpeed;
        //    backHealthBar.fillAmount = Mathf.Lerp(fillB, sliderPercent, percentComplete);
        //}

        float sliderPercent = (float)currentHealth / maxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    void UpdateHealth()
    {
        if (sliderPercent <= 0)
            Destroy(UIBar.gameObject);

        float fillB = backHealthBar.fillAmount;

        //sliderPercent = (float)GameManager.Instance.enemyStats.CurrentHealth / GameManager.Instance.enemyStats.MaxHealth;


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
    }
}
