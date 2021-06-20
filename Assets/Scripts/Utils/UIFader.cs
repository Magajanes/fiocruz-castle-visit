using System;
using System.Collections;
using UnityEngine;

public class UIFader : GameSingleton<UIFader>
{
    public static void FadeIn(GameObject target, Action onFadeFinish = null, float rate = 1)
    {
        var canvasGroup = target.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError($"CanvasGroup component not found in game object! Name: {target.name}");
            return;
        }
        
        canvasGroup.alpha = 0;
        Instance.StartCoroutine(
            FadeInCoroutine(
                canvasGroup,
                onFadeFinish,
                rate
            )
        );
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
        Instance.StartCoroutine(
            FadeOutCoroutine(
                canvasGroup,
                onFadeFinish,
                rate
            )
        );
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

        if (canvasGroup == null) yield break;
        canvasGroup.alpha = 0;
        onFadeFinish?.Invoke();
    }
}
