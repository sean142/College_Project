using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterKey : MonoBehaviour
{
    public GameObject[] slot;//�K�X��l�Ʋ�
    string password;//���T�K�X
    public string inputPassword;//��J�K�X
    void Start()
    {
        password = "2534";
    }

    // �ˬd�K�X 
    public void CheckPass()
    {
        //�N�C�ӱK�X�Ӥl�����Ʀr���ꦨ�r�Ŧ�A�M��P�_
        for (int i = 0; i < slot.Length; i++)
        {
            inputPassword = inputPassword + slot[i].GetComponent<NumberMove>().Num;
        }
        if (inputPassword == password)
        {
            print("���ꦨ�\");
            SewerRailing.instance.animator.SetTrigger("isOpen");
        }
        else
        {
            print("�K�X���~");
            inputPassword = null;
        }
    }

}
