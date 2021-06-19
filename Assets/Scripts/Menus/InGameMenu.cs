using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public const float IN_GAME_MENU_FADE_RATE = 2;
    
    [SerializeField]
    private UIFader _uiFader;
    [SerializeField]
    private Button[] _menuButtons;

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

    private void OpenMenu()
    {
        InputController.Instance.LockInputs(true);
        _uiFader.FadeIn(
            gameObject,
            SetInGameMenuInputScheme,
            IN_GAME_MENU_FADE_RATE
        );

        void SetInGameMenuInputScheme()
        {
            InputController.Instance.SetInputScheme(UIState.InGameMenu);
            InputController.Instance.LockInputs(false);
            SetMenuButtonsInteractive(true);
        }
    }

    public void ResumeGame()
    {
        SetMenuButtonsInteractive(false);
        InputController.Instance.LockInputs(true);
        _uiFader.FadeOut(
            gameObject,
            SetInactiveInputScheme,
            IN_GAME_MENU_FADE_RATE
        );

        void SetInactiveInputScheme()
        {
            InputController.Instance.SetInputScheme(UIState.Inactive);
            InputController.Instance.LockInputs(false);
        }
    }
}
