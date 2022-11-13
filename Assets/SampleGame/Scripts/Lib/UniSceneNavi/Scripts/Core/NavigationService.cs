using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using System.Linq;
using System;
using Zenject;

public static class NavigationService
{
    static public IMessageBroker Broker { get; } = new MessageBroker();
    static public IAsyncMessageBroker AsyncBroker { get; } = new AsyncMessageBroker();
    static Stack<SceneParam> sceneStack = new Stack<SceneParam>();
    static bool isNavigating;
    static ZenjectSceneLoader loader =>
        ProjectContext.Instance.Container.Resolve<ZenjectSceneLoader>();

    /// <summary>
    /// シーンを加算ロードします。
    /// </summary>
    /// <param name="sceneName">読み込むシーン名</param>
    /// <param name="argument">読み込むシーンで必要となる場合はパラメータを与えます</param>
    /// <returns></returns>
    public static IObservable<Unit> NavigateAsync(string sceneName, object argument = null)
    {
        var sceneParam = new SceneParam(sceneName, argument, isStack: true, loadSceneMode: LoadSceneMode.Additive);
        return NavigateAsyncCore(sceneParam);
    }

    public static IObservable<Unit> NavigateAsync(IEnumerable<string> sceneNames, object argument = null)
    {
        return Observable.WhenAll(sceneNames.Select((sceneName, idx) => NavigateAsyncCore(
            new SceneParam(sceneName, argument, isStack: idx == 0 ? true : false, loadSceneMode: LoadSceneMode.Additive))));
    }

    public static IObservable<Unit> NavigateSingleAsync(string sceneName, object argument = null)
    {
        if (isNavigating) { return Observable.ReturnUnit(); }
        return Observable.ReturnUnit()
            .Do(_ => RemoveAllJornals())
            .SelectMany(NavigateAsyncCore(new SceneParam(
                sceneName,
                argument,
                isStack: false,
                LoadSceneMode.Single)));
    }

    /// <summary>
    /// 文字列で画面遷移します。
    /// /シーン名1@{jsonパラメータ}/シーン名2のように記述します。
    /// シーンにパラメータを渡す場合は `@` の横に{jsonパラメータ}を記述します。
    /// jsonパラメータはSceneBase<T>のTクラスのプロパティに相当します。
    /// </summary>
    /// <param name="uri">e.g. /QuestList@{""id"":3}/UnitList/ADV</param>
    public static void NavigateTo(string uri)
    {
        var sceneParams = Parse(uri);
        foreach (var sceneParam in sceneParams)
        {
            if (sceneParam == sceneParams.Last())
            {
                NavigateAsyncCore(sceneParam).Subscribe();
                continue;
            }
            sceneStack.Push(sceneParam);
        }
    }

    public static void RemoveAllJornals()
    {
        sceneStack.Clear();
    }

    public static bool CanGoBack()
    {
        return (sceneStack.Count > 0);
    }

    /// <summary>
    /// シーンをスタックからポップし、ひとつ手前のシーンに戻ります。
    /// 戻れなければ何もしません。
    /// </summary>
    public static IObservable<Unit> GoBackAsync()
    {
        if (isNavigating || !CanGoBack()) return Observable.Return(Unit.Default);

        var currentSceneParam = sceneStack.Pop();
        return currentSceneParam.SceneBase.BackSceneAsync().SelectMany(x =>
        {
            return Observable.FromCoroutine<Unit>(_ => UnloadSceneAsync(currentSceneParam.sceneName));
        });
    }

    static IObservable<Unit> NavigateAsyncCore(SceneParam sceneParam)
    {
        if (CanGoBack())
        {
            var current = sceneStack.Peek();
            if (current.sceneName == sceneParam.sceneName)
            {
                return Observable.ReturnUnit();
            }
        }
        return Observable.ReturnUnit()
            .Do(_ => isNavigating = true)
            .SelectMany(Observable.FromCoroutine(observer => CoNavigateAsync(new SceneParam[] { sceneParam })))
            .Do(_ => isNavigating = false);
    }

