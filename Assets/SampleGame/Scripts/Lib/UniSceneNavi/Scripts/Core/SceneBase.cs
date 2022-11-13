using UnityEngine;
using UniRx;
using Zenject;
using System;

public abstract class SceneBase : MonoBehaviour
{
    [InjectOptional(Id = "SceneBaseArgument")] public object Argument { get; protected set; }

    public virtual void ArgParseTo(SceneParam p) { }

    public bool IsLoaded { get; set; }

    public virtual IObservable<SceneBase> PrepareSceneAsync()
    {
        return Observable.Return(this);
    }

    public virtual IObservable<Unit> StartSceneAsync()
    {
        return Observable.Return(Unit.Default);
    }

    public virtual IObservable<Unit> BackSceneAsync()
    {
        return Observable.Return(Unit.Default);
    }

    public virtual IObservable<Unit> BehindSceneAsync()
    {
        return Observable.Return(Unit.Default);
    }

    public virtual IObservable<Unit> OnRestoreSceneAsync()
    {
        return Observable.Return(Unit.Default);
    }
}

public abstract class SceneBase<T> : SceneBase
{
    public override void ArgParseTo(SceneParam p)
    {
        Argument = p.ParseTo<T>().Argument;
    }
}