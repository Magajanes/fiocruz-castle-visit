using System;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    
    [SerializeField]
    private UIFader uiFader;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn(Action loadCallback = null)
    {
        uiFader.FadeIn(_canvasGroup, loadCallback);
    }

    public void FadeOut()
    {
        uiFader.FadeOut(_canvasGroup);
    }
}
