using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Static : MonoBehaviour
{
    // 延遲
    public static IEnumerator DelayToInvokeDo(System.Action action, float delaySeconds)
    {
        //CoreManager.instance.isCoreGenerating = true;

        yield return new WaitForSeconds(delaySeconds);
        //yield return new WaitForSecondsRealtime(delaySeconds);
        //CoreManager.instance.isCoreGenerating = false;
        action();
        
    }
}


