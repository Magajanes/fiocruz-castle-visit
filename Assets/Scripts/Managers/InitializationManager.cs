using UnityEngine;

public class InitializationManager : Singleton<InitializationManager>
{
    [SerializeField]
    private NewInventoryManager inventoryManager;
    
    private void Start()
    {
        inventoryManager.Initialize();
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
