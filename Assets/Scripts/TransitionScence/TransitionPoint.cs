using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene, DifferentScene
    }

    [Header("Tranition Info")]
    public string sceneName;
    public TransitionType transitionType;
    public TransitionDestination.DestinationTag destinationTag;
    public bool isOnTrigger;

    private void Update()
    {
        if (isOnTrigger)
        {
            FadeOut.instance.TurnOnFadeOut();
        }
        if (FadeOut.instance.canTran)
            StartCoroutine(Static.DelayToInvokeDo(() => { SceneController.Instance.TransitionToDestination(this); FadeOut.instance.canTran = false; }, 3f));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOnTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isOnTrigger = false;
    }  
}
