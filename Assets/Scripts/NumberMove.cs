using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberMove : MonoBehaviour
{
    public Transform moveObj;       //要移動的物體
    public Transform upPos;
    public Transform startPos;
    public Transform downPos;
    Transform targetPos;            //滾動的目標位置
    float moveSpeed;                //滾動速度
    public bool isMove;             //是否在滾動
    public Text middleText;
    public Text downText;
    public Text upText;
    public int Num;                 //用來計算當前格子中的數字

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
                //先讓數字滾上去，然後改變文本中的數字，在歸位
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
    
    // 下滾動方法    
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
