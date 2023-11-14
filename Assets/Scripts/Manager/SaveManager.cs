using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    string sceneName = "level";
    string coreBoolKey = "coreBoolKey";
    public string SceneName { get { return PlayerPrefs.GetString(sceneName); } }
    public string CoreBoolKey { get { return PlayerPrefs.GetString(coreBoolKey); } }
    public Vector3 playerPosition;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.instance.TransitionToMain();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            SavePlayerData();
            SaveCoreData();
            SavePlayerPositionData();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadPlayerData();
            LoadCoreData();
            LoadPlayerPositionData();
        }
    }
  
    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStats.characterData, GameManager.instance.playerStats.characterData.name);
    }

    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStats.characterData, GameManager.instance.playerStats.characterData.name);
    }

    public void SaveCoreData()
    {
        SaveCore(CoreInventory.Instance.coreBool, coreBoolKey);
    }
   
    public void LoadCoreData()
    {
        CoreInventory.Instance.coreBool = LoadCore(coreBoolKey, CoreInventory.Instance.coreBool.Length);       
    }

    public void SavePlayerPositionData()
    {
        SavePlayerPosition(GameManager.instance.playerStats.transform.position);
        //SavePlayerPosition(SceneController.Instance.player.transform.position);
    }

    public void LoadPlayerPositionData()
    {
        Vector3 playerPosition = LoadPlayerPosition();
        Debug.Log("Loaded Player Position: " + playerPosition);
        //SceneController.Instance.player.transform.position = playerPosition;
        CharacterController characterController = SceneController.Instance.player.GetComponent<CharacterController>();
        if (characterController != null)
        {
            characterController.enabled = false;
            characterController.transform.position = playerPosition;
            characterController.enabled = true;
        }
        //if (SceneController.Instance.player != null)
        //{
        //    SceneController.Instance.player.transform.position = playerPosition;
        //    Debug.Log("Player position set successfully.");

        //    CharacterController characterController = SceneController.Instance.player.GetComponent<CharacterController>();
        //    if (characterController != null)
        //    {
        //        characterController.enabled = false;
        //        characterController.transform.position = playerPosition;
        //        characterController.enabled = true;
        //    }
        //}
        //else
        //{
        //    Debug.LogError("Player object is null. Make sure SceneController.Instance.player is correctly assigned.");
        //}
    }

    public void SavePlayerPosition(Vector3 position)
    {
        playerPosition = position;
        PlayerPrefs.SetFloat("PlayerPosX", position.x);
        PlayerPrefs.SetFloat("PlayerPosY", position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", position.z);

        PlayerPrefs.Save();
    }

    public Vector3 LoadPlayerPosition()
    {
        float x = PlayerPrefs.GetFloat("PlayerPosX");
        float y = PlayerPrefs.GetFloat("PlayerPosY");
        float z = PlayerPrefs.GetFloat("PlayerPosZ");
        return new Vector3(x, y, z);
    }

    public void SaveCore(bool[] BoolArray, string key)
    {
        for (int i = 0; i < BoolArray.Length; i++)
        {
            if(BoolArray[i] == true)
            {
                PlayerPrefs.SetInt(key + i, 1);
            }
        }
        PlayerPrefs.Save();
    }

    public bool[] LoadCore(string key, int length)
    {
        bool[] BoolArray = new bool[length];

        for (int i = 0; i < length; i++)
        {
            if (PlayerPrefs.HasKey(key + i))
            {
                int value = PlayerPrefs.GetInt(key + i);
                BoolArray[i] = value == 1;
            }
        }
        
        return BoolArray;
    }

    public void Save(Object data, string key)
    {
        var jsonDate = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(key, jsonDate);
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }

    public void Load(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }  
}
