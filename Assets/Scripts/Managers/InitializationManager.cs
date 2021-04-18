using UnityEngine;

public class InitializationManager : GameSingleton<InitializationManager>
{
    [SerializeField]
    private InventoryManager _inventoryManager;
    
    protected override void Awake()
    {
        Application.targetFrameRate = 120;
        base.Awake();
    }

    private void Start()
    {
        InputController.Instance.LockInputs(true);
        ArtifactsService.LoadGameArtifacts(null);

        _inventoryManager.Initialize();
    }
}

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance;
    public static T Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this as T;
    }
}

public class GameSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
