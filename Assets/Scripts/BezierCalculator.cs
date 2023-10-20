using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCalculator : MonoBehaviour
{
    public int Duration = 5;
    public Transform P0, P1, P2, Bt;

    private float _time;
    private float _duration;

    private void Update()
    {
        if (_duration > Duration)
        {
            _duration = 0;
        }

        var t = _duration / Duration;
        Bt.position =
            Mathf.Pow(1 - t, 2) * P0.position +
            2 * t * (1 - t) * P1.position +
            Mathf.Pow(t, 2) * P2.position;

        _duration += Time.deltaTime;
    }
}
