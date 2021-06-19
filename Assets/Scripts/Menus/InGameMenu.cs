using System;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [Header("Menu objects")]
    [SerializeField]
    private GameObject _inGameMenuPanel;
    [SerializeField]
    private OptionsMenu _inGameOptionsPanel;
    [SerializeField]
    private InventoryPanel _indexPanel;
    [SerializeField]
    private ArtifactInfoPanel _artifactInfoPanel;

    [Header("References")]
    [SerializeField]
    private Toggle _invertYToggle;
    [SerializeField]
    private Button[] _menuButtons;

    private bool _isFading;
    private bool _isImediateIndex;
    private GameObject _currentPanel;

    public delegate void BackButtonAction();
    public BackButtonAction OnBackButtonDelegate;

    private void Awake()
    {
        InputController.OnInGameMenuOpen += OpenMenu;
        InputController.OnInventoryButtonPress += OpenInventoryImediate;
        InputController.OnBackButtonPress += OnBackButtonClick;
        Artifact.OnInteraction += OpenArtifactInfoImediate;
    }

    private void Start()
    {
        SetMenuButtonsInteractive(false);
    }

    private void OnDestroy()
    {
        InputController.OnInGameMenuOpen -= OpenMenu;
        InputController.OnInventoryButtonPress -= OpenInventoryImediate;
        InputController.OnBackButtonPress -= OnBackButtonClick;
        Artifact.OnInteraction -= OpenArtifactInfoImediate;
    }

    private void SetMenuButtonsInteractive(bool interactive)
    {
        foreach (Button button in _menuButtons)
        {
            button.interactable = interactive;
        }
    }

    private void SetInGameMenuInputScheme()
    {
        InputController.Instance.SetInputScheme(UIState.InGameMenu);
    }

    private void SetIndexInputScheme()
    {
        InputController.Instance.SetInputScheme(UIState.InventoryPanel);
    }

    private void SetInactiveInputScheme()
    {
        InputController.Instance.SetInputScheme(UIState.Inactive);
    }

    private void CloseCurrentPanel()
    {
        if (_currentPanel == null) return;
        
        SetMenuButtonsInteractive(true);
        _currentPanel.SetActive(false);
        _currentPanel = null;
        _isFading = false;
    }

    private void OpenMenu()
    {
        SetInGameMenuInputScheme();
        UIFader.FadeIn(
            _inGameMenuPanel,
            () => SetMenuButtonsInteractive(true)
        );
    }

    public void ResumeGame()
    {
        SetMenuButtonsInteractive(false);
        OnBackButtonDelegate = null;

        UIFader.FadeOut(
            _inGameMenuPanel,
            SetInactiveInputScheme
        );
    }

    public void ShowOptionsPanel()
    {
        if (_isFading) return;

        _isFading = true;
        _inGameOptionsPanel.gameObject.SetActive(true);
        _inGameOptionsPanel.Initialize();
        _currentPanel = _inGameOptionsPanel.gameObject;

        UIFader.FadeOut(_inGameMenuPanel);
        SetMenuButtonsInteractive(false);

        UIFader.FadeIn(
            _inGameOptionsPanel.gameObject,
            () => _isFading = false
        );
    }

    public void ShowInventory()
    {
        if (_isFading) return;

        _isFading = true;
        _indexPanel.gameObject.SetActive(true);
        _indexPanel.Initialize();
        _currentPanel = _indexPanel.gameObject;
        OnBackButtonDelegate = BackToMenu;

        UIFader.FadeOut(_inGameMenuPanel);
        SetMenuButtonsInteractive(false);

        UIFader.FadeIn(
            _indexPanel.gameObject,
            () => _isFading = false
        );
    }

    private void OpenInventoryImediate()
    {
        if (_isFading) return;

        _isImediateIndex = true;
        SetIndexInputScheme();
        OnBackButtonDelegate = CloseInventoryImediate;

        _isFading = true;
        _indexPanel.gameObject.SetActive(true);
        _indexPanel.Initialize();

        UIFader.FadeIn(
            _indexPanel.gameObject,
            () => _isFading = false
        );
    }

    private void CloseInventoryImediate()
    {
        if (_isFading) return;

        _isFading = true;
        OnBackButtonDelegate = null;

        UIFader.FadeOut(
            _indexPanel.gameObject,
            ReturnToGameFromIndex
        );

        void ReturnToGameFromIndex()
        {
            SetInactiveInputScheme();
            _indexPanel.gameObject.SetActive(false);

            _isImediateIndex = false;
            _isFading = false;
        }
    }

    public void ShowArtifactInfo(int artifactId)
    {
        if (_isFading) return;

        if (!InventoryManager.PlayerInventory.HasArtifact(artifactId))
            return;

        _isFading = true;
        _artifactInfoPanel.gameObject.SetActive(true);
        _artifactInfoPanel.Initialize(artifactId);
        OnBackButtonDelegate = BackToIndex;

        UIFader.FadeOut(_indexPanel.gameObject);
        UIFader.FadeIn(
            _artifactInfoPanel.gameObject,
            CloseIndexPanel
        );

        void CloseIndexPanel()
        {
            _indexPanel.gameObject.SetActive(false);
            _isFading = false;
        }
    }

    private void OpenArtifactInfoImediate(InitArgs args)
    {
        if (_isFading) return;

        SetIndexInputScheme();
        OnBackButtonDelegate = CloseArtifactInfoImediate;

        _isFading = true;
        _artifactInfoPanel.gameObject.SetActive(true);
        _artifactInfoPanel.Initialize(args);

        UIFader.FadeIn(
            _artifactInfoPanel.gameObject,
            () => _isFading = false
        );
    }

    private void CloseArtifactInfoImediate()
    {
        if (_isFading) return;

        _isFading = true;
        OnBackButtonDelegate = null;

        UIFader.FadeOut(
            _artifactInfoPanel.gameObject,
            ReturnToGameFromArtifactInfo
        );

        void ReturnToGameFromArtifactInfo()
        {
            SetInactiveInputScheme();
            _artifactInfoPanel.gameObject.SetActive(false);
            _isFading = false;
        }
    }

    public void BackToIndex()
    {
        if (_isFading) return;

        _isFading = true;
        _indexPanel.gameObject.SetActive(true);
        _indexPanel.Initialize();

        if (_isImediateIndex)
        {
            OnBackButtonDelegate = CloseInventoryImediate;
        }
        else
        {
            OnBackButtonDelegate = BackToMenu;
        }

        UIFader.FadeIn(_indexPanel.gameObject);
        UIFader.FadeOut(
            _artifactInfoPanel.gameObject,
            CloseArtifactInfoPanel
        );

        void CloseArtifactInfoPanel()
        {
            _artifactInfoPanel.gameObject.SetActive(false);
            _isFading = false;
        }
    }

    public void BackToMenu()
    {
        if (_isFading) return;

        _isFading = true;
        OnBackButtonDelegate = null;

        UIFader.FadeIn(_inGameMenuPanel);
        UIFader.FadeOut(
            _currentPanel,
            CloseCurrentPanel
        );
    }

    public void BackToMainMenu()
    {
        SetMenuButtonsInteractive(false);
        ScenesController.Instance.BackToMainMenu();
    }

    public void OnBackButtonClick()
    {
        OnBackButtonDelegate?.Invoke();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
