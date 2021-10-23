using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private bool _inputLock = true;
    private bool _isAtStartScreen = true;

    private SoundsBundle _soundsBundle;
    private AudioClip _clickSound;
    private AudioClip _clickBackSound;
    
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
    private GameObject inventoryPanel;
    [SerializeField]
    private GameObject artifactInfo;

    public static event Action<bool> OnSelectionMenuFade;

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
            SoundsManager.Instance.PlayMusic(_soundsBundle.GetAudioClipById(MenuConstants.INTRO_MUSIC_ID));
            _clickSound = _soundsBundle.GetAudioClipById(MenuConstants.MENU_CLICK_ID);
            _clickBackSound = _soundsBundle.GetAudioClipById(MenuConstants.MENU_CLICK_BACK_ID);
        }

        void OnFadeFinish()
        {
            UIFader.FadeIn(
                mainText,
                UnlockInput
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
                ShowMenu
            );

            SoundsManager.Instance.PlaySFX(_clickSound);
        }
    }

    private void ShowStartScreen()
    {
        UnlockInput();
        _isAtStartScreen = true;
    }

    private void ShowMenu()
    {
        UnlockInput();
        _isAtStartScreen = false;
        OnSelectionMenuFade?.Invoke(true);
    }

    public void ShowOptionsPanel()
    {
        if (_isAtStartScreen || _inputLock)
            return;

        LockInput();
        ApplySavedPlayerPrefs();

        optionsPanel.SetActive(true);
        UIFader.FadeOut(selectionMenu);
        OnSelectionMenuFade?.Invoke(false);

        UIFader.FadeIn(
            optionsPanel,
            UnlockInput
        );

        SoundsManager.Instance.PlaySFX(_clickSound);
    }

    public void ShowInventory()
    {
        if (_isAtStartScreen || _inputLock)
            return;

        LockInput();
        inventoryPanel.SetActive(true);
        UIFader.FadeOut(selectionMenu);
        OnSelectionMenuFade?.Invoke(false);

        UIFader.FadeIn(
            inventoryPanel, 
            UnlockInput
        );

        SoundsManager.Instance.PlaySFX(_clickSound);

        var inventory = inventoryPanel.GetComponent<InventoryPanel>();
        inventory.Initialize();
    }

    public void ShowArtifactInfo(int artifactId)
    {
        if (!InventoryManager.PlayerInventory.HasArtifact(artifactId))
            return;

        LockInput();
        artifactInfo.SetActive(true);
        UIFader.FadeIn(artifactInfo);

        UIFader.FadeOut(
            inventoryPanel,
            UnlockInput
        );

        SoundsManager.Instance.PlaySFX(_clickSound);

        var artifactInfoPanel = artifactInfo.GetComponent<ArtifactInfoPanel>();
        artifactInfoPanel.Initialize(artifactId);
    }

    public void BackToStartScreen()
    {
        if (_inputLock)
            return;

        LockInput();
        UIFader.FadeOut(menu);
        OnSelectionMenuFade?.Invoke(false);

        UIFader.FadeIn(
            mainMenu,
            ShowStartScreen
        );

        SoundsManager.Instance.PlaySFX(_clickBackSound);
    }

    public void BackToMenu()
    {
        if (_inputLock)
            return;

        LockInput();
        UIFader.FadeIn(selectionMenu);
        OnSelectionMenuFade?.Invoke(true);

        GameObject currentPanel = inventoryPanel.activeInHierarchy ? inventoryPanel : optionsPanel;
        UIFader.FadeOut(
            currentPanel,
            CloseCurrentPanel
        );

        void CloseCurrentPanel()
        {
            currentPanel.SetActive(false);
            UnlockInput();
        }

        SoundsManager.Instance.PlaySFX(_clickBackSound);
    }

    public void BackToInventory()
    {
        if (_inputLock)
            return;

        LockInput();
        UIFader.FadeIn(inventoryPanel);
        UIFader.FadeOut(
            artifactInfo,
            CloseArtifactInfo
        );

        void CloseArtifactInfo()
        {
            artifactInfo.SetActive(false);
            UnlockInput();
        }

        SoundsManager.Instance.PlaySFX(_clickBackSound);
    }

    private void LockInput()
    {
        _inputLock = true;
    }
    
    private void UnlockInput()
    {
        _inputLock = false;
    }

    public void StartGame()
    {
        if (_isAtStartScreen || _inputLock)
            return;

        ApplySavedPlayerPrefs();
        ScenesController.Instance.StartGame();
        SoundsManager.Instance.PlaySFX(_clickSound);
        SoundsManager.Instance.FadeOutMusic(ClearSoundAssets);

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
