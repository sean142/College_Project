using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Riddle : MonoBehaviour
{
    //public static Riddle instance;
    public int number;
    public bool isOnTrigger; //¬O§_¦btrigger¤º
    public GameObject checkUI;
    public CinemachineFreeLook followCinema;
    public void Awake()
    {
        //instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UncleRiddleManager.instance.riddleNumber = number;
            isOnTrigger = true;
            checkUI.SetActive(true);
            followCinema.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOnTrigger = false;
            checkUI.SetActive(false);
            followCinema.enabled = true;
            UncleRiddleManager.instance.UI[number].SetActive(false);
        }
    }
}
