using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject video;
    public GameObject UI;   // 一開始城市場景黑畫面 當影片一播放就關閉
    void Start() 
    {
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.started += OnVideoStarted;
    }

    private void OnVideoStarted(VideoPlayer source)
    {
        Debug.Log("Video start");
        UI.SetActive(false);
    }

    void EndReached(VideoPlayer vp)
    {
        Debug.Log("Video Finished");
        video.gameObject.SetActive(false);
    }

}
