using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public EnemyController[] enemies;
    //public GameObject[] enemyPool;
    private void Awake()
    {
        instance = this;
        //enemyPool[0] = GameObject.Find("Mouse");
        //enemyPool[1] = GameObject.Find("Mouse (1)");
        //enemyPool[2] = GameObject.Find("Mouse (2)");

    }
}
