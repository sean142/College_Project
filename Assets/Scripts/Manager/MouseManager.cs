using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MouseManager : Singleton<MouseManager>
{
    public event Action<GameObject> OnEnemyClicked;

    RaycastHit hitInfo;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        MouseControl();
    }
    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {          
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
                Debug.Log("Test");
            }

            if (hitInfo.collider.gameObject.CompareTag("Attackable"))
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);            
        }
    }
}
