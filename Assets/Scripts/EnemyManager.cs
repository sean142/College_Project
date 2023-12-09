using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public EnemyController[] enemiesPool;
    public GameObject[] enemiesObject;
    public bool[] enemiesBool;
    public Transform[] enemiesPositon;

    private void Awake()
    {
        instance = this;     
    }

    private void Start()
    {
        int trueIndex = 0;
        for (int i = 0; i < enemiesBool.Length; i++)
        {
            if (enemiesBool[i] == true)
            {
                trueIndex++;
            }
        }
        enemiesPool = new EnemyController[trueIndex];

        int j = 0;
        for (int i = 0; i < enemiesBool.Length; i++)
        {
            if (enemiesBool[i] == true)
            {
                enemiesPool[j] = Instantiate(enemiesObject[i]).GetComponent<EnemyController>();
                enemiesPool[j].transform.position = enemiesPositon[i].transform.position;
                enemiesPool[j].guardPos = enemiesPositon[i].transform.position;
                j++;
            }
        }
    }
}
