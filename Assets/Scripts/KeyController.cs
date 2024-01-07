using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public static KeyController instance;
    public bool isOnKeyRange;           //判斷是否在鑰匙範圍內
    public bool isBeingAbsorbKey;       //判斷是否整正在吸收鑰匙
    public bool isHaveKey;              //判斷是否已拿到鑰匙
    public int Duration = 5;
    private float _duration;
    public Transform trailTarget;
    public GameObject key;
    public Transform point;

    private void Start()
    {
        instance = this;

        trailTarget = GameObject.FindGameObjectWithTag("TrailTarget").transform;
        key = GameObject.FindGameObjectWithTag("Key");
    }

    private void FixedUpdate()
    {
        TrailMoveToTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isOnKeyRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOnKeyRange = false;

        }
    }

    void TrailMoveToTarget()
    {
        if (isBeingAbsorbKey)
        {
            if (_duration > Duration)
            {
                _duration = 0;
            }
            var t = _duration / Duration;
            float arrivalThreshold = 0.01f;
            float distanceToTarget = Vector3.Distance(CoreManager.Instance.trailsPool[0].transform.position, trailTarget.position);
            if (distanceToTarget <= arrivalThreshold)
            {
                if (key.activeSelf == true)
                {
                    key.SetActive(false);
                    isHaveKey = true;
                    Debug.Log("獲得鑰匙");
                }
            }
            else
            {
                CoreManager.Instance.trailsPool[0].transform.position = Mathf.Pow(1 - t, 2) * key.transform.position + 2 * t * (1 - t) * point.position + Mathf.Pow(t, 2) * trailTarget.position;
            }
            _duration += Time.deltaTime;
        }
    }

    public void TurnOnTrail()
    {
        CoreManager.Instance.trailsPool[0].transform.position = key.transform.position;
        CoreManager.Instance.trailsPool[0].TurnOn();

        //point.transform.position = new Vector3(key.transform.position.x, key.transform.position.y , key.transform.position.z);
    }

    public void TurnOffTrail()
    {
        CoreManager.Instance.trailsPool[0].TurnOff();
    }
}

