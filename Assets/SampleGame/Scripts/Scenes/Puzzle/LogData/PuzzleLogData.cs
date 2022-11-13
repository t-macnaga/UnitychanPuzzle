using System.Collections.Generic;
using Lib.Commands;

public class PuzzleLogData : ILogData
{
    public int Id => (int)logType;
    public PuzzleLogType logType;
    public List<LogTargetCell> targetCellList = new List<LogTargetCell>();
    public List<(int fromIndex, int toIndex)> stepDownList = new List<(int, int)>();
    public List<PuzzleLogData> children = new List<PuzzleLogData>();

    public void Clear()
    {
        logType = default;
        targetCellList.Clear();
        stepDownList.Clear();
        children.Clear();
    }
}