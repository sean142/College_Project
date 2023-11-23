using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    string sceneName = "level";
    string coreBoolKey = "coreBoolKey";
    string coreInSceneKey = "coreInSceneKey";
    public string SceneName { get { return PlayerPrefs.GetString(sceneName); } }
    public string CoreBoolKey { get { return PlayerPrefs.GetString(coreBoolKey); } }
    public string CoreInSceneKey { get { return PlayerPrefs.GetString(coreInSceneKey); } }
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
            SaveCoreBugData();
            SavePlayerPositionData();
            SaveCoreInSceneData();
            SaveEnemyStateData();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadPlayerData();
            LoadCoreBugData();
            LoadPlayerPositionData();
            LoadCoreInSceneData();
            LoadEnemyStateData();
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
    public void SaveCoreBugData()
    {
        SaveCoreBug(CoreInventory.Instance.coreBool, coreBoolKey);
    }
    public void LoadCoreBugData()
    {
        CoreInventory.Instance.coreBool = LoadCoreBug(coreBoolKey, CoreInventory.Instance.coreBool.Length);
    }

    public void SaveCoreBug(bool[] BoolArray, string key)
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
    public bool[] LoadCoreBug(string key, int length)
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
   
    //存儲場上中掉落那個核心
    public void SaveCoreInSceneData()
    {
        SaveCoreInScene(CoreManager.Instance.corePool, coreInSceneKey);
    }
    public void LoadCoreInSceneData()
    {
        bool[] activeCores = LoadCoreInScene(coreInSceneKey, CoreManager.Instance.corePool.Length);

        for (int i = 0; i < activeCores.Length; i++)
        {
            CoreManager.Instance.corePool[i].isActive = activeCores[i];
            CoreManager.Instance.corePool[i].TurnOn();
        }
    }

    public void SaveCoreInScene(CoreItemOnWorld[] coreArray, string key)
    {
        for (int i = 0; i < coreArray.Length; i++)
        {
            if (coreArray[i].isActive)
            {
                PlayerPrefs.SetInt(key + i, 1);
                PlayerPrefs.SetFloat(key + i + "x", coreArray[i].transform.position.x);
                PlayerPrefs.SetFloat(key + i + "y", coreArray[i].transform.position.y);
                PlayerPrefs.SetFloat(key + i + "z", coreArray[i].transform.position.z);
                Debug.Log(coreArray[i]);
            }
        }
        PlayerPrefs.Save();
    }
    public bool [] LoadCoreInScene(string key, int length)
    {
        bool[] BoolArray = new bool[length];
        Vector3[] positions = new Vector3[length];

        //如果在 PlayerPrefs 中有對應的值，則設置Bool和位置
        for (int i = 0; i < length; i++)
        {
            if (PlayerPrefs.HasKey(key + i))
            {
                int value = PlayerPrefs.GetInt(key + i);
                BoolArray[i] = value == 1;
                positions[i] = new Vector3(PlayerPrefs.GetFloat(key + i + "x"), PlayerPrefs.GetFloat(key + i + "y"), PlayerPrefs.GetFloat(key + i + "z"));
            }
        }

        //在這裡使用位置數組來實例化和放置核心物件

        return BoolArray;
    }

    //敵人狀態
    public void SaveEnemyStateData()
    {
        for (int i = 0; i < Enemy.instance.enemies.Length; i++)
        {
            SaveEnemyState(Enemy.instance.enemies[i].isDead, "isDeadKey" + i);
        }       
    }

    public void LoadEnemyStateData()
    {
        for (int i = 0; i < Enemy.instance.enemies.Length; i++)
        {
            Enemy.instance.enemies[i].isDead = LoadEnemyState("isDeadKey" + i);
        }
    }
   
    public void SaveEnemyState(bool Bool,string key)
    {
        PlayerPrefs.SetInt(key, Bool ? 1 : 0);
        PlayerPrefs.Save();

        // 檢查保存是否成功
        if (PlayerPrefs.HasKey(key))
        {
            Debug.Log("Saved " + key + " successfully!");
        }
        else
        {
            Debug.LogWarning("Failed to save " + key + "!");
        }
    }

    public bool LoadEnemyState(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            // 如果PlayerPrefs中存在該鍵，則返回其值，否則返回false
            return PlayerPrefs.GetInt(key) == 1;
        }
        else
        {
            // 如果PlayerPrefs中不存在該鍵，則返回預設值
            return false;
        }
    }

    //敵人狀態
    //public void SaveEnemyStateData()
    //{
    //    SaveEnemyState("EnemyID", EnemyController.instance.currentInt);
    //    SaveEnemyState("isDead", EnemyController.instance.isDead ? 1 : 0);
    //}

    //public void LoadEnemyStateData()
    //{
    //    // 從PlayerPrefs中讀取敵人的ID和死亡狀態
    //    int enemyID = LoadEnemyState("EnemyID");
    //    int isDead = LoadEnemyState("isDead");

    //    // 使用讀取的數據來恢復敵人的狀態
    //    EnemyController.instance.currentInt = enemyID;
    //    EnemyController.instance.isDead = isDead == 1;
    //}

    //public void SaveEnemyState(string key, int value)
    //{
    //    PlayerPrefs.SetInt(key, value);

    //    PlayerPrefs.Save();
    //}   

    //public int LoadEnemyState(string key)
    //{
    //    if (PlayerPrefs.HasKey(key))
    //    {
    //        // 如果PlayerPrefs中存在該鍵，則返回其值，否則返回0
    //        return PlayerPrefs.GetInt(key);

    //    }
    //    return 0;
    //}
}
