﻿using System;
using System.Collections.Generic;
using UnityEngine;

public enum UIState
{
    Inactive,
    ArtifactInfo,
    InventoryPanel
}

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private ArtifactInfoPanel _artifactInfoPanel;
    [SerializeField]
    private InventoryPanel _inventoryPanel;

    public static UIState CurrentState { get; private set; }
    private static Dictionary<UIState, UIPanel> uiPanels = new Dictionary<UIState, UIPanel>();
    public static event Action<UIState> OnUIStateChange;

    public void Initialize(ArtifactsService artifactsService, Inventory inventory)
    {
        InitializePanels(artifactsService, inventory);
        InitializeUIPanelsDictionary();
    }

    private void InitializeUIPanelsDictionary()
    {
        uiPanels.Add(UIState.Inactive, null);
        uiPanels.Add(UIState.ArtifactInfo, _artifactInfoPanel);
        uiPanels.Add(UIState.InventoryPanel, _inventoryPanel);
    }

    private void InitializePanels(ArtifactsService artifactsService, Inventory inventory)
    {
        _artifactInfoPanel.Initialize(artifactsService);
        _inventoryPanel.Initialize(artifactsService, inventory);
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
            currentPanel.Show(args.ArtifactId);
        }

        OnUIStateChange?.Invoke(CurrentState);
    }
}
