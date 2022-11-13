using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class BackKeyHandler : MonoBehaviour
{
    [SerializeField] bool enablePopWhenAndroidBackKeyDown = true;

    void Start()
    {
        if (!enablePopWhenAndroidBackKeyDown)
        {
            Destroy(this);
            return;
        }

        this.UpdateAsObservable()
            .Where(x => Input.GetKeyDown(KeyCode.Escape))
            .Where(_ => true) //TODO: 禁止するフラグ作る
            .Subscribe(x =>
        {
            NavigationService.Broker.Publish<BackButtonEvent>(new BackButtonEvent());
        });
    }
}
