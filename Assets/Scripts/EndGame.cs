using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FadeOut.instance.TurnOnFadeOut();
            StartCoroutine(Static.DelayToInvokeDo(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }, 5f));
        }
    }
}
