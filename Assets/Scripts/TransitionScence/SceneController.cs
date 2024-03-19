using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController :Singleton<SceneController>
{
    public GameObject playerPrefab;
    public GameObject player;
    CharacterController coll;

    [Header("Bool")]
    public bool isFirstTimeInGame;
    public bool isTransitioning; 
    public bool isStandingUp; // 用來判斷玩家是否standUp 因為只會觸發一次  有跨場景需要所以一直保持true
    public bool outDoor;      // 判斷玩家在戶外還是室內 因為fog

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void Update()
    {
        CheckOutdoorOrIndoor();
    }

    public void TransitionToDestination(TransitionPoint transitionPoint)
    {

        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                StartCoroutine(Transiton(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                StartCoroutine(Transiton(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        }
    }
    
    IEnumerator Transiton(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        //保存數據
        SaveManager.Instance.SavePlayerData();
        SaveManager.Instance.SaveCoreBugData();
        SaveManager.Instance.SavePlayerPositionData();
        SaveManager.Instance.SaveCoreInSceneData();
        //SaveManager.Instance.SaveEnemyData();
        SaveManager.Instance.SaveEnemyStateData();
        SaveManager.Instance.SaveEnemyStateAndCoreStateData();

        //用TransitionPoint 中的 sceneName 來判斷是否為同場景。如果不相同，則表示目標轉換點位於不同的場景
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            player = PlayerController.instance.gameObject;
            PlayerController.instance.canMove = true;

            SaveManager.Instance.LoadPlayerData();
            SaveManager.Instance.LoadCoreBugData();

            SaveManager.Instance.SavePlayerData();
            SaveManager.Instance.SaveCoreBugData();
            SaveManager.Instance.SavePlayerPositionData();
            SaveManager.Instance.SaveCoreInSceneData();
            //SaveManager.Instance.SaveEnemyData();
            SaveManager.Instance.SaveEnemyStateData();
            SaveManager.Instance.SaveMinimapPositionData();

            isTransitioning = false;
            yield break;
        }
        else
        {
            player = GameManager.Instance.playerStats.gameObject;
            coll = player.GetComponent<CharacterController>();
            coll.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            coll.enabled = true;
            yield return null;
        }
    }
    //尋找目標點
    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();

        for (int i = 0; i < entrances.Length; i++)
        {
            if (entrances[i].destinationTag == destinationTag)
                return entrances[i];
        }

        return null;
    }
    
    public void TransitionToMain()
    {
        StartCoroutine(LoadMain());
    }

    public void TransitionToLoadGame()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }

    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("Office"));
    }

    public IEnumerator LoadLevel(string scene)
    {
        if (scene != "")
        {
            yield return SceneManager.LoadSceneAsync(scene);
            yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);
            // 數據保存
            SaveManager.Instance.SavePlayerData();
            SaveManager.Instance.SaveCoreBugData();
            
            if(isFirstTimeInGame)
            {
                SaveManager.Instance.SavePlayerPositionData();
                SaveManager.Instance.SaveMinimapPositionData();

                isFirstTimeInGame = false;
            }
            if (isStandingUp)
                PlayerController.instance.canMove = true;
            yield break;
        }
    }

    IEnumerator LoadMain()
    {
        yield return SceneManager.LoadSceneAsync("MainMenu");
        yield break;
    }

    private void CheckOutdoorOrIndoor()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.Contains("City") || sceneName.Contains("Sewer"))
        {
            outDoor = true;
        }
        else
            outDoor = false;
    }
}
