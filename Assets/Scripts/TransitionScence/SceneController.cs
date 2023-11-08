using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController :Singleton<SceneController>
{
    public GameObject playerPrefab;

    GameObject player;
    public CharacterController coll;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
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
        //�O�s�ƾ�
        SaveManager.Instance.SavePlayerData();

        if (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            //Ū���ƾ�
            SaveManager.Instance.LoadPlayerData();
            Debug.Log("test1");
            yield break;
        }
        else
        {
            player = GameManager.Instance.playerStats.gameObject;
            coll = player.GetComponent<CharacterController>();
            coll.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            coll.enabled = true;
            Debug.Log("test");
            yield return null;
        }
    }

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
        Debug.Log("" + SaveManager.Instance.SceneName);
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
            yield return player = Instantiate(playerPrefab, GameManager.instance.GetEntrance().position, GameManager.instance.GetEntrance().rotation);

            // �ƾګO�s
            SaveManager.Instance.SavePlayerData();

            Debug.Log("" + scene);
            yield break;
        }
    }

    IEnumerator LoadMain()
    {
        yield return SceneManager.LoadSceneAsync("MainMenu");
        yield break;
    }
}
