using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : Singleton<MenuController>
{
    private bool _inputLock = true;
    private bool _isAtStartScreen = true;

    private SoundsBundle _soundsBundle;
    private AudioClip _clickSound;
    private AudioClip _clickBackSound;

    private GameObject _currentPanel;
    
    [Header("References")]
    [SerializeField]
    private Toggle toggle;

    [Header("UI Elements")]
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject mainText;
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject selectionMenu;
    [SerializeField]
    private GameObject optionsPanel;
    [SerializeField]
    private GameObject creditsPanel;
    [SerializeField]
    private GameObject inventoryPanel;
    [SerializeField]
    private GameObject artifactInfo;

    private void Start()
    {
        SoundsManager.LoadSoundsBundle(
            MenuConstants.MAIN_MENU_SOUNDS_BUNDLE_PATH,
            OnSoundsLoaded
        );
        
        UIFader.FadeIn(
            mainMenu,
            OnFadeFinish,
            0.5f
        );

        void OnSoundsLoaded(SoundsBundle bundle)
        {
            _soundsBundle = bundle;
            var music = _soundsBundle.GetAudioClipById(MenuConstants.INTRO_MUSIC_ID);
            SoundsManager.Instance.PlayMusic(music, true);
            _clickSound = _soundsBundle.GetAudioClipById(MenuConstants.MENU_CLICK_ID);
            _clickBackSound = _soundsBundle.GetAudioClipById(MenuConstants.MENU_CLICK_BACK_ID);
        }

        void OnFadeFinish()
        {
            UIFader.FadeIn(
                mainText,
                () => _inputLock = false
            );
        }
    }

    private void Update()
    {
        if (_inputLock)
            return;
        
        if (_isAtStartScreen && Input.anyKeyDown)
        {
            _inputLock = true;
            UIFader.FadeOut(mainMenu);
            UIFader.FadeIn(
                menu,
                () =>
                {
                    _inputLock = false;
                    _isAtStartScreen = false;
                }
            );

            SoundsManager.Instance.PlaySFX(_clickSound);
        }
    }

    public void ShowOptionsPanel(Action onComplete)
    {
        if (_isAtStartScreen || _inputLock)
            return;

        _inputLock = true;
        ApplySavedPlayerPrefs();
        optionsPanel.SetActive(true);

        if (_currentPanel != null)
        {
            UIFader.FadeOut(_currentPanel);
        }
        else
        {
            UIFader.FadeIn(selectionMenu);
        }

        UIFader.FadeIn(
            optionsPanel,
            () => 
            {
                _inputLock = false;
                _currentPanel?.SetActive(false);
                _currentPanel = optionsPanel;
                onComplete?.Invoke();
            }
        );

        SoundsManager.Instance.PlaySFX(_clickSound);
    }
    public void ShowCreditsPanel(Action onComplete)
    {
        if (_isAtStartScreen || _inputLock)
            return;

        _inputLock = true;
        creditsPanel.SetActive(true);

        if (_currentPanel != null)
        {
            UIFader.FadeOut(_currentPanel);
        }
        else
        {
            UIFader.FadeIn(selectionMenu);
        }

        UIFader.FadeIn(
            creditsPanel,
            () =>
            {
                _inputLock = false;
                _currentPanel?.SetActive(false);
                _currentPanel = creditsPanel;
                onComplete?.Invoke();
            }
        );

        SoundsManager.Instance.PlaySFX(_clickSound);
    }

    public void ShowInventory(Action onComplete)
    {
        if (_isAtStartScreen || _inputLock)
            return;

        _inputLock = true;
        inventoryPanel.SetActive(true);

        if (_currentPanel != null)
        {
            UIFader.FadeOut(_currentPanel);
        }
        else
        {
            UIFader.FadeIn(selectionMenu);
        }

        UIFader.FadeIn(
            inventoryPanel, 
            () => 
            {
                _inputLock = false;
                _currentPanel?.SetActive(false);
                _currentPanel = inventoryPanel;
                onComplete?.Invoke();
            }
        );

        SoundsManager.Instance.PlaySFX(_clickSound);

        var inventory = inventoryPanel.GetComponent<InventoryPanel>();
        inventory.Initialize();
    }

    public void ShowArtifactInfo(int artifactId)
    {
        if (!InventoryManager.PlayerInventory.HasArtifact(artifactId))
            return;

        _inputLock = true;
        artifactInfo.SetActive(true);

        if (_currentPanel != null)
        {
            UIFader.FadeOut(_currentPanel);
        }

        UIFader.FadeIn(
            artifactInfo,
            () =>
            {
                _inputLock = false;
                _currentPanel?.SetActive(false);
                _currentPanel = artifactInfo;
            }
        );

        SoundsManager.Instance.PlaySFX(_clickSound);

        var artifactInfoPanel = artifactInfo.GetComponent<ArtifactInfoPanel>();
        artifactInfoPanel.Initialize(artifactId);
    }

    public void BackToInventory()
    {
        if (_inputLock)
            return;

        _inputLock = true;
        inventoryPanel.SetActive(true);

        UIFader.FadeOut(_currentPanel);
        UIFader.FadeIn(
            inventoryPanel,
            CloseArtifactInfo
        );

        void CloseArtifactInfo()
        {
            _currentPanel?.SetActive(false);
            _currentPanel = inventoryPanel;
            _inputLock = false;
        }

        SoundsManager.Instance.PlaySFX(_clickBackSound);
    }

    public void BackToMenu(Action onComplete)
    {
        if (_inputLock)
            return;

        _inputLock = true;

        UIFader.FadeOut(selectionMenu);
        UIFader.FadeOut(
            _currentPanel,
            CloseCurrentPanel
        );

        void CloseCurrentPanel()
        {
            _currentPanel.SetActive(false);
            _currentPanel = null;
            _inputLock = false;
            onComplete?.Invoke();
        }

        SoundsManager.Instance.PlaySFX(_clickBackSound);
    }

    public void StartGame()
    {
        if (_isAtStartScreen || _inputLock)
            return;

        ApplySavedPlayerPrefs();
        ScenesController.Instance.StartGame();
        SoundsManager.Instance.PlaySFX(_clickSound);
        SoundsManager.Instance.FadeOutMusic(SoundsManager.ChannelType.Music, ClearSoundAssets);

        void ClearSoundAssets()
        {
            _clickSound = null;
            _clickBackSound = null;
            Resources.UnloadAsset(_soundsBundle);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void ApplySavedPlayerPrefs()
    {
        PlayerPrefsService.ApplySavedPlayerPrefs(out bool invertY);
        toggle.isOn = invertY;
    }
}

public static class MenuConstants
{
    public const string MAIN_MENU_SOUNDS_BUNDLE_PATH = "SoundBundles/MainMenuSounds";
    public const string INTRO_MUSIC_ID = "intro_music";
    public const string MENU_CLICK_ID = "click";
    public const string MENU_CLICK_BACK_ID = "click_back";
}
