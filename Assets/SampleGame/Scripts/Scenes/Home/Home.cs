using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

public class Home : SceneBase
{
    [Inject(Id = "quest")] Button questButton;
    [Inject(Id = "menu")] Button menuButton;
    [Inject(Id = "advButton")] Button advButton;
    [Inject(Id = "unitButton")] Button unitButton;
    [Inject] IMessageBroker Broker;
    [Inject] TransitionController transitionController;

    void Start()
    {
        // var param1 = new SceneParam("QuestList", data, true, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        // var param2 = new SceneParam("UnitList", null, true, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        // var param3 = new SceneParam("ADV", null, true, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        // NavigationService.NavigateTo(new SceneParam[] { param1, param2, param3 });

        // NavigationService.NavigateTo(@"/QuestList@{""id"":3,""hoge"":1}/UnitList/ADV");

        questButton.OnClickAsObservable().Subscribe(_ =>
        {
            transitionController.LoadSceneAdditive("QuestList");//, argument: data);// new QuestListPropery { id = 2 });
        });
        menuButton.OnClickAsObservable().Subscribe(_ =>
        {
            Debug.Log("clicked menu.");
            // TransitionController.Instance.LoadSceneAdditive("ZenQuestList");
        });
        unitButton.OnClickAsObservable().Subscribe(_ =>
        {
            transitionController.LoadSceneAdditive("UnitList");
        });
    }
}