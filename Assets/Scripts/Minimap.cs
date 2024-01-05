using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public static Minimap instance;
    public Transform target;
    public float offsetX, offsetY,offsetZ;
    public float lerpSpeed;
    public GameObject miniMap;
    public GameObject maxMap;
    public bool minimapChange;
    public bool maxmapChange;
    public Camera minimapCamera;
    public float minimapFieldOfView;
    public float maxmapFieldOfView;
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
        if (Input.GetKeyDown(KeyCode.M))
        {
            minimapChange = !minimapChange;
            maxmapChange = !maxmapChange;
            miniMap.SetActive(minimapChange);
            maxMap.SetActive(maxmapChange);
            if (maxmapChange)
            {
                offsetY = 5.5f;
                minimapCamera.fieldOfView = maxmapFieldOfView;
            }
            else
                offsetY = 4;
            minimapCamera.fieldOfView = minimapFieldOfView;
        }
    }
}
