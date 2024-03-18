using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterKey : MonoBehaviour
{
    public GameObject[] slot;//密碼格子數組
    string password;//正確密碼
    public string inputPassword;//輸入密碼
    void Start()
    {
        password = "2534";
    }

    // 檢查密碼 
    public void CheckPass()
    {
        //將每個密碼個子中的數字拼湊成字符串，然後判斷
        for (int i = 0; i < slot.Length; i++)
        {
            inputPassword = inputPassword + slot[i].GetComponent<NumberMove>().Num;
        }
        if (inputPassword == password)
        {
            print("解鎖成功");
            SewerRailing.instance.animator.SetTrigger("isOpen");
        }
        else
        {
            print("密碼錯誤");
            inputPassword = null;
        }
    }

}
