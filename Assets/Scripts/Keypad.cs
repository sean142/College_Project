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
        //charHolder.text = "";
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
            Debug.Log("���\");
            UncleHouseDoor.instance.animator.SetBool("isOpen",true);
        }
        else
        {
            Debug.Log("�����\");
            charHolder.text = "";
        }
    }

    public void Clear()
    {
        charHolder.text = "";
    }
}
