using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject UI; //��ܶ}�������s
    private Animator animator;
    public bool isOnTrigger; //�O�_�btrigger��
    public bool isDoorOpen;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && isOnTrigger &&!isDoorOpen)
        {
            animator.SetTrigger("isOpen");
            isDoorOpen = true;
            UI.SetActive(false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("isClose"))
            animator.SetBool("isClose", false);     
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
            isDoorOpen = false;

            UI.SetActive(false);

            animator.SetBool("isClose", true);
        }
    }
}
