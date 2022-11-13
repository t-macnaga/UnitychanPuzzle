using System.Collections;
using UnityEngine;

public class RemoveCellCommand : IPuzzleCommand
{
    float duration = 0.3F;
    public IEnumerator ExecuteAsync(PuzzleContext context, PuzzleLogData log)
    {
        foreach (var target in log.targetCellList)
        {
            var cell = context.View.GetCell(target.index);
            cell.PlayRemove();
        }
        yield return WaitForCache.Seconds(duration);
    }
}
