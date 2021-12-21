using UnityEngine;
using UnityEngine.Video;

public class VideoTrigger : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer _videoPlayer;

    private bool _videoPlaying = false;

    private void OnTriggerEnter(Collider other)
    {
        InputController.OnInteractionButtonPress -= StartVideo;
        InputController.OnInteractionButtonPress += StartVideo;
    }

    private void OnTriggerExit(Collider other)
    {   
        InputController.OnInteractionButtonPress -= StartVideo;
    }

    private void StartVideo()
    {
        InputController.OnInteractionButtonPress -= StartVideo;
        if (_videoPlaying) return;

        _videoPlayer.Play();
        _videoPlayer.isLooping = true;

        _videoPlaying = true;
    }
}
