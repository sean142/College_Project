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
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadPlayerData();
            LoadCoreData();
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
