using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacePoolObject : MonoBehaviour
{
    public bool isActive;   

    public virtual void TurnOn()
    {
        isActive = true;
    }

    public virtual void TurnOff()
    {
        isActive = false;
    }
}
