using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public void Initialize()
    {
        InputController.OnInventoryButtonPress += ShowInventory;
        InputController.OnBackButtonPress += CloseInventory;
    }

    private void OnDestroy()
    {
        InputController.OnInteractionButtonPress -= ShowInventory;
        InputController.OnBackButtonPress -= CloseInventory;
    }

    private void ShowInventory()
    {
        UIManager.ChangeState(UIState.InventoryPanel);
    }

    private void CloseInventory()
    {
        UIManager.ChangeState(UIState.Inactive);
    }
}
