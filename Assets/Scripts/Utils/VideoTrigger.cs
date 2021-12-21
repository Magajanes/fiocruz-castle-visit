using UnityEngine;
using UnityEngine.Video;

public class VideoTrigger : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer _videoPlayer;

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
        _videoPlayer.Play();
        _videoPlayer.isLooping = true;
        InputController.OnInteractionButtonPress -= StartVideo;
    }
}
