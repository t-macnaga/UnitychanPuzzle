using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class TransitionController : MonoBehaviour
{
    public enum LoadingMode
    {
        // ローディング画面を挟まない
        None,
        // 長めのローディングになりそうな場合
        LongLoading,
    }

    [SerializeField] GameObject view = default;
    [SerializeField] GameObject block = default;
    [SerializeField] Material transitionMaterial = default;
    LoadingMode loadingMode = LoadingMode.None;
    public LoadingMode Mode
    {
        get => loadingMode;
        set => loadingMode = value;
    }

    public void LoadScene(string sceneName, LoadingMode mode = LoadingMode.None, object argument = null)
    {
        loadingMode = mode;
        NavigationService.NavigateSingleAsync(sceneName, argument)
        .Subscribe(_ => { }, e => Debug.LogException(e));
    }

    public void LoadSceneAdditive(string sceneName, object argument = null, LoadingMode mode = LoadingMode.None)
    {
        loadingMode = mode;
        NavigationService.NavigateAsync(sceneName, argument)
        .Subscribe(_ => { }, e => Debug.LogException(e));
    }

    public void LoadSceneAdditive(IEnumerable<string> sceneNames, LoadingMode mode = LoadingMode.None, object argument = null)
    {
        loadingMode = mode;
        NavigationService.NavigateAsync(sceneNames, argument)
        .Subscribe(_ => { }, e => Debug.LogException(e));
    }

    static public TransitionController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);

        NavigationService.AsyncBroker.Subscribe<AsyncBeginLoadSceneEvent>(_ => Observable.FromCoroutine(Show)).AddTo(this);
        NavigationService.AsyncBroker.Subscribe<AsyncAfterLoadSceneEvent>(_ => Observable.FromCoroutine(Hide)).AddTo(this);
        NavigationService.Broker.Receive<BackButtonEvent>().Subscribe(_ => OnBackEvent()).AddTo(this);
        NavigationService.Broker.Receive<TapBlockEvent>().Subscribe(_ => block.SetActive(true)).AddTo(this);
        NavigationService.Broker.Receive<TapUnblockEvent>().Subscribe(_ => block.SetActive(false)).AddTo(this);
    }

    void OnDestroy()
    {
        Instance = null;
    }

    IEnumerator Show()
    {
        if (loadingMode == LoadingMode.LongLoading)
        {
            view.SetActive(true);

            var cutOff = 0F;
            while (cutOff < 1F)
            {
                cutOff += 0.05f;
                transitionMaterial.SetFloat("_CutOff", cutOff);
                yield return null;
            }
        }
    }

    IEnumerator Hide()
    {
        if (loadingMode == LoadingMode.LongLoading)
        {
            var cutOff = 1F;
            while (cutOff > 0F)
            {
                transitionMaterial.SetFloat("_CutOff", cutOff);
                cutOff -= 0.05f;
                yield return null;
            }
            view.SetActive(false);
        }
    }

    void OnBackEvent()
    {
        if (PopupManager.Instance.CanGoBack())
        {
            PopupManager.Instance.Pop();
            return;
        }

        if (NavigationService.CanGoBack())
        {
            NavigationService.GoBackAsync().Subscribe();
            return;
        }

        NavigationService.Broker.Publish<NoMoreBackEvent>(new NoMoreBackEvent());
    }

}
