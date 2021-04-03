using System;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private bool _inputLock = true;
    private bool _isAtStartScreen = true;
    
    [Header("References")]
    [SerializeField]
    private UIFader uiFader;

    [Header("UI Elements")]
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject mainText;
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject inventoryPanel;
    [SerializeField]
    private GameObject artifactInfo;

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
    }

    //This method is called from "COMPENDIUM" button in scene
    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
        uiFader.FadeIn(inventoryPanel);
        var inventory = inventoryPanel.GetComponent<InventoryPanel>();
        inventory.Initialize();
    }

    //This method is called from Inventory Slots buttons in MainMenu scene
    public void ShowArtifactInfo(int artifactId)
    {
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

        _inputLock = true;
        uiFader.FadeOut(menu);
        uiFader.FadeIn(
            mainMenu,
            ShowStartScreen
        );
    }

    public void BackToMenu()
    {
        if (_inputLock)
            return;

        _inputLock = true;
        uiFader.FadeOut(
            inventoryPanel,
            () => {
                inventoryPanel.SetActive(false);
                _inputLock = false;
            }
        );
    }

    public void BackToInventory()
    {
        if (_inputLock)
            return;

        _inputLock = true;
        uiFader.FadeIn(inventoryPanel);
        uiFader.FadeOut(
            artifactInfo,
            () => {
                artifactInfo.SetActive(false);
                _inputLock = false;
            }
        );
    }

    private void UnlockInput()
    {
        _inputLock = false;
    }

    public void StartGame()
    {
        OnGameStart?.Invoke();
    }
}
