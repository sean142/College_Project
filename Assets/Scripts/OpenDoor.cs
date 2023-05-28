using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject UI; //顯示開門的按鈕
    private Animator animator;
    public bool isOnTrigger; //是否在trigger內

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)&&isOnTrigger)
        {
            animator.SetBool("isOpen", true);
        }
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

            animator.SetBool("isOpen", false);
            animator.SetBool("isClose", true);
        }
    }
}
