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
        InitializeInventory();
    }

    private void InitializeInventory()
    {
        if (PlayerInventory != null) 
            return;

        if (_resetInventory)
            PlayerPrefsService.DeletePlayerPrefs();

        PlayerInventory = new Inventory();
        PlayerInventory.Initialize();
    }
}
