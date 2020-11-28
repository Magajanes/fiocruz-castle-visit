using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private InventoryManager inventoryManager;

    [Header("Delete PlayerPrefs")]
    [SerializeField]
    private bool deletePlayerPrefsOnStart;

    private void Start()
    {
        if (deletePlayerPrefsOnStart)
            PlayerPrefs.DeleteAll();

        Cursor.lockState = CursorLockMode.Locked;
        InputController.LockInputs(true);
        InitializeLoading();
    }

    private void InitializeLoading()
    {
        ArtifactsService.LoadGameArtifacts(() => {
            Debug.Log("Assets loaded");
            InitializeManagers();
            InitializeInventory();
            InputController.LockInputs(false);
        });
    }

    private void InitializeManagers()
    {
        uiManager.Initialize();
        inventoryManager.Initialize();
    }

    private void InitializeInventory()
    {
        InventoryService.InitializeInventory();
    }
}
