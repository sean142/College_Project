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
    public bool canTrans;
  
    private void Update()
    {
        if (canTrans)
        {
            FadeOut.instance.TurnOnFadeOut();
            PlayerController.instance.animator.SetBool("Move", false);
            StartCoroutine(Static.DelayToInvokeDo(() =>
            {
                if (!SceneController.Instance.isTransitioning)
                    SceneController.Instance.TransitionToDestination(this);
                SceneController.Instance.isTransitioning = true;
            }, 5f));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTrans = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            canTrans = false;
    }  
}
