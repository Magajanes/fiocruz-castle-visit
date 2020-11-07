using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static ArtifactsService _artifactsService;
    private static PlayerPrefsService _playerPrefsService;
    private static Inventory _inventory;
    public static event Action OnAssetsLoaded;

    private void Start()
    {
        InitializeServices();
        InitializeLoading();
    }

    private void InitializeServices()
    {
        if (_playerPrefsService == null)
            _playerPrefsService = new PlayerPrefsService();

        if (_artifactsService == null)
            _artifactsService = new ArtifactsService(_playerPrefsService);
    }

    private void InitializeLoading()
    {
        _artifactsService.LoadArtifacts(OnAssetsLoaded);
    }

    private void InitializeInventory()
    {
        if (_inventory == null)
        {
            _inventory = new Inventory(_playerPrefsService);
            _inventory.Initialize();
        }
    }
}
