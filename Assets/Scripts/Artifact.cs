using UnityEngine;

public class Artifact : MonoBehaviour
{
    [SerializeField]
    private int _artifactId;

    private void OnTriggerEnter(Collider other)
    {
        InputController.OnInteractionButtonPress += ShowInfo;
    }

    private void OnTriggerExit(Collider other)
    {
        InputController.OnInteractionButtonPress -= ShowInfo;
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
        var args = new InteractionArgs(UIState.ArtifactInfo, _artifactId);
        UIManager.ChangeUIState(args);
    }
}

public class InteractionArgs
{
    public readonly UIState UIState;
    public readonly int ArtifactId;

    public InteractionArgs(UIState uiState, int artifactId = 0)
    {
        UIState = uiState;
        ArtifactId = artifactId;
    }
}
