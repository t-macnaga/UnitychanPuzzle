using UnityEngine;
using UniRx;
using System.Linq;
using Lib.Commands;
using System.Collections;

public class PuzzleDirector : MonoBehaviour
{
    [SerializeField] PuzzleBoardView view;
    [SerializeField] Vector2Int boardSize;
    public PuzzleContext Context { get; set; } = new PuzzleContext();

    void Update()
    {
        Context.StateMachine?.Update();
    }

    public void Setup(PuzzleQuest quest)
    {
        Context.Director = this;
        Context.View = view;
        Context.Model = new PuzzleBoardModel(quest, boardSize);
        Context.Logic = new PuzzleLogic();
        Context.Commands = new PuzzleCommands();
        Context.Commands.ExecutionContext.OnAfterExecuteCommand += OnAfterExecuteCommand;
        Context.Commands.RunRoutine(Context).AddTo(this);
        Context.StateMachine = new PuzzleStateMachine();

        view.Create(Context.Model);
        MessageBroker.Default.Receive<PuzzleCellSwipeMessage>()
            .Where(_ => !Context.Commands.IsBusy)
            .Subscribe(OnPuzzleCellSwipeMessage).AddTo(this);

        // TODO: remove.
        EvaluateBoard();
        Context.StateMachine.Initialize(Context);
    }

    void OnPuzzleCellSwipeMessage(PuzzleCellSwipeMessage message)
    {
        var result = Context.Logic.TrySwapLog(Context, message.Index, message.Direction);
        foreach (var logData in result.logList)
        {
            Context.Commands.Enqueue(logData);
        }
    }

    void OnAfterExecuteCommand(PuzzleLogData command)
    {
        EvaluateBoard();
    }

    void EvaluateBoard()
    {
        var result = Context.Logic.Log(Context);
        if (!Context.Commands.AnyLog() &&
             Context.Model.Quest.enemies.All(x => x.IsDead))
        {
            Context.StateMachine.ChangeResultState();
            return;
        }
        foreach (var logData in result.logList)
        {
            Context.Commands.Enqueue(logData);
        }
    }

    public void BackToHome()
    {
        TransitionController.Instance.LoadScene("Home", TransitionController.LoadingMode.LongLoading);
    }
}
