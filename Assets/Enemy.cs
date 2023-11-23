using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy instance;
    public EnemyController[] enemies;

    private void Awake()
    {
        instance = this;    
    }   
}
