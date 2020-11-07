using UnityEngine;

public class Player : MonoBehaviour
{
    private void Awake()
    {
        InputController.OnInventoryButtonPress += ShowInventory;
    }

    private void OnDisable()
    {
        InputController.OnInventoryButtonPress -= ShowInventory;
    }

    private void OnDestroy()
    {
        InputController.OnInventoryButtonPress -= ShowInventory;
    }

    private void ShowInventory()
    {
        var args = new InteractionArgs(UIState.InventoryPanel);
        UIManager.ChangeUIState(args);
    }
}
