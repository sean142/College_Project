using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject video;
 
    void Start()
    {
        videoPlayer.loopPointReached += EndReached;
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        Debug.Log("Video Finished");
        video.gameObject.SetActive(false);
        //videoPlayer.gameObject.SetActive(false);
    }

}
