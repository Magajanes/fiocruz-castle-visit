using UnityEngine;

public class InventoryManager : GameSingleton<InventoryManager>
{
    public static Inventory PlayerInventory;

    [SerializeField]
    private bool _resetInventory;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        ArtifactsService.LoadGameArtifacts();
        
        if (_resetInventory)
        {
            ResetAndInitializeInventory();
            return;
        }

        InitializeInventory();
    }

    private void InitializeInventory()
    {
        if (PlayerInventory != null) return;

        PlayerInventory = new Inventory();
        PlayerInventory.Initialize();
    }

    private void ResetAndInitializeInventory()
    {
        PlayerPrefsService.ResetCollectedArtifacts();
        if (PlayerInventory == null)
        {
            PlayerInventory = new Inventory();
        }
        PlayerInventory.Initialize();
    }
}
