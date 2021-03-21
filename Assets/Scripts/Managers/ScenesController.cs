using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{
    private static ScenesController _instance;
    public static ScenesController Instance => _instance;

    [SerializeField]
    private LoadingScreen _loadingScreen;

    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);

        _instance = this;
    }

    private void Start()
    {
        MenuController.OnGameStart -= StartGame;
        MenuController.OnGameStart += StartGame;

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(_loadingScreen.gameObject);
    }

    public void StartGame()
    {
        MenuController.OnGameStart -= StartGame;
        _loadingScreen.FadeIn(LoadMainScene);
    }

    public void LoadMainScene()
    {
        var asyncOperation = SceneManager.LoadSceneAsync("NewMain", LoadSceneMode.Additive);
        asyncOperation.completed += (o1) =>
        {
            var unloadOperation = SceneManager.UnloadSceneAsync("MainMenu");
            unloadOperation.completed += (o2) =>
            {
                _loadingScreen.FadeOut();
            };
        };
    }
}
