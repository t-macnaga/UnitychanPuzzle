using System.Collections.Generic;

public class SwapCellsLogic
{
    RemoveCellsLogic removeCellsLogic = new RemoveCellsLogic();
    public void Swap(PuzzleContext context, int indexA, int indexB)
    {
        var cellA = context.Model.Get(indexA);
        var cellB = context.Model.Get(indexB);
        var cellPosA = cellA.position;
        var cellTypeA = cellA.cellType;
        var unitTypeA = cellA.unitType;
        var cellPosB = cellB.position;
        var cellTypeB = cellB.cellType;
        var unitTypeB = cellB.unitType;
        context.Model.cellModelList[indexA].cellType = cellTypeB;
        context.Model.cellModelList[indexA].unitType = unitTypeB;
        context.Model.cellModelList[indexB].cellType = cellTypeA;
        context.Model.cellModelList[indexB].unitType = unitTypeA;
        var temp = context.Model.cellModelList[indexA].unitModel;
        context.Model.cellModelList[indexA].unitModel = context.Model.cellModelList[indexB].unitModel;
        context.Model.cellModelList[indexB].unitModel = temp;
    }

    public void TrySwap(PuzzleContext context, int index, SwipeDirection direction, ref List<PuzzleLogData> logList)
    {
        var cellA = context.Model.Get(index);
        if (cellA.cellType != PuzzleCellType.SelfCell) return;
        var positionA = cellA.position;
        var positionB = context.Model.GetNeiborPosition(positionA, direction);
        if (0 <= positionB.x && positionB.x < context.Model.BoardSize.x &&
            0 <= positionB.y && positionB.y < context.Model.BoardSize.y)
        {
            var cellB = context.Model.cellModelGrid[positionB.y, positionB.x];
            if (cellB.cellType == PuzzleCellType.EnemyCell) return;

            Swap(context, index, cellB.index);
            LogSwapCells(context, ref logList, index, cellB.index);
            removeCellsLogic.Log(context, ref logList);
            if (removeCellsLogic.RemovableCellIndices.Count <= 0)
            {
                Swap(context, index, cellB.index);
                LogSwapCells(context, ref logList, index, cellB.index);
            }
            removeCellsLogic.Clear();
        }
    }

    void LogSwapCells(PuzzleContext context, ref List<PuzzleLogData> logList, int indexA, int indexB)
    {
        var logData = context.Logic.CreateLogData();
        logData.logType = PuzzleLogType.SwapCells;
        logData.targetCellList.Add(context.Logic.CreateLogTarget(indexA));
        logData.targetCellList.Add(context.Logic.CreateLogTarget(indexB));
        logList.Add(logData);
    }
}
