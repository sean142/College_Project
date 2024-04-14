using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class StartAnimation : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += EndReached;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void EndReached(VideoPlayer vp)
    {
        Debug.Log("Video Finished");
        SceneManager.LoadScene("MainMenu");
    }
}
