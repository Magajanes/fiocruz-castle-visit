using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : GameSingleton<ScenesController>
{
    [SerializeField]
    private LoadingScreen _loadingScreen;

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

    private void LoadMainScene()
    {
        var loadOperation = SceneManager.LoadSceneAsync("NewMain", LoadSceneMode.Additive);
        StartCoroutine(ShowProgress());

        loadOperation.completed += (load) =>
        {
            var unloadOperation = SceneManager.UnloadSceneAsync("MainMenu");
            unloadOperation.completed += (unload) =>
            {
                _loadingScreen.FadeOut();
            };
        };

        IEnumerator ShowProgress()
        {
            while (loadOperation.progress < 1)
            {
                _loadingScreen.SetProgress(loadOperation.progress);
                yield return null;
            }
            _loadingScreen.SetProgress(1);
        }
    }
}
