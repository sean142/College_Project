using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public static Minimap instance;
    public Transform target;
    public float offsetX, offsetY,offsetZ;
    public float lerpSpeed;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("MinimapPoint").transform;
        //target.transform.position = SceneController.Instance.player.transform.position;
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x + offsetX, target.position.y + offsetY, target.position.z + offsetZ), lerpSpeed);
    }
}
