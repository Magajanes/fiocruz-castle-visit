using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum UIState
    {
        Inactive,
        ArtifactInfo
    }

    public static UIState CurrentState;

    private void Awake()
    {
        Artifact.OnInteraction += ShowArtifactInfo;
        InputController.OnCloseButtonPress += CloseArtifactInfoScreen;
    }

    private void OnDestroy()
    {
        Artifact.OnInteraction -= ShowArtifactInfo;
        InputController.OnCloseButtonPress -= CloseArtifactInfoScreen;
    }

    private void OnDisable()
    {
        Artifact.OnInteraction -= ShowArtifactInfo;
        InputController.OnCloseButtonPress -= CloseArtifactInfoScreen;
    }

    private void ShowArtifactInfo(ArtifactInfo info)
    {
        if (CurrentState == UIState.ArtifactInfo)
            return;

        CurrentState = UIState.ArtifactInfo;
        Debug.LogFormat("Interacted with {0}", info.Name);
    }

    public void CloseArtifactInfoScreen()
    {
        CurrentState = UIState.Inactive;
        Debug.Log("Closed artifact info screen.");
    }
}
