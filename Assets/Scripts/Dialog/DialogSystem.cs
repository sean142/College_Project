using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public GameObject playerObj;
    public Text textLabel;
    public Image faceImage;
    public Image firstImage;
    public GameObject firstUI;

    public TextAsset textFile;
    public int index;
    public float textSpeed;

    public Sprite face01, face02;

    private bool textFinished; 
    private bool canceTyping;     

    List<string> textList = new List<string>();
    
    private void Awake()
    {
        GetTextFormFile(textFile);
    }

    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        textFinished = true;

        StartCoroutine(SetTextUI());
    }
   
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter)&&index == textList.Count)
        {
            gameObject.SetActive(false);
            index = 0;
            PlayerController.instance.canMove = true;
            PlayerController.instance.canTalk = false;
            PlayerController.instance.canAnimator = true;
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (textFinished && !canceTyping)
            {
                StartCoroutine(SetTextUI());
            }
            else if (!textFinished&&!canceTyping)
            {
                canceTyping =true;
            }
        }    
    }

    void GetTextFormFile(TextAsset file)
    {
        textList.Clear();
        index = 0;

        var lineDate = file.text.Split('\n');

        foreach (var line in lineDate)
        {
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI()
    {
        textFinished = false;
        textLabel.text = "";

        switch (textList[index])
        {
            case"¥D¨¤":
                faceImage.sprite = face01;
                index++;
                break;
            case "¨û¨û":
                faceImage.sprite = face02;
                index++;
                break;
        }
        
        int letter = 0;
        while (!canceTyping && letter < textList[index].Length - 1)
        {
            textLabel.text += textList[index][letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }

        textLabel.text = textList[index];
        canceTyping = false;
        textFinished = true;
        index++;
    }
}
