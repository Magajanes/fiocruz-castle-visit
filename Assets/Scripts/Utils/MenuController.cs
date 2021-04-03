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

    private void UnlockInput()
    {
        _inputLock = false;
    }

    private void ShowMenu()
    {
        UnlockInput();
        _isAtStartScreen = false;
    }

    private void ShowStartScreen()
    {
        UnlockInput();
        _isAtStartScreen = true;
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

    public void StartGame()
    {
        OnGameStart?.Invoke();
    }

    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
        uiFader.FadeIn(inventoryPanel);
        var inventory = inventoryPanel.GetComponent<InventoryPanel>();
        inventory.Initialize();
    }
}
