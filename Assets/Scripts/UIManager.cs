using System;
using UnityEngine;

public enum UIState
{
    Inactive,
    ArtifactInfo
}

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private ArtifactInfoPanel artifactInfoPanel;

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
        artifactInfoPanel.gameObject.SetActive(true);
        artifactInfoPanel.Initialize(info);
    }

    public void CloseArtifactInfoScreen()
    {
        SetUIState(UIState.Inactive);
        artifactInfoPanel.gameObject.SetActive(false);
    }

    private void SetUIState(UIState state)
    {
        var previousState = CurrentState;
        CurrentState = state;

        if (CurrentState != previousState)
            OnUIStateChange?.Invoke(CurrentState);
    }
}
