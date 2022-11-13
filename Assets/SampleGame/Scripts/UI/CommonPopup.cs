using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class CommonPopup : MonoBehaviour
{
    [SerializeField] Button yesButton = default;
    [SerializeField] Button noButton = default;
    [SerializeField] Button closeButton = default;
    [SerializeField] Transform window = default;

    static public IObservable<Unit> Show(Action yes)
    {
        var go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/CommonPopup"));
        PopupManager.Instance.Push(go);

        var script = go.GetComponent<CommonPopup>();
        return script.yesButton.OnClickAsObservable().Do(x =>
        {
            PopupManager.Instance.Pop();
            yes();
        });
    }

    void Awake()
    {
        noButton.OnClickAsObservable().Subscribe(_ => Close());
        closeButton.OnClickAsObservable().Subscribe(_ => Close());
    }

    void OnEnable()
    {
        // window.localScale = Vector3.zero;
        // window.transform.DOScale(Vector3.one, 1f);
    }

    void Close()
    {
        // window.transform.DOScale(Vector3.zero, 1f)
        // .OnCompleteAsObservable()
        // .Subscribe(_ =>
        {
            PopupManager.Instance.Pop();
        }
        //);
    }
}
