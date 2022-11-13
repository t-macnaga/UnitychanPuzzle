using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwapCellCommand : IPuzzleCommand
{
    float duration = 0.3F;
    public IEnumerator ExecuteAsync(PuzzleContext context, PuzzleLogData log)
    {
        var indexA = log.targetCellList[0].index;
        var indexB = log.targetCellList[1].index;
        var cellA = context.View.GetCell(indexA);
        var cellB = context.View.GetCell(indexB);
        var localPosA = cellA.transform.localPosition;
        var localPosB = cellB.transform.localPosition;
        cellA.SetIndex(indexB);
        cellB.SetIndex(indexA);
        var temp = context.View.cellList[indexA];
        context.View.cellList[indexA] = context.View.cellList[indexB];
        context.View.cellList[indexB] = temp;
        cellA.transform.DOLocalMove(localPosB, duration);
        cellB.transform.DOLocalMove(localPosA, duration);
        yield return WaitForCache.Seconds(duration);
    }
}
