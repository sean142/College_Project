using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SewerPasswordPaper : MonoBehaviour
{
    public GameObject UI; //��ܶ}�������s
    public bool isOnTrigger; //�O�_�btrigger��
    public CinemachineFreeLook followCinema;

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
            followCinema.enabled = false;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOnTrigger = false;

            UI.SetActive(false);
            followCinema.enabled = true;

        }
    }
}
