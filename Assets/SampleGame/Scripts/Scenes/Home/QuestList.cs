using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
using Zenject;

public class QuestList : SceneBase
{
    [Inject(Id = "buttonPrototype")] Button button;
    [Inject] TransitionController transitionController;

    void Awake()
    {
        button.gameObject.SetActive(false);
    }

    public override IObservable<SceneBase> PrepareSceneAsync()
    {
        return base.PrepareSceneAsync()
        .ContinueWith(new FakeQuestListAPI().RequestAsync())
        .ContinueWith(x =>
        {
            foreach (var quest in x.quests)
            {
                var clone = GameObject.Instantiate(button, button.transform.parent, false);
                clone.gameObject.SetActive(true);
                clone.GetComponentInChildren<Text>().text = quest.name;
                clone.OnClickAsObservable().Subscribe(_ =>
                {
                    transitionController.LoadScene("Puzzle", TransitionController.LoadingMode.LongLoading, quest);
                });
            }
            return Observable.Return(this);
        });
    }
}