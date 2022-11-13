using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class Root : SceneBase
{
    CanvasGroup canvasGroup;

    public override IObservable<SceneBase> PrepareSceneAsync()
    {
        return Observable.Return(this)
        .Do(x =>
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
        });
    }

    public override IObservable<Unit> StartSceneAsync()
    {
        return CanvasFade.StartFadeIn(canvasGroup);
    }

    public override IObservable<Unit> BackSceneAsync()
    {
        return CanvasFade.StartFadeOut(canvasGroup);
    }
}
