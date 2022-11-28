using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkButton : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;

    public GameObject talkUI;
    public GameObject firstUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && player.canTalk == true)
        {
            firstUI.SetActive(false);
            talkUI.SetActive(true);  
        }
    }//在trigger外 不能啟動對話 

    private void OnTriggerEnter(Collider other)
    {        
        if(other.gameObject.tag == "Player")
        {
            player.canMove = false;
            player.canTalk = true;
            firstUI.SetActive(true);           
        }       
    }
}
