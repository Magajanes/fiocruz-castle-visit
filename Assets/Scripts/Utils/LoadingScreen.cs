using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private UIFader uiFader;
    [SerializeField]
    private Slider loadingProgressSlider;

    [Header("Panels")]
    [SerializeField]
    private GameObject _background;
    [SerializeField]
    private GameObject _picture;

    public void FadeIn(Action loadCallback = null)
    {
        uiFader.FadeIn(
            _background,
            ShowLoadingPicture
        );

        void ShowLoadingPicture()
        {
            uiFader.FadeIn(
                _picture, 
                loadCallback, 
                2
            );
        }
    }

    public void FadeOut()
    {
        uiFader.FadeOut(
            _picture,
            FadeBackgroundOut,
            2
        );

        void FadeBackgroundOut()
        {
            uiFader.FadeOut(
                _background,
                () => SetProgress(0)
            );
        }
    }

    public void SetProgress(float progress)
    {
        loadingProgressSlider.value = progress * loadingProgressSlider.maxValue;
    }
}
