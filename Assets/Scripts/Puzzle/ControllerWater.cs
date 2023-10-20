using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerWater : MonoBehaviour
{
    public GameObject wall;
    public Animator animator;
    public GameObject water;

    private void Awake()
    {
        animator = water.GetComponent<Animator>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")&&ShadowTrigger.shadowCheck)
        {
            wall.SetActive(false);
            animator.SetTrigger("waterDown");
        }
    }
}
