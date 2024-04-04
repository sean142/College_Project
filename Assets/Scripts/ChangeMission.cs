using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMission : MonoBehaviour
{
    public GameObject[] textUI;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            textUI[0].SetActive(false);
            textUI[1].SetActive(true);
        }
    }
}
