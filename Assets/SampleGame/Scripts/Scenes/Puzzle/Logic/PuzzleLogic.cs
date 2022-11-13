using System.Collections.Generic;
using TouchScript.Utils;

public class PuzzleLogic
{
    RemoveCellsLogic removeCellsLogic = new RemoveCellsLogic();
    StepDownCellsLogic stepDownCellsLogic = new StepDownCellsLogic();
    GenerateCellsLogic generateCellsLogic = new GenerateCellsLogic();
    SwapCellsLogic swapCellsLogic = new SwapCellsLogic();
    List<PuzzleLogData> resultLogList = new List<PuzzleLogData>();
    ObjectPool<PuzzleLogData> logDataPool;
    ObjectPool<LogTargetCell> logTargetPool;

    public PuzzleLogic()
    {
        logDataPool = new ObjectPool<PuzzleLogData>(1, () => new PuzzleLogData(), null, (log) => log.Clear());
        logTargetPool = new ObjectPool<LogTargetCell>(1, () => new LogTargetCell(), null, (log) => log.Clear());
    }

    public IReadOnlyList<PuzzleLogData> Log(PuzzleContext context)
    {
        resultLogList.Clear();
        removeCellsLogic.Log(context, ref resultLogList);
        stepDownCellsLogic.Log(context, ref resultLogList);
        generateCellsLogic.Log(context, ref resultLogList);
        return resultLogList;
    }

    public IReadOnlyList<PuzzleLogData> TrySwapLog(PuzzleContext context, int index, SwipeDirection swipeDirection)
    {
        resultLogList.Clear();
        swapCellsLogic.TrySwap(context, index, swipeDirection, ref resultLogList);
        return resultLogList;
    }

    internal PuzzleLogData CreateLogData() => logDataPool.Get();
    internal LogTargetCell CreateLogTarget(
        int index,
        PuzzleCellType cellType = default,
        PuzzleUnitType unitType = default)
    {
        var log = logTargetPool.Get();
        log.index = index;
        log.cellType = cellType;
        log.unitType = unitType;
        return log;
    }
}
