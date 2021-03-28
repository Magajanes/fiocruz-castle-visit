using UnityEngine;

public class InitializationManager : Singleton<InitializationManager>
{
    private void Start()
    {
        ArtifactsService.LoadGameArtifacts(null);
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
