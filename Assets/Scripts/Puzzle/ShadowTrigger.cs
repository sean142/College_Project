using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTrigger : MonoBehaviour
{
    public float distanceThreshold = 0.1f;  // 允許的距離差距
    public GameObject[] movableObject;      // 玩家可選擇移動的物體
    public GameObject pointA;               // 指定點
    public GameObject selectedObject;       // 當前所點擊的物件
    private Vector3 offset;                 
    public GameObject targetObject;         // 指定物(這物件能與牆壁物件吻合)
    public static bool shadowCheck;

    private void Start()
    {
        movableObject = new GameObject[3];
        movableObject[0] = GameObject.FindGameObjectWithTag("A"); 
        movableObject[1] = GameObject.FindGameObjectWithTag("B");
        movableObject[2] = GameObject.FindGameObjectWithTag("C");

        targetObject = movableObject[0];
    }

    void Update()
    {
        ShadowCheck();
        //SelectedObject();
    }

    void SelectedObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 檢查滑鼠點擊位置是否點擊到物體，如果是，拾取物體
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("A") || hit.collider.CompareTag("B") || hit.collider.CompareTag("C"))
                {
                    selectedObject = hit.collider.gameObject; 
                    offset = selectedObject.transform.position - hit.point;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // 釋放物體
            selectedObject = null;
        }

        if (selectedObject != null)
        {
            // 移動目標物體到滑鼠位置
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //鎖定Z軸
                //Vector3 newPosition = hit.point + offset;
                //newPosition.z = selectedObject.transform.position.z;
                //selectedObject.transform.position = newPosition;

                //鎖定Z軸
                Vector3 newPosition = new Vector3(hit.point.x + offset.x, hit.point.y + offset.y, selectedObject.transform.position.z);
                selectedObject.transform.position = newPosition;

                //selectedObject.transform.position = hit.point + offset;
            }
        }
    }

    void ShadowCheck()
    {
        if (targetObject != null)
        {
            float distance = Vector3.Distance(targetObject.transform.position, pointA.transform.position);

            if (distance <= distanceThreshold)
            {
                Debug.Log("影子吻合！");
                shadowCheck = true;
            }
        }
    }
}
