using System;
using System.Collections.Generic;
using UnityEngine;

public enum UIState
{
    Inactive,
    ArtifactInfo
}

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private UIPanel artifactInfoPanel;

    public static UIState CurrentState { get; private set; }
    public static event Action<UIState> OnUIStateChange;
    private static Dictionary<UIState, UIPanel> uiPanels = new Dictionary<UIState, UIPanel>();

    private void Start()
    {
        InitializeUIPanelsDictionary();
    }

    private void InitializeUIPanelsDictionary()
    {
        uiPanels.Add(UIState.Inactive, null);
        uiPanels.Add(UIState.ArtifactInfo, artifactInfoPanel);
    }

    public static void ChangeUIState(InteractionArgs args)
    {
        if (CurrentState == args.UIState)
            return;

        var currentPanel = uiPanels[CurrentState];
        if (currentPanel != null)
            currentPanel.gameObject.SetActive(false);

        CurrentState = args.UIState;
        currentPanel = uiPanels[CurrentState];
        if (currentPanel != null)
        {
            currentPanel.gameObject.SetActive(true);
            currentPanel.Initialize(args.ArtifactInfo);
        }

        OnUIStateChange?.Invoke(CurrentState);
    }
}
