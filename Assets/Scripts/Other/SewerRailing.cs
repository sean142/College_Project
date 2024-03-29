using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SewerRailing : MonoBehaviour
{
    public static SewerRailing instance;
    public GameObject UI; //��ܶ}�������s
    public Animator animator;
    public bool isOnTrigger; //�O�_�btrigger��
    public GameObject checkUI;
    public CinemachineFreeLook followCinema;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
        instance = this;
    }

    public void OnButtonClicked(int index)
    {
        // �p�G���a���U�O
        if(index == 0)
        {            
            checkUI.SetActive(false);
            UI.SetActive(true);
        }
        if (index==1)
        {
            checkUI.SetActive(false);
            followCinema.enabled = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOnTrigger = true;
            //UI.SetActive(true);
            checkUI.SetActive(true);
            followCinema.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOnTrigger = false;

            UI.SetActive(false);
            checkUI.SetActive(false);
            followCinema.enabled = true;
        }
    }
}
