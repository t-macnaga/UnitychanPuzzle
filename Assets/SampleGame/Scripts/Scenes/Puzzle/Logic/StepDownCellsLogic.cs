using System.Collections.Generic;
using System.Linq;

public class StepDownCellsLogic
{
    SwapCellsLogic swapCellsLogic = new SwapCellsLogic();
    public void Log(PuzzleContext context, ref List<PuzzleLogData> logList)
    {
        var boardModel = context.Model;
        var boardSize = boardModel.BoardSize;
        var childLogList = new List<PuzzleLogData>();
        var stepDownList = new List<(int fromIndex, int toIndex)>();
        // 下のほうから下のほうにずらす。
        for (var x = 0; x < boardSize.x; x++)
        {
            stepDownList.Clear();
            for (var y = boardSize.y - 1; y >= 0; y--)
            {
                var emptyModel = boardModel.cellModelGrid[y, x];
                if (emptyModel.cellType != PuzzleCellType.Empty) continue;
                var modelIndex = -1;
                for (var yy = y - 1; yy >= 0; yy--)
                {
                    var model = boardModel.cellModelGrid[yy, x];
                    if (model.cellType != PuzzleCellType.Empty)
                    {
                        modelIndex = model.index;
                        break;
                    }
                }
                if (modelIndex != -1)
                {
                    stepDownList.Add((modelIndex, emptyModel.index));
                    swapCellsLogic.Swap(context, emptyModel.index, modelIndex);
                }
            }
            if (stepDownList.Count > 0)
            {
                var logData = context.Logic.CreateLogData();
                logData.logType = PuzzleLogType.StepDownCells;
                logData.stepDownList = stepDownList.ToList();
                childLogList.Add(logData);
            }
        }
        if (childLogList.Count > 0)
        {
            var logData = context.Logic.CreateLogData();
            logData.logType = PuzzleLogType.Parallel;
            logData.children = childLogList;
            logList.Add(logData);
        }
    }
}
