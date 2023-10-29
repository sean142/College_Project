using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Static : MonoBehaviour
{
    // 核心生成延遲
    public static IEnumerator DelayToTurnCore(System.Action action, float delaySeconds)
    {
        CoreManager.instance.isCoreGenerating = true;
        yield return new WaitForSeconds(delaySeconds);
        //yield return new WaitForSecondsRealtime(delaySeconds);
        CoreManager.instance.isCoreGenerating = false;
        action();        
    } 
    
    public static IEnumerator DelayToInvokeDo(System.Action action, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        //yield return new WaitForSecondsRealtime(delaySeconds);
        action();        
    }
}


