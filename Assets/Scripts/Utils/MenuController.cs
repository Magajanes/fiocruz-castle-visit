using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private bool _inputLock = true;
    private bool _isAtStartScreen = true;
    
    [Header("References")]
    [SerializeField]
    private UIFader uiFader;
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
    public static event Action OnGameStart;

    private void Awake()
    {
        //Sound starts here
    }

    private void Start()
    {
        uiFader.FadeIn(
            mainMenu,
            OnFadeFinish,
            0.25f
        );

        void OnFadeFinish()
        {
            uiFader.FadeIn(
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
            uiFader.FadeOut(mainMenu);
            uiFader.FadeIn(
                menu,
                ShowMenu
            );
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
        uiFader.FadeOut(selectionMenu);
        OnSelectionMenuFade?.Invoke(false);

        uiFader.FadeIn(
            optionsPanel,
            UnlockInput
        );
    }

    //This method is called from "COMPENDIUM" button in scene
    public void ShowInventory()
    {
        if (_isAtStartScreen || _inputLock)
            return;
        
        inventoryPanel.SetActive(true);
        uiFader.FadeOut(selectionMenu);
        OnSelectionMenuFade?.Invoke(false);

        uiFader.FadeIn(inventoryPanel);
        var inventory = inventoryPanel.GetComponent<InventoryPanel>();
        inventory.Initialize();
    }

    //This method is called from Inventory Slots buttons in MainMenu scene
    public void ShowArtifactInfo(int artifactId)
    {
        if (!InventoryManager.PlayerInventory.HasArtifact(artifactId))
            return;
        
        artifactInfo.SetActive(true);
        uiFader.FadeIn(artifactInfo);
        uiFader.FadeOut(inventoryPanel);
        var artifactInfoPanel = artifactInfo.GetComponent<ArtifactInfoPanel>();
        artifactInfoPanel.Initialize(artifactId);
    }

    public void BackToStartScreen()
    {
        if (_inputLock)
            return;

        LockInput();
        uiFader.FadeOut(menu);
        OnSelectionMenuFade?.Invoke(false);

        uiFader.FadeIn(
            mainMenu,
            ShowStartScreen
        );
    }

    public void BackToMenu()
    {
        if (_inputLock)
            return;

        LockInput();
        uiFader.FadeIn(selectionMenu);
        OnSelectionMenuFade?.Invoke(true);

        GameObject currentPanel = inventoryPanel.activeInHierarchy ? inventoryPanel : optionsPanel;
        uiFader.FadeOut(
            currentPanel,
            () => {
                currentPanel.SetActive(false);
                UnlockInput();
            }
        );
    }

    public void BackToInventory()
    {
        if (_inputLock)
            return;

        LockInput();
        uiFader.FadeIn(inventoryPanel);
        uiFader.FadeOut(
            artifactInfo,
            () => {
                artifactInfo.SetActive(false);
                UnlockInput();
            }
        );
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
        OnGameStart?.Invoke();
    }

    private void ApplySavedPlayerPrefs()
    {
        bool invertY = PlayerPrefsService.GetBool(PlayerPrefsService.INVERT_MOUSE_Y);
        InputController.Instance.SetInvertMouseY(invertY);
        toggle.isOn = invertY;
    }

    public void SetInvertMouseY(bool active)
    {
        PlayerPrefsService.SetBool(PlayerPrefsService.INVERT_MOUSE_Y, active);
        InputController.Instance.SetInvertMouseY(active);
    }
}
