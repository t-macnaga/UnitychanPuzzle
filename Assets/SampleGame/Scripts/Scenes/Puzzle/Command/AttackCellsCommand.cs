using System.Collections;
using UnityEngine;
using DG.Tweening;
using Lib.Commands;

public class AttackCellsCommand : IPuzzleCommand
{
    float duration = 0.3F;
    public IEnumerator ExecuteAsync(PuzzleContext context, PuzzleLogData log)
    {
        foreach (var target in log.targetCellList)
        {
            var cell = context.View.GetCell(target.index);
            cell.transform.DOPunchPosition(new Vector3(5f, 5f, 0), duration, 5, 1f);
            cell.PlayDamage();
        }
        yield return WaitForCache.Seconds(duration);
    }
}
