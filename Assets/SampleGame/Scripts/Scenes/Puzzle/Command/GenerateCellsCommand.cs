using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lib.Commands;
using DG.Tweening;

public class GenerateCellsCommand : IPuzzleCommand
{
    float duration = 0.3F;
    public IEnumerator ExecuteAsync(PuzzleContext context, PuzzleLogData log)
    {
        foreach (var targetCell in log.targetCellList)
        {
            var cell = context.View.GetCell(targetCell.index);
            cell.Setup(targetCell.index, targetCell.cellType, targetCell.unitType, null);
            var rect = cell.transform as RectTransform;
            var pos = rect.anchoredPosition;
            var fromPos = pos;
            fromPos.y = 100F;
            rect.DOAnchorPos(pos, duration).From(fromPos).SetEase(Ease.InOutQuad);
        }
        yield return WaitForCache.Seconds(duration);
    }
}
