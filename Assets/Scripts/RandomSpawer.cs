using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawer : MonoBehaviour
{
    public GameObject[] spawnees;
    public GameObject[] spawnPoints;

    private void Awake()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }
    void Start()
    {
        //InvokeRepeating("SpawnRandom", Ins_Time, Ins_Time);
        //SpawnRandom();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SpawnRandom();
        }
    }

    void SpawnRandom()
    {
        int Random_Objects = Random.Range(0, spawnees.Length);
        int Random_Points = Random.Range(0, spawnPoints.Length);
        Instantiate(spawnees[Random_Objects], spawnPoints[Random_Points].transform.position, spawnPoints[Random_Points].transform.rotation);
    }
}
