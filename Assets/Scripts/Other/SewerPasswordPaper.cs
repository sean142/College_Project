using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewerPasswordPaper : MonoBehaviour
{
    public GameObject UI; //��ܶ}�������s
    public bool isOnTrigger; //�O�_�btrigger��
    private void Awake()
    {
        //animator = GetComponent<Animator>();
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOnTrigger = true;
            UI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOnTrigger = false;

            UI.SetActive(false);
        }
    }
}
