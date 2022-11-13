using Lib.Commands;
using System;
using UniRx;

public interface IPuzzleCommand : ICommand<PuzzleContext, PuzzleLogData>
{
}

public class PuzzleCommands
{
    public CommandExecutionContext<PuzzleContext, PuzzleLogData> ExecutionContext { get; set; }
        = new CommandExecutionContext<PuzzleContext, PuzzleLogData>();
    public bool IsBusy { get; private set; }
    public bool AnyLog() => ExecutionContext.LogQueue.Count > 0;

    public PuzzleCommands()
    {
        ExecutionContext.OnBeforeExecuteCommand += OnBeforeExecuteCommand;
        ExecutionContext.OnAfterExecuteCommand += OnAfterExecuteCommand;
        ExecutionContext.Commands.Add((int)PuzzleLogType.Parallel, new ParallelCommand());
        ExecutionContext.Commands.Add((int)PuzzleLogType.AttackCells, new AttackCellsCommand());
        ExecutionContext.Commands.Add((int)PuzzleLogType.GenerateCells, new GenerateCellsCommand());
        ExecutionContext.Commands.Add((int)PuzzleLogType.RemoveCells, new RemoveCellCommand());
        ExecutionContext.Commands.Add((int)PuzzleLogType.StepDownCells, new StepDownCellCommand());
        ExecutionContext.Commands.Add((int)PuzzleLogType.SwapCells, new SwapCellCommand());
    }

    public void Enqueue(PuzzleLogData log)
    {
        ExecutionContext.LogQueue.Enqueue(log);
    }

    void OnBeforeExecuteCommand(ILogData log)
    {
        IsBusy = true;
    }

    void OnAfterExecuteCommand(ILogData log)
    {
        IsBusy = false;
    }

    public IDisposable RunRoutine(PuzzleContext context)
    => Observable.FromCoroutine(_ => ExecutionContext.ExecuteAsync(context)).Subscribe();
}