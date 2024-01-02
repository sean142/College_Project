using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberMove : MonoBehaviour
{
    public Transform moveObj;       //�n���ʪ�����
    public Transform upPos;
    public Transform startPos;
    public Transform downPos;
    Transform targetPos;            //�u�ʪ��ؼЦ�m
    float moveSpeed;                //�u�ʳt��
    public bool isMove;             //�O�_�b�u��
    public Text middleText;
    public Text downText;
    public Text upText;
    public int Num;                 //�Ψӭp���e��l�����Ʀr

    private void Start()
    {
        moveSpeed = 5f;
        Num = 0;
    }

    void Update()
    {
        if (isMove)
        {
            moveObj.position = Vector3.Lerp(moveObj.position, targetPos.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(moveObj.position, targetPos.position) < 0.5f)
            {
                //�����Ʀr�u�W�h�A�M����ܤ奻�����Ʀr�A�b�k��
                moveObj.position = targetPos.position;
                isMove = false;
                middleText.text = Num.ToString();
                moveObj.position = startPos.position;
            }
        }
    }

    public void PageUp()
    {
        if (!isMove)
        {
            Num += 1;
            if (Num > 9)
            {
                Num = 0;
            }
        }
        targetPos = upPos;
        downText.text = Num.ToString();
        isMove = true;
    }
    
    // �U�u�ʤ�k    
    public void PageDown()
    {
        if (!isMove)
        {
            Num -= 1;
            if (Num < 0)
            {
                Num = 9;
            }
        }
        targetPos = downPos;
        upText.text = Num.ToString();
        isMove = true;
    }

}
