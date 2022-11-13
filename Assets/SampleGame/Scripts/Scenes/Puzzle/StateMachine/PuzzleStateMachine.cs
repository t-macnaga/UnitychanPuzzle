using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PuzzleStateMachine
{
    StateMachine<PuzzleContext> StateMachine { get; } = new StateMachine<PuzzleContext>();
    PuzzleStartState startState = new PuzzleStartState();
    PuzzlePlayState playState = new PuzzlePlayState();
    PuzzleResultState resultState = new PuzzleResultState();

    public void Initialize(PuzzleContext context)
    {
        StateMachine.Context = context;
        StateMachine.ChangeState(startState);
    }

    public void Update()
    {
        StateMachine.Update();
    }

    public void Clear()
    {
        StateMachine?.Dispose();
    }

    public void ChangePlayState()
    {
        StateMachine.ChangeState(playState);
    }

    public void ChangeResultState()
    {
        StateMachine.ChangeState(resultState);
    }
}

public class PuzzleState : IState<PuzzleContext>
{
    public virtual void Enter(PuzzleContext context) { }
    public virtual void Update(PuzzleContext context) { }
    public virtual void Exit(PuzzleContext context) { }
}

public class PuzzleStartState : PuzzleState
{
    public override void Enter(PuzzleContext context)
    {
        Debug.Log("Enter Puzzle Start");
        Observable.Timer(System.TimeSpan.FromSeconds(1F))
            .Subscribe(_ => context.StateMachine.ChangePlayState());
    }

    public override void Exit(PuzzleContext context)
    {
        Debug.Log("Exit Puzzle Start");
    }
}

public class PuzzlePlayState : PuzzleState
{
    public override void Enter(PuzzleContext context)
    {
        Debug.Log("Enter Puzzle Play");
    }
    public override void Exit(PuzzleContext context)
    {
        Debug.Log("Exit Puzzle Play");
    }
}

public class PuzzleResultState : PuzzleState
{
    public override void Enter(PuzzleContext context)
    {
        Debug.Log("Win!");
    }

    public override void Update(PuzzleContext context)
    {
        if (Input.GetMouseButtonDown(0))
        {
            context.Director.BackToHome();
        }
    }
}