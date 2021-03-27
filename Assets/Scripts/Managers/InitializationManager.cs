using UnityEngine;

public class InitializationManager : Singleton<InitializationManager>
{
    public const string INVENTORY_MANAGER_PREFAB_PATH = "Prefabs/InventoryManager";

    [SerializeField]
    private InventoryPanel _inventoryPanel;

    private void Start()
    {
        ArtifactsService.LoadGameArtifacts(InstantiateIventoryManager);
    }

    private void InstantiateIventoryManager()
    {
        ResourceRequest request = Resources.LoadAsync(INVENTORY_MANAGER_PREFAB_PATH);
        request.completed += InstantiateInventoryManager;

        void InstantiateInventoryManager(AsyncOperation operation)
        {
            var prefab = request.asset as GameObject;
            var inventoryManager = Instantiate(prefab).GetComponent<InventoryManager>();
            DontDestroyOnLoad(inventoryManager.gameObject);
            inventoryManager.Initialize(_inventoryPanel);
        }
    }

}

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this as T;
        DontDestroyOnLoad(gameObject);
    }
}
