using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : GameSingleton<ScenesController>
{
    [SerializeField]
    private LoadingScreen _loadingScreen;

    public void StartGame()
    {
        _loadingScreen.FadeIn(LoadMainScene);
    }

    public void BackToMainMenu()
    {
        _loadingScreen.FadeIn(LoadMainMenuScene);
    }

    private void LoadMainScene()
    {
        var loadOperation = SceneManager.LoadSceneAsync("NewMain", LoadSceneMode.Additive);
        StartCoroutine(ShowProgress(loadOperation));

        loadOperation.completed += (load) =>
        {
            var unloadOperation = SceneManager.UnloadSceneAsync("MainMenu");
            unloadOperation.completed += (unload) =>
            {
                _loadingScreen.FadeOut();
            };
        };
    }

    private void LoadMainMenuScene()
    {
        var loadOperation = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
        StartCoroutine(ShowProgress(loadOperation));

        loadOperation.completed += (load) =>
        {
            var unloadOperation = SceneManager.UnloadSceneAsync("NewMain");
            unloadOperation.completed += (unload) =>
            {
                _loadingScreen.FadeOut();
            };
        };
    }

    private IEnumerator ShowProgress(AsyncOperation loadOperation)
    {
        while (loadOperation.progress < 1)
        {
            _loadingScreen.SetProgress(loadOperation.progress);
            yield return null;
        }
        _loadingScreen.SetProgress(1);
    }
}
