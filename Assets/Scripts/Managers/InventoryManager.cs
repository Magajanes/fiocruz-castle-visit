using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static Inventory PlayerInventory;
    
    public void Initialize()
    {
        InputController.OnInventoryButtonPress += ShowInventory;
        InputController.OnBackButtonPress += CloseInventory;

        InitializeInventory();
    }

    private void InitializeInventory()
    {
        if (PlayerInventory != null) return;
        
        PlayerInventory = new Inventory();
        PlayerInventory.Initialize();
    }

    private void ShowInventory()
    {
        UIManager.ChangeState(UIState.InventoryPanel);
    }

    private void CloseInventory()
    {
        UIManager.ChangeState(UIState.Inactive);
    }

    private void OnDestroy()
    {
        InputController.OnInteractionButtonPress -= ShowInventory;
        InputController.OnBackButtonPress -= CloseInventory;
    }
}
