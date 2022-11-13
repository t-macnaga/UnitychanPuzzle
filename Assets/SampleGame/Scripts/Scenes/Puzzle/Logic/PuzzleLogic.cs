using System.Collections.Generic;
using TouchScript.Utils;

public class PuzzleLogResult
{
    public List<PuzzleLogData> logList = new List<PuzzleLogData>();
}

public class PuzzleLogic
{
    RemoveCellsLogic removeCellsLogic = new RemoveCellsLogic();
    StepDownCellsLogic stepDownCellsLogic = new StepDownCellsLogic();
    GenerateCellsLogic generateCellsLogic = new GenerateCellsLogic();
    SwapCellsLogic swapCellsLogic = new SwapCellsLogic();
    PuzzleLogResult result = new PuzzleLogResult();
    ObjectPool<PuzzleLogData> logDataPool;
    ObjectPool<LogTargetCell> logTargetPool;

    public PuzzleLogic()
    {
        logDataPool = new ObjectPool<PuzzleLogData>(1, () => new PuzzleLogData(), null, (log) => log.Clear());
        logTargetPool = new ObjectPool<LogTargetCell>(1, () => new LogTargetCell(), null, (log) => log.Clear());
    }

    public PuzzleLogResult Log(PuzzleContext context)
    {
        result.logList.Clear();
        removeCellsLogic.Log(context, ref result.logList);
        stepDownCellsLogic.Log(context, ref result.logList);
        generateCellsLogic.Log(context, ref result.logList);
        return result;
    }

    public PuzzleLogResult TrySwapLog(PuzzleContext context, int index, SwipeDirection swipeDirection)
    {
        result.logList.Clear();
        swapCellsLogic.TrySwap(context, index, swipeDirection, ref result.logList);
        return result;
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
