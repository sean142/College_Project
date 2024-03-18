using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewerRailing : MonoBehaviour
{
    public static SewerRailing instance;
    public GameObject UI; //顯示開門的按鈕
    public Animator animator;
    public bool isOnTrigger; //是否在trigger內
    private void Awake()
    {
        //animator = GetComponent<Animator>();
        instance = this;
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

            animator.SetBool("isClose", true);
        }
    }
}
