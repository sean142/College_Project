﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public static bool isStandingUp; // 用來判斷玩家是否standUp 因為只會觸發一次  有跨場景需要所以一直保持true
    public CharacterStats playerStats;

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