    public static IEnumerator UnloadSceneAsync(string sceneName)
    {
        yield return SceneManager.UnloadSceneAsync(sceneName).AsObservable().ToYieldInstruction();
        if (sceneStack.Count > 0)
        {
            var current = sceneStack.Peek();
            if (current.SceneBase != null)
            {
                current.SceneBase.gameObject.SetActive(true);
            }
            else
            {
                yield return LoadSceneAsync(current, true);
            }
            if (current.SceneBase != null)
            {
                yield return current.SceneBase.StartSceneAsync().ToYieldInstruction();
            }
        }
    }

    static IEnumerator DispatchAsync<T>(T t)
    {
        yield return AsyncBroker.PublishAsync<T>(t).ToYieldInstruction();
    }

    static IEnumerator CoNavigateAsync(IEnumerable<SceneParam> sceneParams)
    {
        if (CanGoBack())
        {
            var current = sceneStack.Peek();
            if (current.SceneBase != null)
            {
                yield return current.SceneBase.BehindSceneAsync().ToYieldInstruction();
                current.SceneBase.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("sceneStack SceneBase on top is null.");
            }
        }

        yield return DispatchAsync<AsyncBeginLoadSceneEvent>(new AsyncBeginLoadSceneEvent());
        foreach (var sceneParam in sceneParams)
        {
            yield return LoadSceneAsync(sceneParam, false);
        }
        yield return DispatchAsync<AsyncAfterLoadSceneEvent>(new AsyncAfterLoadSceneEvent());

        foreach (var sceneParam in sceneParams)
        {
            if (sceneParam.SceneBase != null) yield return sceneParam.SceneBase.StartSceneAsync().ToYieldInstruction();
        }
    }

    static IEnumerator LoadSceneAsync(SceneParam sceneParam, bool isRestore)
    {
        yield return loader.LoadSceneAsync(sceneParam.sceneName, sceneParam.LoadSceneMode, container =>
        {
            if (isRestore) { return; }

            container.Bind<object>()
            .WithId("SceneBaseArgument")
            .FromInstance(sceneParam.Argument).AsSingle();
        });

        var sceneBase = SceneManager.GetSceneByName(sceneParam.sceneName)
        .GetRootGameObjects()
        .Select(x => x.GetComponentInChildren<SceneBase>())
        .Where(x => x != null)
        .FirstOrDefault(x => !x.IsLoaded);

        if (sceneBase != null)
        {
            sceneBase.IsLoaded = true;
            sceneParam.SceneBase = sceneBase;
            //TODO:　文字列で遷移してきたと決め打ちしている
            if (isRestore)
            {
                sceneBase.ArgParseTo(sceneParam);
            }
        }

        if (isRestore)
        {
            if (sceneBase != null) yield return sceneBase.PrepareSceneAsync().ToYieldInstruction();
            if (sceneBase != null) yield return sceneBase.OnRestoreSceneAsync().ToYieldInstruction();
        }
        else
        {
            if (sceneParam.IsStack) sceneStack.Push(sceneParam);
            if (sceneBase != null) yield return sceneBase.PrepareSceneAsync().ToYieldInstruction();
        }
    }

    static IEnumerable<SceneParam> Parse(string uri)
    {
        var sceneParams = new List<SceneParam>();
        foreach (var scene in uri.Split('/'))
        {
            var splitted = scene.Split('@');
            if (splitted.Length > 1)
            {
                sceneParams.Add(new SceneParam(splitted[0], splitted[1], true, UnityEngine.SceneManagement.LoadSceneMode.Additive));
            }
            else
            {
                sceneParams.Add(new SceneParam(splitted[0], null, true, UnityEngine.SceneManagement.LoadSceneMode.Additive));
            }
        }
        return sceneParams;
    }

}