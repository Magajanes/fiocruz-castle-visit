using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : GameSingleton<LoadingScreen>
{
    [Header("References")]
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Image _loadingBarImage;

    [Header("Panels")]
    [SerializeField]
    private GameObject _background;
    [SerializeField]
    private GameObject _picture;

    public void FadeIn(Action onLoadFinished = null)
    {
        _canvasGroup.blocksRaycasts = true;
        UIFader.FadeIn(
            _background,
            ShowLoadingPicture
        );

        void ShowLoadingPicture()
        {
            UIFader.FadeIn(
                _picture, 
                onLoadFinished, 
                2
            );
        }
    }

    public void FadeOut()
    {
        UIFader.FadeOut(
            _picture,
            FadeBackgroundOut,
            2
        );

        void FadeBackgroundOut()
        {
            UIFader.FadeOut(
                _background,
                OnFadeOutFinish
            );

            void OnFadeOutFinish()
            {
                SetProgress(0);
                _canvasGroup.blocksRaycasts = false;
            }
        }
    }

    public void SetProgress(float progress)
    {
        _loadingBarImage.fillAmount = progress;
    }
}
