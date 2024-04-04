using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGate1 : MonoBehaviour
{
    public static DungeonGate1 instance;
    public Animator animator;
    public GameObject gate;
    public bool isbool; // ����֤߮ɹh���~�|�}��
    public void Awake()
    {
        instance = this;
        animator = gate.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isbool)
        {
            animator.SetTrigger("GateOpen");
        }    
    }
}
