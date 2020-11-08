using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UIManager uiManager;

    private static Inventory _inventory;

    private void Start()
    {
        InitializeLoading();
        InitializeManagers();
        InitializeInventory();
    }

    private void InitializeLoading()
    {
        ArtifactsService.LoadArtifactsInfo(() => {
            Debug.Log("Assets loaded");
        });
    }

    private void InitializeManagers()
    {
        uiManager.Initialize();
    }

    private void InitializeInventory()
    {
        InventoryService.InitializeInventory();
    }
}
