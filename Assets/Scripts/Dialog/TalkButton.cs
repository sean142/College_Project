using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkButton : MonoBehaviour
{
    public GameObject playerObj;

    public GameObject talkUI;
    public GameObject firstUI;

    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && PlayerController.instance.canTalk == true)
        {
            firstUI.SetActive(false);
            talkUI.SetActive(true);  
        }
    }//在trigger外 不能啟動對話 

    private void OnTriggerEnter(Collider other)
    {        
        if(other.gameObject.tag == "Player" && PlayerController.instance.isGround == true)
        {
            PlayerController.instance.canMove = false;
            PlayerController.instance.canAnimator = false;
            PlayerController.instance.canTalk = true;
            firstUI.SetActive(true);           
        }       
    }
}
