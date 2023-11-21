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
    public Vector3 playerPosition;  // 到場景二時 存生成點
    public CharacterController characterController;

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

    //存儲和加載玩家血量與場景名稱
    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStats.characterData, GameManager.instance.playerStats.characterData.name);
    }
    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStats.characterData, GameManager.instance.playerStats.characterData.name);
    }    

    public void Save(Object data, string key)
    {
        // 將數據轉換為 JSON 格式並儲存到 PlayerPrefs
        var jsonDate = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(key, jsonDate);
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }
    public void Load(Object data, string key)
    {
        // 如果在 PlayerPrefs 中有對應的值，則從 JSON 格式轉換回原始數據
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }


    //存儲和加載玩家位置
    public void SavePlayerPositionData()
    {
        //SavePlayerPosition(GameManager.instance.playerStats.transform.position);
        SavePlayerPosition(SceneController.Instance.player.transform.position);
    }
    public void LoadPlayerPositionData()
    {
        if (SceneController.Instance.player != null)
        {
            Vector3 playerPosition = LoadPlayerPosition();
            Debug.Log("Loaded Player Position: " + playerPosition);
            characterController = SceneController.Instance.player.GetComponent<CharacterController>();

            if (characterController != null)
            {
                characterController.enabled = false;
                characterController.transform.position = playerPosition;
                characterController.enabled = true;
            }
        }
    }

    public void SavePlayerPosition(Vector3 position)
    {
        // 將玩家位置儲存到變數和 PlayerPrefs
        playerPosition = position;
        PlayerPrefs.SetFloat("PlayerPosX", position.x);
        PlayerPrefs.SetFloat("PlayerPosY", position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", position.z);

        PlayerPrefs.Save();
    }
    public Vector3 LoadPlayerPosition()
    {
        // 從 PlayerPrefs 獲取玩家位置並返回
        float x = PlayerPrefs.GetFloat("PlayerPosX");
        float y = PlayerPrefs.GetFloat("PlayerPosY");
        float z = PlayerPrefs.GetFloat("PlayerPosZ");
        return new Vector3(x, y, z);
    }


    //存儲和加載背包核心數據
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
        //如果元素為 true，則在 PlayerPrefs 中設置對應的值為 1
        for (int i = 0; i < BoolArray.Length; i++)
        {
            if (BoolArray[i] == true)
            {
                PlayerPrefs.SetInt(key + i, 1);
            }
        }
        PlayerPrefs.Save();
    }
    public bool[] LoadCore(string key, int length)
    {
        bool[] BoolArray = new bool[length];

        //如果在 PlayerPrefs 中有對應的值，則設置Bool
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
}
