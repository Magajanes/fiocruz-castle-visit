using System;
using System.Collections;
using UnityEngine;

public class UIFader : MonoBehaviour
{
    public void FadeIn(CanvasGroup canvasGroup, Action onFadeFinish = null, float rate = 1)
    {
        canvasGroup.alpha = 0;
        StartCoroutine(FadeInCoroutine());

        IEnumerator FadeInCoroutine()
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
    }

    public void FadeOut(CanvasGroup canvasGroup, Action onFadeFinish = null, float rate = 1)
    {
        canvasGroup.alpha = 1;
        StartCoroutine(FadeOutCoroutine());

        IEnumerator FadeOutCoroutine()
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
}
