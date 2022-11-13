using UnityEngine;
using UniRx;
using System.Linq;
using Lib.Commands;

public class PuzzleDirector : MonoBehaviour
{
    [SerializeField] PuzzleBoardView view;
    [SerializeField] Vector2Int boardSize;
    public PuzzleContext Context { get; set; } = new PuzzleContext();

    public void Setup(PuzzleQuest quest)
    {
        Context.Director = this;
        Context.View = view;
        Context.Model = new PuzzleBoardModel(quest, boardSize);
        Context.Logic = new PuzzleLogic();
        Context.Commands = new PuzzleCommands();
        Context.Commands.ExecutionContext.OnAfterExecuteCommand += OnAfterExecuteCommand;
        Context.Commands.RunRoutine(Context).AddTo(this);

        view.Create(Context.Model);
        MessageBroker.Default.Receive<PuzzleCellSwipeMessage>()
            .Where(_ => !Context.Commands.IsBusy)
            .Subscribe(OnPuzzleCellSwipeMessage).AddTo(this);

        // TODO: remove.
        EvaluateBoard();
    }

    void OnPuzzleCellSwipeMessage(PuzzleCellSwipeMessage message)
    {
        var logList = Context.Logic.TrySwapLog(Context, message.Index, message.Direction);
        foreach (var logData in logList)
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
        var logList = Context.Logic.Log(Context);
        foreach (var logData in logList)
        {
            Context.Commands.Enqueue(logData);
        }
    }
}
