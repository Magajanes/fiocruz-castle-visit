using System;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public const float IN_GAME_MENU_FADE_RATE = 1.5f;
    public const string IN_GAME_MENU_SOUNDS_BUNDLE_PATH = "SoundBundles/InGameMenuSounds";
    public const string IN_GAME_AMBIENCE_ID = "in_game_ambience";
    public const string MENU_CLICK_ID = "click";
    public const string MENU_CLICK_BACK_ID = "click_back";
    public const string OPEN_BOOK_ID = "open_book";

    [Header("Menu objects")]
    [SerializeField]
    private GameObject _playerHUD;
    [SerializeField]
    private GameObject _inGameMenuPanel;
    [SerializeField]
    private GameObject _selectionMenu;
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

    private SoundsBundle _soundsBundle;
    private AudioClip _clickSound;
    private AudioClip _clickBackSound;
    private AudioClip _openBookSound;

    public delegate void BackButtonAction();
    public BackButtonAction OnBackButtonDelegate;

    private void Awake()
    {
        AddListeners();
    }

    private void AddListeners()
    {
        RemoveListeners();
        InputController.OnInGameMenuOpen += OpenMenu;
        InputController.OnInventoryButtonPress += OpenInventoryImediate;
        InputController.OnBackButtonPress += OnBackButtonClick;
        Artifact.OnInteraction += OpenArtifactInfoImediate;
    }

    private void RemoveListeners()
    {
        InputController.OnInGameMenuOpen -= OpenMenu;
        InputController.OnInventoryButtonPress -= OpenInventoryImediate;
        InputController.OnBackButtonPress -= OnBackButtonClick;
        Artifact.OnInteraction -= OpenArtifactInfoImediate;
    }

    private void Start()
    {
        SetMenuButtonsInteractive(false);
        SoundsManager.LoadSoundsBundle(IN_GAME_MENU_SOUNDS_BUNDLE_PATH,OnSoundsLoaded);

        void OnSoundsLoaded(SoundsBundle bundle)
        {
            _soundsBundle = bundle;
            var ambience = _soundsBundle.GetAudioClipById(IN_GAME_AMBIENCE_ID);
            SoundsManager.Instance.PlayAmbience(ambience, true);

            _clickSound = _soundsBundle.GetAudioClipById(MENU_CLICK_ID);
            _clickBackSound = _soundsBundle.GetAudioClipById(MENU_CLICK_BACK_ID);
            _openBookSound = _soundsBundle.GetAudioClipById(OPEN_BOOK_ID);
        }
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    private void SetMenuButtonsInteractive(bool interactive)
    {
        foreach (Button button in _menuButtons)
        {
            button.interactable = interactive;
        }
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
        InputController.Instance.SetInputScheme(UIState.InGameMenu);
        OnBackButtonDelegate = ResumeGame;

        UIFader.FadeIn(_inGameMenuPanel, () => SetMenuButtonsInteractive(true), IN_GAME_MENU_FADE_RATE);
        UIFader.FadeOut(_playerHUD, rate: IN_GAME_MENU_FADE_RATE);

        SoundsManager.Instance.PlaySFX(_clickSound);
    }

    public void ResumeGame()
    {
        SetMenuButtonsInteractive(false);
        OnBackButtonDelegate = null;

        UIFader.FadeIn(_playerHUD, rate: IN_GAME_MENU_FADE_RATE);
        UIFader.FadeOut(
            _inGameMenuPanel,
            () => InputController.Instance.SetInputScheme(UIState.Inactive),
            IN_GAME_MENU_FADE_RATE
        );

        SoundsManager.Instance.PlaySFX(_clickBackSound);
    }

    public void ShowOptionsPanel()
    {
        if (_isFading) return;

        _isFading = true;
        _inGameOptionsPanel.gameObject.SetActive(true);
        _inGameOptionsPanel.Initialize();
        _currentPanel = _inGameOptionsPanel.gameObject;

        SetMenuButtonsInteractive(false);
        OnBackButtonDelegate = BackToMenu;

        UIFader.FadeOut(_selectionMenu, null, IN_GAME_MENU_FADE_RATE);
        UIFader.FadeIn(
            _inGameOptionsPanel.gameObject,
            () => _isFading = false,
            IN_GAME_MENU_FADE_RATE
        );

        SoundsManager.Instance.PlaySFX(_clickSound);
    }

    public void ShowInventory()
    {
        if (_isFading) return;

        _isFading = true;
        _indexPanel.gameObject.SetActive(true);
        _indexPanel.Initialize();
        _currentPanel = _indexPanel.gameObject;
        OnBackButtonDelegate = BackToMenu;

        SetMenuButtonsInteractive(false);

        UIFader.FadeOut(_selectionMenu, null, IN_GAME_MENU_FADE_RATE);
        UIFader.FadeIn(_indexPanel.gameObject, () => _isFading = false, IN_GAME_MENU_FADE_RATE);

        SoundsManager.Instance.PlaySFX(_openBookSound);
    }

    private void OpenInventoryImediate()
    {
        if (_isFading) return;

        _isImediateIndex = true;
        InputController.Instance.SetInputScheme(UIState.InventoryPanel);
        OnBackButtonDelegate = CloseInventoryImediate;

        _isFading = true;
        _indexPanel.gameObject.SetActive(true);
        _indexPanel.Initialize();

        UIFader.FadeIn(_inGameMenuPanel, rate: IN_GAME_MENU_FADE_RATE);
        _selectionMenu.SetActive(false);

        UIFader.FadeIn(_indexPanel.gameObject, () => _isFading = false, IN_GAME_MENU_FADE_RATE);
        UIFader.FadeOut(_playerHUD, rate: IN_GAME_MENU_FADE_RATE);

        SoundsManager.Instance.PlaySFX(_openBookSound);
    }

    private void CloseInventoryImediate()
    {
        if (_isFading) return;

        _isFading = true;
        OnBackButtonDelegate = null;

        UIFader.FadeIn(_playerHUD, rate: IN_GAME_MENU_FADE_RATE);
        UIFader.FadeOut(_indexPanel.gameObject, ReturnToGameFromIndex, IN_GAME_MENU_FADE_RATE);

        UIFader.FadeOut(
            _inGameMenuPanel,
            () => _selectionMenu.SetActive(true),
            IN_GAME_MENU_FADE_RATE
        );

        SoundsManager.Instance.PlaySFX(_clickBackSound);

        void ReturnToGameFromIndex()
        {
            InputController.Instance.SetInputScheme(UIState.Inactive);
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

        UIFader.FadeOut(_indexPanel.gameObject, null, IN_GAME_MENU_FADE_RATE);
        UIFader.FadeIn(_artifactInfoPanel.gameObject, CloseIndexPanel, IN_GAME_MENU_FADE_RATE);

        SoundsManager.Instance.PlaySFX(_clickSound);

        void CloseIndexPanel()
        {
            _indexPanel.gameObject.SetActive(false);
            _isFading = false;
        }
    }

    private void OpenArtifactInfoImediate(InitArgs args)
    {
        if (_isFading) return;

        InputController.Instance.SetInputScheme(UIState.InventoryPanel);
        OnBackButtonDelegate = CloseArtifactInfoImediate;

        _isFading = true;
        _artifactInfoPanel.gameObject.SetActive(true);
        _artifactInfoPanel.Initialize(args);

        UIFader.FadeIn(_inGameMenuPanel, rate: IN_GAME_MENU_FADE_RATE);
        _selectionMenu.SetActive(false);

        UIFader.FadeIn(_artifactInfoPanel.gameObject, () => _isFading = false, IN_GAME_MENU_FADE_RATE);
        UIFader.FadeOut(_playerHUD, rate: IN_GAME_MENU_FADE_RATE);

        SoundsManager.Instance.PlaySFX(_openBookSound);
    }

    private void CloseArtifactInfoImediate()
    {
        if (_isFading) return;

        _isFading = true;
        OnBackButtonDelegate = null;

        UIFader.FadeIn(_playerHUD, rate: IN_GAME_MENU_FADE_RATE);
        UIFader.FadeOut(_artifactInfoPanel.gameObject, ReturnToGameFromArtifactInfo, IN_GAME_MENU_FADE_RATE);

        UIFader.FadeOut(
            _inGameMenuPanel,
            () => _selectionMenu.SetActive(true),
            IN_GAME_MENU_FADE_RATE
        );


        SoundsManager.Instance.PlaySFX(_clickBackSound);

        void ReturnToGameFromArtifactInfo()
        {
            InputController.Instance.SetInputScheme(UIState.Inactive);
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

        UIFader.FadeIn(_indexPanel.gameObject, null, IN_GAME_MENU_FADE_RATE);
        UIFader.FadeOut(_artifactInfoPanel.gameObject, CloseArtifactInfoPanel, IN_GAME_MENU_FADE_RATE);

        SoundsManager.Instance.PlaySFX(_clickBackSound);

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

        UIFader.FadeIn(_selectionMenu, () => OnBackButtonDelegate = ResumeGame, IN_GAME_MENU_FADE_RATE);
        UIFader.FadeOut(_currentPanel, CloseCurrentPanel, IN_GAME_MENU_FADE_RATE);

        SoundsManager.Instance.PlaySFX(_clickBackSound);
    }

    public void BackToMainMenu()
    {
        SetMenuButtonsInteractive(false);

        ScenesController.Instance.BackToMainMenu();
        SoundsManager.Instance.PlaySFX(_clickBackSound);
        SoundsManager.Instance.BackToMainMenu(ClearSoundAssets);

        void ClearSoundAssets()
        {
            _clickSound = null;
            _clickBackSound = null;
            Resources.UnloadAsset(_soundsBundle);
        }
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
