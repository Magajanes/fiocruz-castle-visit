using UnityEngine;

public class Player : MonoBehaviour
{
    private static Inventory _inventory;

    public static Inventory Inventory
    {
        get
        {
            InitializeInventory();
            return _inventory;
        }
    }

    private void Awake()
    {
        InputController.OnInventoryButtonPress += ShowInventory;
    }

    private void Start()
    {
        InitializeInventory();
    }

    private void OnDisable()
    {
        InputController.OnInventoryButtonPress -= ShowInventory;
    }

    private void OnDestroy()
    {
        InputController.OnInventoryButtonPress -= ShowInventory;
    }

    private static void InitializeInventory()
    {
        if (_inventory != null)
            return;
        _inventory = new Inventory();
        _inventory.Initialize();
    }

    private void ShowInventory()
    {
        var args = new InteractionArgs(UIState.InventoryPanel);
        UIManager.ChangeUIState(args);
    }
}
