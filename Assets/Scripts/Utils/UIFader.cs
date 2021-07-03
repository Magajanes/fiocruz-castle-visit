using System;
using System.Collections;
using UnityEngine;

public class UIFader : GameSingleton<UIFader>
{
    private static Coroutine _fadeInCoroutine;
    private static Coroutine _fadeOutCoroutine;
    
    public static void FadeIn(GameObject target, Action onFadeFinish = null, float rate = 1)
    {
        var canvasGroup = target.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError($"CanvasGroup component not found in game object! Name: {target.name}");
            return;
        }
        
        canvasGroup.alpha = 0;
        _fadeInCoroutine = Instance.StartCoroutine(FadeInCoroutine(canvasGroup, onFadeFinish, rate));
    }

    public static void FadeOut(GameObject target, Action onFadeFinish = null, float rate = 1)
    {
        var canvasGroup = target.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError($"CanvasGroup component not found in game object! Name: {target.name}");
            return;
        }

        canvasGroup.alpha = 1;
        _fadeOutCoroutine = Instance.StartCoroutine(FadeOutCoroutine(canvasGroup, onFadeFinish, rate));
    }

    public static void StopFadeInCoroutine()
    {
        if (_fadeInCoroutine == null) return;

        Instance.StopCoroutine(_fadeInCoroutine);
        _fadeInCoroutine = null;
    }

    public static void StopFadeOutCoroutine()
    {
        if (_fadeOutCoroutine == null) return;

        Instance.StopCoroutine(_fadeOutCoroutine);
        _fadeOutCoroutine = null;
    }

    private static IEnumerator FadeInCoroutine(CanvasGroup canvasGroup, Action onFadeFinish, float rate)
    {
        float alpha = 0;
        while (canvasGroup != null && canvasGroup.alpha < 1)
        {
            alpha += rate * Time.deltaTime;
            canvasGroup.alpha = alpha;
            yield return null;
        }

        _fadeInCoroutine = null;
        if (canvasGroup == null) yield break;

        canvasGroup.alpha = 1;
        onFadeFinish?.Invoke();
    }

    private static IEnumerator FadeOutCoroutine(CanvasGroup canvasGroup, Action onFadeFinish, float rate)
    {
        float alpha = 1;
        while (canvasGroup != null && canvasGroup.alpha > 0)
        {
            alpha -= rate * Time.deltaTime;
            canvasGroup.alpha = alpha;
            yield return null;
        }

        _fadeOutCoroutine = null;
        if (canvasGroup == null) yield break;
        
        canvasGroup.alpha = 0;
        onFadeFinish?.Invoke();
    }
}
