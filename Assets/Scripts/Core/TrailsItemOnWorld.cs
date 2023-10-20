using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailsItemOnWorld : BacePoolObject
{
    void Awake()
    {
        TurnOff();     
    }
    public override void TurnOn()
    {
        base.TurnOn();
        this.gameObject.SetActive(true);
    }

    public override void TurnOff()
    {
        base.TurnOff();
        this.gameObject.SetActive(false);
    }
}
