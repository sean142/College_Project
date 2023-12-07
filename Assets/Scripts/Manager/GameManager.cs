using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;
    public CharacterStats[] enemyStats;
    public CinemachineFreeLook followCinema;

    List<IEndGameObserver> endGameObserver = new List<IEndGameObserver>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
   
    public void RigisterPlayer(CharacterStats player)
    {
        playerStats = player;

        followCinema = FindObjectOfType<CinemachineFreeLook>();
       
        if (followCinema != null)
        {
            followCinema.Follow = playerStats.transform;
            followCinema.LookAt = playerStats.transform;
            followCinema.m_YAxis.m_MaxSpeed = 0;
            followCinema.m_XAxis.m_MaxSpeed = 0;
        }
    }
    public void RigisterEnemy()
    {      
        enemyStats = new CharacterStats[EnemyManager.instance.enemiesPool.Length];

        for (int i = 0; i < EnemyManager.instance.enemiesPool.Length; i++)
        {
            enemyStats[i] = EnemyManager.instance.enemiesPool[i].GetComponentInChildren<CharacterStats>();
        }
    }

    public void AddObserver(IEndGameObserver observer)
    {
        endGameObserver.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObserver.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in endGameObserver)
            observer.EndNotify();
    }
    
    public Transform GetEntrance()
    {
        foreach (var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.destinationTag == TransitionDestination.DestinationTag.ENTER)
                return item.transform;
        }
        return null;
    }
}
