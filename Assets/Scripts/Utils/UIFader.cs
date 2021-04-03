using System;
using System.Collections;
using UnityEngine;

public class UIFader : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn(GameObject target, Action onFadeFinish = null, float rate = 1)
    {
        var canvasGroup = target.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError($"CanvasGroup component not found in game object! Name: {target.name}");
            return;
        }
        
        canvasGroup.alpha = 0;
        StartCoroutine(
            FadeInCoroutine(
                canvasGroup,
                onFadeFinish,
                rate
            )
        );
    }

    public void FadeOut(GameObject target, Action onFadeFinish = null, float rate = 1)
    {
        var canvasGroup = target.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError($"CanvasGroup component not found in game object! Name: {target.name}");
            return;
        }

        canvasGroup.alpha = 1;
        StartCoroutine(
            FadeOutCoroutine(
                canvasGroup,
                onFadeFinish,
                rate
            )
        );
    }

    private IEnumerator FadeInCoroutine(CanvasGroup canvasGroup, Action onFadeFinish, float rate)
    {
        float alpha = 0;
        while (canvasGroup.alpha < 1)
        {
            alpha += rate * Time.deltaTime;
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = 1;
        onFadeFinish?.Invoke();
    }

    private IEnumerator FadeOutCoroutine(CanvasGroup canvasGroup, Action onFadeFinish, float rate)
    {
        float alpha = 1;
        while (canvasGroup.alpha > 0)
        {
            alpha -= rate * Time.deltaTime;
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = 0;
        onFadeFinish?.Invoke();
    }
}
