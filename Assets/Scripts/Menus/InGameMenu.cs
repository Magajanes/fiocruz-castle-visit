using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public const float IN_GAME_MENU_FADE_RATE = 2;

    [Header("Menu objects")]
    [SerializeField]
    private GameObject _inGameMenuPanel;
    [SerializeField]
    private GameObject _inGameOptionsPanel;

    [Header("References")]
    [SerializeField]
    private UIFader _uiFader;
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
        SetMenuButtonsInteractive(true);
    }

    private void SetInactiveInputScheme()
    {
        InputController.Instance.SetInputScheme(UIState.Inactive);
        _currentPanel = null;
    }

    private void CloseCurrentPanel()
    {
        SetMenuButtonsInteractive(true);
        _currentPanel.SetActive(false);
        _currentPanel = _inGameMenuPanel;
        _isFading = false;
    }

    private void OpenMenu()
    {
        _currentPanel = _inGameMenuPanel;
        SetInGameMenuInputScheme();
        _uiFader.FadeIn(
            _inGameMenuPanel,
            null,
            IN_GAME_MENU_FADE_RATE
        );
    }

    public void ResumeGame()
    {
        SetMenuButtonsInteractive(false);
        _uiFader.FadeOut(
            _inGameMenuPanel,
            SetInactiveInputScheme,
            IN_GAME_MENU_FADE_RATE
        );
    }

    public void ShowOptionsPanel()
    {
        PlayerPrefsService.ApplySavedPlayerPrefs(out bool invertY);
        _invertYToggle.isOn = invertY;

        _inGameOptionsPanel.SetActive(true);
        _currentPanel = _inGameOptionsPanel;

        _uiFader.FadeOut(_inGameMenuPanel);
        SetMenuButtonsInteractive(false);

        _uiFader.FadeIn(
            _inGameOptionsPanel,
            null,
            IN_GAME_MENU_FADE_RATE
        );
    }

    public void BackToMenu()
    {
        if (_isFading)
            return;

        _isFading = true;
        _uiFader.FadeIn(_inGameMenuPanel);
        _uiFader.FadeOut(
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
