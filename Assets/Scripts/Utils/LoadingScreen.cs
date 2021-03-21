using System;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private UIFader uiFader;

    [Header("Panels")]
    [SerializeField]
    private GameObject _background;
    [SerializeField]
    private GameObject _picture;

    public void FadeIn(Action loadCallback = null)
    {
        uiFader.FadeIn(
            _background,
            () => uiFader.FadeIn(_picture, loadCallback, 2)
        );
    }

    public void FadeOut()
    {
        uiFader.FadeOut(
            _picture,
            () => uiFader.FadeOut(_background),
            2
        );
    }
}
