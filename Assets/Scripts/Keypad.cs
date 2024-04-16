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
    public AudioSource audioSource;

    void Awake()
    {
        password = "5137";
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
            Debug.Log("成功");
            audioSource.Play();
            StartCoroutine(Static.DelayToInvokeDo(() =>
            {
                UncleHouseDoor.instance.animator.SetBool("isOpen", true);
            }, 1f));
        }
        else
        {
            Debug.Log("不成功");
            charHolder.text = "";
        }
    }

    public void Clear()
    {
        charHolder.text = "";
    }
}
