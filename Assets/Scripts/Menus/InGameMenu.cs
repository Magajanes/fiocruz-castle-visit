using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public const float IN_GAME_MENU_FADE_RATE = 1;

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
    private GameObject _currentPanel;

    private void Awake()
    {
        InputController.OnInGameMenuOpen += OpenMenu;
    }

    private void Start()
    {
        SetMenuButtonsInteractive(false);
    }

    private void OnDestroy()
    {
        InputController.OnInGameMenuOpen -= OpenMenu;
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
            () => SetMenuButtonsInteractive(true),
            IN_GAME_MENU_FADE_RATE
        );
    }

    public void ResumeGame()
    {
        SetMenuButtonsInteractive(false);
        UIFader.FadeOut(
            _inGameMenuPanel,
            SetInactiveInputScheme,
            IN_GAME_MENU_FADE_RATE
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
            () => _isFading = false,
            IN_GAME_MENU_FADE_RATE
        );
    }

    public void ShowInventory()
    {
        if (_isFading) return;

        _isFading = true;
        _indexPanel.SetActive(true);
        _indexPanel.Initialize();
        _currentPanel = _indexPanel.gameObject;

        UIFader.FadeOut(_inGameMenuPanel);
        SetMenuButtonsInteractive(false);

        UIFader.FadeIn(
            _indexPanel.gameObject,
            () => _isFading = false,
            IN_GAME_MENU_FADE_RATE
        );
    }

    public void BackToMenu()
    {
        if (_isFading) return;

        _isFading = true;
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

    public void ExitGame()
    {
        Application.Quit();
    }
}
