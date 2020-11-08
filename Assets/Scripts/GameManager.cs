using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static ArtifactsService _artifactsService;
    private static PlayerPrefsService _playerPrefsService;
    private static Inventory _inventory;
    public static event Action OnInventoryDeleted;

    [Header("Managers")]
    [SerializeField]
    private UIManager uiManager;

    private void Start()
    {
        InitializeServices();
        InitializeInventory();
        InitializeLoading();
        InitializeManagers();
    }

    private void InitializeServices()
    {
        if (_playerPrefsService == null)
            _playerPrefsService = new PlayerPrefsService();

        if (_artifactsService == null)
            _artifactsService = new ArtifactsService(_playerPrefsService);

        Debug.Log("Services initialized!");
    }

    private void InitializeLoading()
    {
        _artifactsService.LoadArtifacts(() => {
            Debug.Log("Assets loaded!");
        });
    }

    private void InitializeInventory()
    {
        if (_inventory == null)
        {
            _inventory = new Inventory(_playerPrefsService);
            _inventory.Initialize();
        }
    }

    private void InitializeManagers()
    {
        uiManager.Initialize(_artifactsService, _inventory);
    }

    public void DeleteInventory()
    {
        _playerPrefsService.DeleteInventory();
        _inventory.Initialize();
        OnInventoryDeleted?.Invoke();
    }
}
