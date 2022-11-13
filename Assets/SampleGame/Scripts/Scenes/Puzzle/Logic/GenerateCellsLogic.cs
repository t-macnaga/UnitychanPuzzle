using System.Collections.Generic;
using UnityEngine;

public class GenerateCellsLogic
{
    public void Log(PuzzleContext context, ref List<PuzzleLogData> logList)
    {
        var boardModel = context.Model;
        var boardSize = boardModel.BoardSize;
        var targetCellList = new List<LogTargetCell>();
        // 空いてるところに埋める
        for (var x = 0; x < boardSize.x; x++)
        {
            for (var y = 0; y < boardSize.y; y++)
            {
                var model = boardModel.cellModelGrid[y, x];
                if (model.cellType == PuzzleCellType.Empty)
                {
                    model.cellType = PuzzleCellType.SelfCell;
                    model.unitType = RandomUnitType();
                    model.unitModel = null;
                    var logTarget = context.Logic.CreateLogTarget(model.index, model.cellType, model.unitType);
                    targetCellList.Add(logTarget);
                }
            }
        }
        if (targetCellList.Count > 0)
        {
            var logData = context.Logic.CreateLogData();
            logData.logType = PuzzleLogType.GenerateCells;
            logData.targetCellList = targetCellList;
            logList.Add(logData);
        }
    }

    //TODO:
    PuzzleUnitType RandomUnitType()
    {
        var types = new PuzzleUnitType[]
        {
            PuzzleUnitType.TypeA,
            PuzzleUnitType.TypeB,
            PuzzleUnitType.TypeC,
            PuzzleUnitType.TypeD,
            PuzzleUnitType.TypeE,
        };
        return types[Random.Range(0, 4)];
    }
}
