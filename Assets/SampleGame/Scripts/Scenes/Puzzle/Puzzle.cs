using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class Puzzle : SceneBase<PuzzleQuest>
{
    [SerializeField] Button backButton = default;
    [SerializeField] Text text = default;
    [SerializeField] PuzzleDirector director;

    void Awake()
    {
        if (Argument == null)
        {
            var quest = new FakeQuestListResponse().quests[0];
            SetupDirector(quest);
        }
    }

    public override IObservable<SceneBase> PrepareSceneAsync()
    {
        backButton.OnClickAsObservable().Subscribe(_ =>
        {
            TransitionController.Instance.LoadScene("Home", TransitionController.LoadingMode.LongLoading);
        });
        var quest = Argument as PuzzleQuest;
        SetupDirector(quest);
        return Observable.Return(this);
    }

    void SetupDirector(PuzzleQuest quest)
    {
        text.text = quest.name;
        director.Setup(quest);
    }
}
