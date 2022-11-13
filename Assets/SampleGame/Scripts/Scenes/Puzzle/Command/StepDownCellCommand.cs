using System.Collections;
using UnityEngine;
using DG.Tweening;

public class StepDownCellCommand : IPuzzleCommand
{
    float duration = 0.1F;
    public IEnumerator ExecuteAsync(PuzzleContext context, PuzzleLogData log)
    {
        foreach (var stepDown in log.stepDownList)
        {
            var fromIndex = stepDown.fromIndex;
            var toIndex = stepDown.toIndex;
            var cellA = context.View.GetCell(fromIndex);
            var cellB = context.View.GetCell(toIndex);
            var localPosA = cellA.transform.localPosition;
            var localPosB = cellB.transform.localPosition;
            cellA.SetIndex(toIndex);
            cellB.SetIndex(fromIndex);
            var temp = context.View.cellList[fromIndex];
            context.View.cellList[fromIndex] = context.View.cellList[toIndex];
            context.View.cellList[toIndex] = temp;
            cellA.transform.DOLocalMove(localPosB, duration).SetEase(Ease.InOutQuad);
            cellB.transform.DOLocalMove(localPosA, duration).SetEase(Ease.InOutQuad);
            yield return WaitForCache.Seconds(duration);
        }
    }
}
