using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGate : MonoBehaviour
{
    public Animator animator;
    public GameObject gate;
    public void Awake()
    {
        animator = gate.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("GateOpen");
        }    
    }
}
