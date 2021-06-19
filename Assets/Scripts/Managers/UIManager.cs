using System;
using System.Collections.Generic;
using UnityEngine;

public enum UIState
{
    Inactive,
    ArtifactInfo,
    InventoryPanel,
    InGameMenu
}

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private ArtifactInfoPanel _artifactInfoPanel;
    [SerializeField]
    private InventoryPanel _inventoryPanel;

    private static UIState _currentState;
    private static UIPanel _currentPanel;
    public static event Action<UIState> OnUIStateChange;

    private static Dictionary<UIState, UIPanel> _uiPanels = new Dictionary<UIState, UIPanel>();

    public void Initialize()
    {
        InitializePanelsDictionary();
    }

    private void InitializePanelsDictionary()
    {
        _uiPanels[UIState.Inactive] = null;
        _uiPanels[UIState.ArtifactInfo] = _artifactInfoPanel;
        _uiPanels[UIState.InventoryPanel] = _inventoryPanel;
    }

    public static void ChangeState(UIState state, UIPanel.InitArgs args = null)
    {
        if (_currentState == state)
            return;

        _currentState = state;
        _currentPanel?.SetActive(false);
        _currentPanel = _uiPanels[_currentState];
        if (_currentPanel != null)
        {
            _currentPanel.SetActive(true);
            _currentPanel.Initialize(args);
        }

        OnUIStateChange?.Invoke(_currentState);
    }
}
