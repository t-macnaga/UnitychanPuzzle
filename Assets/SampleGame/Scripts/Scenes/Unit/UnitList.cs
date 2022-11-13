using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
using Zenject;

public class UnitEntity
{
    public int id;
    public string name;
}

public class UnitList : Root
{
    [Inject] UnitCell.Factory factory;

    public override IObservable<SceneBase> PrepareSceneAsync()
    {
        return base.PrepareSceneAsync()
        .ContinueWith(x =>
        {
            var units = new UnitEntity[]{
                new UnitEntity{id=1,name="Uniちゃん"},
                new UnitEntity{id=1,name="Zenちゃん"},
            };
            foreach (var unit in units)
            {
                var clone = factory.Create();
                clone.Setup(unit);
                clone.gameObject.SetActive(true);
                // clone.GetComponentInChildren<Text>().text = quest.name;
                // clone.OnClickAsObservable().Subscribe(_ =>
                // {
                //     CommonPopup.Show(() =>
                //     {
                //         TransitionController.Instance.LoadScene("InGame", TransitionController.LoadingMode.LongLoading, quest);
                //     }).Subscribe();
                // });
            }
            return Observable.Return(this);
        });
    }
}