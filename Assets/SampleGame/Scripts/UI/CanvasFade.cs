using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

static public class CanvasFade
{
    static public IObservable<Unit> StartFadeIn(CanvasGroup canvasGroup)
    {
        return Observable.FromMicroCoroutine(() => { return FadeInAlpha(canvasGroup); });
    }

    static public IObservable<Unit> StartFadeOut(CanvasGroup canvasGroup)
    {
        return Observable.FromMicroCoroutine(() => { return FadeOutAlpha(canvasGroup); });
    }

    static IEnumerator FadeInAlpha(CanvasGroup canvasGroup)
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 0.1f;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    static IEnumerator FadeOutAlpha(CanvasGroup canvasGroup)
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.1f;
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}