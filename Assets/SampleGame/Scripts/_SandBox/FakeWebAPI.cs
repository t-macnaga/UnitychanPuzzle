using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class FakeQuestListAPI
{
    public IObservable<FakeQuestListResponse> RequestAsync()
    {
        // レスポンスをでっちあげる適当なコード
        return Observable.ReturnUnit()
        .Do(_ => NavigationService.Broker.Publish<TapBlockEvent>(new TapBlockEvent()))
        .ContinueWith(Observable.Timer(System.TimeSpan.FromSeconds(0.3f)))
        .Do(_ => NavigationService.Broker.Publish<TapUnblockEvent>(new TapUnblockEvent()))
        .Select(x => new FakeQuestListResponse());
    }
}

public class FakeQuestListResponse
{
    public PuzzleQuest[] quests = new PuzzleQuest[]
    {
            new PuzzleQuest{name="Quest1",enemies = new PuzzleUnitModel[]
            {
                    new PuzzleUnitModel
                    {
                        hp = 3,
                        maxHp = 3,
                        maxTurn = 3,
                        turn = 3,
                        initialIndex = 22,
                        unitType = PuzzleUnitType.TypeA
                    },
                    new PuzzleUnitModel
                    {
                        hp = 3,
                        maxHp = 3,
                        maxTurn = 3,
                        turn = 3,
                        initialIndex = 27,
                        unitType = PuzzleUnitType.TypeA
                    },
                    // new PuzzleUnitModel
                    // {
                    //     hp = 3,
                    //     maxHp = 3,
                    //     maxTurn = 3,
                    //     turn = 3,
                    //     initialIndex = 62,
                    //     unitType = PuzzleUnitType.TypeA
                    // },
                    // new PuzzleUnitModel
                    // {
                    //     hp = 3,
                    //     maxHp = 3,
                    //     maxTurn = 3,
                    //     turn = 3,
                    //     initialIndex = 67,
                    //     unitType = PuzzleUnitType.TypeA
                    // },
                    // new PuzzleUnitModel
                    // {
                    //     hp = 3,
                    //     maxHp = 3,
                    //     maxTurn = 3,
                    //     turn = 3,
                    //     initialIndex = 85,
                    //     unitType = PuzzleUnitType.TypeA
                    // },
            }},
    };
}

public class Quest
{
    public string name;
}