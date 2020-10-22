using System;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    [SerializeField]
    private ArtifactInfo info;

    public static event Action<ArtifactInfo> OnInteraction;

    private void OnTriggerEnter(Collider other)
    {
        InputController.OnInteractionButtonPress += SendInfo;
        Debug.LogFormat("Player can now interact with {0}.", info.Name);
    }

    private void OnTriggerExit(Collider other)
    {
        InputController.OnInteractionButtonPress -= SendInfo;
        Debug.LogFormat("Player can no longer interact with {0}.", info.Name);
    }

    private void OnDisable()
    {
        InputController.OnInteractionButtonPress -= SendInfo;
    }

    private void OnDestroy()
    {
        InputController.OnInteractionButtonPress -= SendInfo;
    }

    private void SendInfo()
    {
        OnInteraction?.Invoke(info);
    }
}
