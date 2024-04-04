using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    string sceneName = "level";
    string coreBoolKey = "coreBoolKey";
    string coreInSceneKey = "coreInSceneKey";
    string enemyState = "enemyState";
    string enemyData = "enemyData";
    public string SceneName { get { return PlayerPrefs.GetString(sceneName); } }
    //public string CoreBoolKey { get { return PlayerPrefs.GetString(coreBoolKey); } }
    //public string CoreInSceneKey { get { return PlayerPrefs.GetString(coreInSceneKey); } }
    //public string EnemyState {get { return PlayerPrefs.GetString(enemyState); } }
    //public string EnemyData {get { return PlayerPrefs.GetString(enemyData); } }

    public Vector3 playerPosition;  
    public CharacterController characterController;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
   
    private void Update()
    {        
        if (Input.GetKeyDown(KeyCode.N))
        {
            SavePlayerData();
            SaveCoreBugData();
            SavePlayerPositionData();
            SaveCoreInSceneData();
            SaveEnemyStateData();
            SaveEnemyData();
            SaveEnemyStateAndCoreStateData();
            SaveMinimapPositionData();
        }

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    LoadPlayerData();
        //    LoadCoreBugData();
        //    LoadPlayerPositionData();
        //    LoadCoreInSceneData();
        //    LoadEnemyStateData();
        //    LoadEnemyData();
        //}
    }

    //存儲和加載玩家血量與場景名稱
    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
    }
    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerStats.characterData, GameManager.Instance.playerStats.characterData.name);
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
 
    //存儲敵人血量
    public void SaveEnemyData()
    {
        for (int i = 0; i < GameManager.Instance.enemyStats.Length; i++)
        {
            SaveEnemy(GameManager.Instance.enemyStats[i].characterData, enemyData + i + SceneManager.GetActiveScene().name);
        }
    }

    public void LoadEnemyData()
    {
        for (int i = 0; i < GameManager.Instance.enemyStats.Length; i++)
        {
            LoadEnemy(GameManager.Instance.enemyStats[i].characterData, enemyData + i + SceneManager.GetActiveScene().name);         
        }
    }

    public void SaveEnemy(Object data, string key)
    {
        // 將數據轉換為 JSON 格式並儲存到 PlayerPrefs
        var jsonData = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.Save();
    }

    public void LoadEnemy(Object data, string key)
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

    // 存儲場上中掉落那個核心
    public void SaveCoreInSceneData()
    {
        for (int i = 0; i < CoreManager.Instance.corePool.Length; i++)
        {
            if (CoreManager.Instance.corePool[i].isActive)
            {
                SaveCoreInScene(CoreManager.Instance.corePool[i], CoreManager.Instance.corePool[i].transform.position, coreInSceneKey + i + SceneManager.GetActiveScene().name);
            }
        }     
    }

    public void LoadCoreInSceneData()
    {
        for (int i = 0; i < CoreManager.Instance.corePool.Length; i++)
        {
            bool isActive = LoadCoreIsActive(coreInSceneKey + i+ SceneManager.GetActiveScene().name);
            CoreManager.Instance.corePool[i].isActive = isActive;
          
            if (isActive)
            {
                Vector3 position = LoadCorePosition(coreInSceneKey + i + SceneManager.GetActiveScene().name);
                CoreManager.Instance.corePool[i].transform.position = position;
                CoreManager.Instance.corePool[i].TurnOn();
                CoreManager.Instance.isCoreTurnOn = true;
            }
         
        }

        for (int i = 0; i < CoreInventory.Instance.coreBool.Length; i++)
        {
            if (CoreInventory.Instance.coreBool[i] == true)
            {
                Debug.LogError("核心關閉");
                CoreManager.Instance.corePool[i].TurnOff();
                CoreManager.Instance.isCoreTurnOn = false;
            }
        }

      
    }

    public void SaveCoreInScene(CoreItemOnWorld coreItem, Vector3 position, string key)
    {
        if (coreItem.isActive)
        {
            PlayerPrefs.SetInt(key, 1);
            PlayerPrefs.SetFloat(key + "x", position.x);
            PlayerPrefs.SetFloat(key + "y", position.y);
            PlayerPrefs.SetFloat(key + "z", position.z);
            Debug.Log(coreItem);
        }
        PlayerPrefs.Save();
    }

    public bool LoadCoreIsActive(string key)
    {
        return PlayerPrefs.GetInt(key) == 1;
    }

    public Vector3 LoadCorePosition(string key)
    {
        float x = PlayerPrefs.GetFloat(key + "x");
        float y = PlayerPrefs.GetFloat(key + "y");
        float z = PlayerPrefs.GetFloat(key + "z");

        return new Vector3(x, y, z);
    }

    //敵人狀態
    public void SaveEnemyStateData()
    {
        for (int i = 0; i < EnemyManager.instance.enemiesPool.Length; i++)
        {
            SaveEnemyState(EnemyManager.instance.enemiesPool[i].isDead, enemyState + i + SceneManager.GetActiveScene().name);
        }       
    }

    public void LoadEnemyStateData()
    {
        for (int i = 0; i < EnemyManager.instance.enemiesPool.Length; i++)
        {
            EnemyManager.instance.enemiesPool[i].isDead = LoadEnemyState(enemyState + i + SceneManager.GetActiveScene().name);          
        }
    }
   
    public void SaveEnemyState(bool Bool,string key)
    {
        PlayerPrefs.SetInt(key, Bool ? 1 : 0);
        PlayerPrefs.Save();

        //// 檢查保存是否成功
        //if (PlayerPrefs.HasKey(key))
        //{
        //    Debug.Log("Saved " + key + " successfully!");
        //}
        //else
        //{
        //    Debug.LogWarning("Failed to save " + key + "!");
        //}
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
    //當敵人已死亡、核心未生成的狀態就存檔 需要強制生成核心到敵人死亡位置
    public void SaveEnemyStateAndCoreStateData()
    {       
        for (int i = 0; i < EnemyManager.instance.enemiesPool.Length; i++)
        {
            if (EnemyManager.instance.enemiesPool[i]!= null && EnemyManager.instance.enemiesPool[i].isDead && !CoreManager.Instance.corePool[i].isActive)
            {
                PlayerPrefs.SetInt("enemyState" + i, 1);
                PlayerPrefs.SetInt("coreState" + i, 0);
                SaveEnemyPosition(EnemyManager.instance.enemiesPool[i].transform.position);
                SaveEnemyInt(EnemyManager.instance.enemiesPool[i].currentInt);
            }
        }       
    }
    public void LoadEnemyStateAndCoreStateData()
    {
        Vector3 enemyPosition = LoadEnemyPosition();
        int enemyIndex = LoadEnemyInt();
        Debug.Log("Enemy" + enemyIndex);
        for (int i = 0; i < EnemyManager.instance.enemiesPool.Length; i++)
        {
            if (PlayerPrefs.GetInt("enemyState" + i) == 1)
            {    
                CoreManager.Instance.corePool[enemyIndex].transform.position = enemyPosition;
                CoreManager.Instance.isCoreTurnOn = true;
                CoreManager.Instance.corePool[enemyIndex].TurnOn();
            }
        }

        for (int i = 0; i < CoreInventory.Instance.coreBool.Length; i++)
        {
            if (CoreInventory.Instance.coreBool[i] == true)
            {
                Debug.LogError("核心關閉");
                CoreManager.Instance.corePool[i].TurnOff();
                CoreManager.Instance.isCoreTurnOn = false;
            }
        }

        for (int i = 0; i < CoreManager.Instance.corePool.Length; i++)
        {
            if (CoreManager.Instance.corePool[i].isActive)
            {
                CoreManager.Instance.isCoreTurnOn = true;
            }
        }
    }

    public void SaveEnemyPosition(Vector3 position)
    {
        // 將敵人位置儲存到變數和 PlayerPrefs
        PlayerPrefs.SetFloat("EnemyPosX", position.x);
        PlayerPrefs.SetFloat("EnemyPosY", position.y + 0.5f);
        PlayerPrefs.SetFloat("EnemyPosZ", position.z);

        PlayerPrefs.Save();
    }
    public Vector3 LoadEnemyPosition()
    {
        // 從 PlayerPrefs 獲取敵人位置並返回
        float x = PlayerPrefs.GetFloat("EnemyPosX");
        float y = PlayerPrefs.GetFloat("EnemyPosY");
        float z = PlayerPrefs.GetFloat("EnemyPosZ");
        return new Vector3(x, y, z);
    }

    public void SaveEnemyInt(int index)
    {
        // 拿到當前死亡敵人的CurrnetIndex
        PlayerPrefs.SetInt("EnemyIndex", index);
    }

    public int LoadEnemyInt()
    {
        return PlayerPrefs.GetInt("EnemyIndex");
    }

    public void SaveMinimapPositionData()
    {
        SaveMinimapPosition(Minimap.instance.target.transform.position);
    }

    public void LoadMinimapPositionData()
    {
        Vector3 minimapPosition = LoadMiniPosition();
        Debug.Log("MinimapPosition" + minimapPosition);
        Minimap.instance.transform.position = minimapPosition;
    }

    public void SaveMinimapPosition(Vector3 position)
    {
        PlayerPrefs.SetFloat("MinimapPosX", position.x);
        PlayerPrefs.SetFloat("MinimapPosY", position.y);
        PlayerPrefs.SetFloat("MinimapPosZ", position.z);

        PlayerPrefs.Save();
    }
    public Vector3 LoadMiniPosition()
    {
        float x = PlayerPrefs.GetFloat("MinimapPosX");
        float y = PlayerPrefs.GetFloat("MinimapPosY");
        float z = PlayerPrefs.GetFloat("MinimapPosZ");
        return new Vector3(x, y, z);
    }
    //可以參考
    //public void SaveEnemyState()
    //{
    //    for (int i = 0; i < EnemyManager.instance.enemiesBool.Length; i++)
    //    {
    //        //SaveEnemyState(EnemyManager.instance.enemiesBool[i], enemy + i);
    //        PlayerPrefs.SetInt(enemy + i, EnemyManager.instance.enemiesBool[i] ? 1 : 0);

    //    }
    //}

    //public void LoadEnemyState()
    //{
    //    for (int i = 0; i < EnemyManager.instance.enemiesBool.Length; i++)
    //    {
    //        //EnemyManager.instance.enemiesBool[i] = LoadEnemyState(enemy + i);
    //        EnemyManager.instance.enemiesBool[i] = PlayerPrefs.GetInt(enemy + i) == 1;
    //        //EnemyManager.instance.enemiesBool[0] = true;
    //        //EnemyManager.instance.enemiesBool[2] = true;
    //    }
    //}
}
