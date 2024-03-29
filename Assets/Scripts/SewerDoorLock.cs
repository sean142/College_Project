using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewerDoorLock : MonoBehaviour
{
    public GameObject lockUI;
    public Animator sewerDoorAnimator;
    public bool lookingForKey;
    public GameObject pointLight;
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lockUI.SetActive(true);
            lookingForKey = true;

            if (KeyController.instance.isHaveKey)
            {
                sewerDoorAnimator.SetTrigger("OpenDoor");
                lockUI.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lockUI.SetActive(false);
            pointLight.SetActive(true);
        }
    }
}
