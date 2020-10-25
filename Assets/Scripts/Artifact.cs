using System;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    [SerializeField]
    private ArtifactInfo info;

    private void OnTriggerEnter(Collider other)
    {
        InputController.OnInteractionButtonPress += ShowInfo;
        Debug.LogFormat("Player can now interact with {0}.", info.Name);
    }

    private void OnTriggerExit(Collider other)
    {
        InputController.OnInteractionButtonPress -= ShowInfo;
        Debug.LogFormat("Player can no longer interact with {0}.", info.Name);
    }

    private void OnDisable()
    {
        InputController.OnInteractionButtonPress -= ShowInfo;
    }

    private void OnDestroy()
    {
        InputController.OnInteractionButtonPress -= ShowInfo;
    }

    private void ShowInfo()
    {
        var args = new InteractionArgs(UIState.ArtifactInfo, info);
        UIManager.ChangeUIState(args);
    }
}

public class InteractionArgs
{
    public readonly UIState UIState;
    public readonly ArtifactInfo ArtifactInfo;

    public InteractionArgs(UIState uiState, ArtifactInfo artifactInfo)
    {
        UIState = uiState;
        ArtifactInfo = artifactInfo;
    }
}
