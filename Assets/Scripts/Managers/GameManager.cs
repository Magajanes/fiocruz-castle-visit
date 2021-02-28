using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private LightManager lightManager;

    [Header("Delete PlayerPrefs")]
    [SerializeField]
    private bool deletePlayerPrefsOnStart;

    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

    private void Start()
    {
        if (deletePlayerPrefsOnStart)
            PlayerPrefs.DeleteAll();

        Cursor.lockState = CursorLockMode.Locked;
        InputController.LockInputs(false);
        InitializeLoading();
    }

    private void InitializeLoading()
    {
        ArtifactsService.LoadGameArtifacts(OnAssetsLoaded);

        void OnAssetsLoaded()
        {
            Debug.Log("Assets loaded");
            LoadUIManager();
        }
    }

    private void LoadUIManager()
    {
        ResourceRequest request = Resources.LoadAsync("Prefabs/UIManager");
        request.completed += InstantiateUIManager;

        void InstantiateUIManager(AsyncOperation operation)
        {
            var prefab = request.asset as GameObject;
            var uiManager = Instantiate(prefab).GetComponent<UIManager>();
            uiManager.transform.SetParent(transform.parent);
            uiManager.Initialize();
            LoadIventoryManager();
        }
    }

    private void LoadIventoryManager()
    {
        ResourceRequest request = Resources.LoadAsync("Prefabs/InventoryManager");
        request.completed += InstantiateInventoryManager;

        void InstantiateInventoryManager(AsyncOperation operation)
        {
            var prefab = request.asset as GameObject;
            var inventoryManager = Instantiate(prefab).GetComponent<InventoryManager>();
            inventoryManager.transform.SetParent(transform.parent);
            inventoryManager.Initialize();
            lightManager.Initialize();
            InputController.LockInputs(false);
        }
    }
}
