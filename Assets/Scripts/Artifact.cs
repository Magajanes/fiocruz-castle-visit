using UnityEngine;

public class Artifact : MonoBehaviour
{
    [SerializeField]
    private ArtifactInfo _info;

    private void OnTriggerEnter(Collider other)
    {
        InputController.OnInteractionButtonPress += ShowInfo;
        Debug.Log($"Player can now interact with {_info.Name}.");
    }

    private void OnTriggerExit(Collider other)
    {
        InputController.OnInteractionButtonPress -= ShowInfo;
        Debug.Log($"Player can no longer interact with {_info.Name}.");
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
        var args = new InteractionArgs(UIState.ArtifactInfo, _info);
        UIManager.ChangeUIState(args);
    }
}

public class InteractionArgs
{
    public readonly UIState UIState;
    public readonly object Context;

    public InteractionArgs(UIState uiState, object context = null)
    {
        UIState = uiState;
        Context = context;
    }
}
