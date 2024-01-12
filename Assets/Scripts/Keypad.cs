using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keypad : MonoBehaviour
{
    public TMP_InputField charHolder;
    public GameObject[] buttons;
    public GameObject enterButton;
    string password = "";

    void Awake()
    {
        password = "2";     
    }

    public void ButtonClick(int buttonNumber)
    {
        if (charHolder.text.Length < 4) 
            charHolder.text = charHolder.text + buttonNumber.ToString();
    }

    public void EnterEvent()
    {
        if (charHolder.text == password)
        {
            Debug.Log("成功");
        }
        else
        {
            Debug.Log("不成功");
            charHolder.text = "";
        }
    }
}
