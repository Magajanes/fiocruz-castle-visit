using System;
using UnityEngine;

public enum UIState
{
    Inactive,
    ArtifactInfo
}

public class UIManager : MonoBehaviour
{
    public static UIState CurrentState { get; private set; }
    public static event Action<UIState> OnUIStateChange;

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

        SetUIState(UIState.ArtifactInfo);
        Debug.LogFormat("Interacted with {0}", info.Name);
    }

    public void CloseArtifactInfoScreen()
    {
        SetUIState(UIState.Inactive);
        Debug.Log("Closed artifact info screen.");
    }

    private void SetUIState(UIState state)
    {
        var previousState = CurrentState;
        CurrentState = state;

        if (CurrentState != previousState)
            OnUIStateChange?.Invoke(CurrentState);
    }
}
