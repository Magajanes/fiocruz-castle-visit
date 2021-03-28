using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private UIFader _uiFader;
    [SerializeField]
    private Slider _loadingProgressSlider;

    [Header("Panels")]
    [SerializeField]
    private GameObject _background;
    [SerializeField]
    private GameObject _picture;

    public void FadeIn(Action onLoadFinished = null)
    {
        _canvasGroup.blocksRaycasts = true;
        _uiFader.FadeIn(
            _background,
            ShowLoadingPicture
        );

        void ShowLoadingPicture()
        {
            _uiFader.FadeIn(
                _picture, 
                onLoadFinished, 
                2
            );
        }
    }

    public void FadeOut()
    {
        _uiFader.FadeOut(
            _picture,
            FadeBackgroundOut,
            2
        );

        void FadeBackgroundOut()
        {
            _uiFader.FadeOut(
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
        _loadingProgressSlider.value = progress * _loadingProgressSlider.maxValue;
    }
}
