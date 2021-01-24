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

    public void InitializeInventory()
    {
        PlayerInventory = new Inventory();
        PlayerInventory.Initialize();
    }

    public void SaveArtifact(int artifactId)
    {
        PlayerInventory.AddArtifact(artifactId);
    }

    public List<int> GetCollectedArtifactsIds()
    {
        return PlayerInventory.GetCollectedArtifactsIds();
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
